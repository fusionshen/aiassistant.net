using AI_Assistant_Win.Models;
using AI_Assistant_Win.Models.Middle;
using AI_Assistant_Win.Utils;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AI_Assistant_Win.Business
{
    public class GaugeBlockMethodBLL
    {
        private readonly SQLiteConnection connection = SQLiteHandler.Instance.GetSQLiteConnection();

        private readonly ApiBLL apiBLL = ApiHandler.Instance.GetApiBLL();

        public List<CircularAreaSummaryHistory> GetSummaryListByConditions(DateTime? startDate, DateTime? endDate, string text)
        {
            //var allSummary = connection.Table<ScaleAccuracyTracer>().ToList();
            //var allMethod = connection.Table<GaugeBlockMethodResult>().ToList();
            //var all = allSummary.Select(t => new CircularAreaSummaryHistory
            //{
            //    Summary = t,
            //    MethodList = [.. allMethod.Where(x => t.TestNo.Equals(x.TestNo) && t.CoilNumber.Equals(x.CoilNumber)).OrderBy(x => x.Position)]
            //}).ToList();
            //var filtered = all.Where(t => Filter(t, startDate, endDate, text)).ToList();
            //var sorted = filtered.OrderByDescending(t => t.Summary.CreateTime).ToList();
            //return sorted;
            return null;
        }

        private bool Filter(CircularAreaSummaryHistory t, DateTime? startDate, DateTime? endDate, string text)
        {
            //var result = true;
            //if (startDate != null)
            //{
            //    result = result && t.Summary.CreateTime != null && t.Summary.CreateTime >= startDate;
            //}
            //if (endDate != null)
            //{
            //    result = result && t.Summary.CreateTime != null && t.Summary.CreateTime <= endDate;
            //}
            //if (!string.IsNullOrEmpty(text))
            //{
            //    result = result && (t.Summary.Id.ToString().Equals(text) ||
            //        (!string.IsNullOrEmpty(t.Summary.TestNo) && t.Summary.TestNo.Contains(text)) ||
            //        (!string.IsNullOrEmpty(t.Summary.CoilNumber) && t.Summary.CoilNumber.Contains(text)) ||
            //        (t.Summary.IsUploaded && text.Equals("已上传")) ||
            //        (!t.Summary.IsUploaded && text.Equals("未上传")) ||
            //        (!string.IsNullOrEmpty(t.Summary.Creator) && t.Summary.Creator.Contains(text)) ||
            //        (!string.IsNullOrEmpty(t.Summary.Uploader) && t.Summary.Uploader.Contains(text)) ||
            //        (!string.IsNullOrEmpty(t.Summary.LastReviser) && t.Summary.LastReviser.Contains(text))
            //        );
            //}
            //return result;
            return true;
        }

        public GaugeBlockMethodResult GetResultById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new Exception(LocalizeHelper.ID_IS_EMPTY);
            }
            // if t.Id.ToString().Equals(id), will throw not function toString(), funny!
            var item = connection.Table<GaugeBlockMethodResult>().FirstOrDefault(t => t.Id.Equals(id));
            return item;
        }

        public int SaveResult(GaugeBlockResult tempGaugeBlockResult)
        {
            connection.BeginTransaction();
            try
            {
                var result = AddOrUpdateByTransaction(tempGaugeBlockResult);
                connection.Commit();
                return result.Id;
            }
            catch (Exception)
            {
                connection.Rollback();
                throw;
            }
        }

        public GaugeBlockMethodResult GetResultExitsInDB(GaugeBlockResult tempGaugeBlockResult)
        {
            var target = connection.Table<GaugeBlockMethodResult>()
                .FirstOrDefault(t => tempGaugeBlockResult.CalculateScale.Id.Equals(t.ScaleId) &&
                                     tempGaugeBlockResult.InputEdgeLength.Equals(t.InputLength));
            return target;
        }

        private GaugeBlockMethodResult AddOrUpdateByTransaction(GaugeBlockResult tempGaugeBlockResult)
        {
            var target = new GaugeBlockMethodResult();
            if (tempGaugeBlockResult.Id == 0)
            {
                target = new GaugeBlockMethodResult
                {
                    OriginImagePath = tempGaugeBlockResult.OriginImagePath,
                    RenderImagePath = tempGaugeBlockResult.RenderImagePath,
                    ScaleId = tempGaugeBlockResult.Item.CalculateScale.Id, //tempBlacknessResult.CalculateScale.Id,
                    Confidence = tempGaugeBlockResult.Item.Confidence,
                    Pixels = tempGaugeBlockResult.Item.AreaOfPixels,
                    Area = tempGaugeBlockResult.Item.CalculatedArea,
                    VertexPositions = tempGaugeBlockResult.Item.VertexPositonsText,
                    EdgePixels = string.Join(" ", tempGaugeBlockResult.Item.EdgePixels.Select(t => $"{t.Key}={t.Value:F2}")),
                    CalculatdEdges = string.Join(" ", tempGaugeBlockResult.Item.CalculatedEdgeLengths.Select(t => $"{t.Key}={t.Value:F2}{tempGaugeBlockResult.Item.LengthUnit}")),
                    Prediction = JsonConvert.SerializeObject(tempGaugeBlockResult.Item.Prediction, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }),
                    WorkGroup = tempGaugeBlockResult.WorkGroup,
                    Analyst = tempGaugeBlockResult.Analyst,
                    InputEdge = tempGaugeBlockResult.InputEdge,
                    CalculatedLength = tempGaugeBlockResult.Item.CalculatedEdgeLengths[tempGaugeBlockResult.InputEdge],
                    InputLength = float.Parse(tempGaugeBlockResult.InputEdgeLength),
                    LengthAccuracy = $"{1 - Math.Abs(tempGaugeBlockResult.Item.CalculatedEdgeLengths[tempGaugeBlockResult.InputEdge] - float.Parse(tempGaugeBlockResult.InputEdgeLength)) / Math.Abs(float.Parse(tempGaugeBlockResult.InputEdgeLength)):P2}",
                    AreaAccuracy = $"{Math.Pow(float.Parse(tempGaugeBlockResult.InputEdgeLength), 2) * tempGaugeBlockResult.Item.CalculatedArea / Math.Pow(tempGaugeBlockResult.Item.CalculatedEdgeLengths[tempGaugeBlockResult.InputEdge], 2):F4}{LocalizeHelper.SQUARE_MILLIMETER} {1 - Math.Abs(tempGaugeBlockResult.Item.CalculatedArea - Math.Pow(float.Parse(tempGaugeBlockResult.InputEdgeLength), 2) * tempGaugeBlockResult.Item.CalculatedArea / Math.Pow(tempGaugeBlockResult.Item.CalculatedEdgeLengths[tempGaugeBlockResult.InputEdge], 2)) / Math.Abs(Math.Pow(float.Parse(tempGaugeBlockResult.InputEdgeLength), 2) * tempGaugeBlockResult.Item.CalculatedArea / Math.Pow(tempGaugeBlockResult.Item.CalculatedEdgeLengths[tempGaugeBlockResult.InputEdge], 2)):P2}",
                    CreateTime = DateTime.Now
                };
                var ok = connection.Insert(target);
                if (ok == 0)
                {
                    throw new Exception(LocalizeHelper.ADD_SUBJECT_FAILED);
                }
            }
            else // update
            {
                // find result
                target = GetResultById(tempGaugeBlockResult.Id.ToString()) ?? throw new Exception(LocalizeHelper.FIND_SUBJECT_FAILED);
                target.OriginImagePath = tempGaugeBlockResult.OriginImagePath;
                target.RenderImagePath = tempGaugeBlockResult.RenderImagePath;
                target.ScaleId = tempGaugeBlockResult.Item.CalculateScale.Id; //tempBlacknessResult.CalculateScale.Id,
                target.Confidence = tempGaugeBlockResult.Item.Confidence;
                target.Pixels = tempGaugeBlockResult.Item.AreaOfPixels;
                target.Area = tempGaugeBlockResult.Item.CalculatedArea;
                target.VertexPositions = tempGaugeBlockResult.Item.VertexPositonsText;
                target.EdgePixels = string.Join(" ", tempGaugeBlockResult.Item.EdgePixels.Select(t => $"{t.Key}={t.Value:F2}"));
                target.CalculatdEdges = string.Join(" ", tempGaugeBlockResult.Item.CalculatedEdgeLengths.Select(t => $"{t.Key}={t.Value:F2}{tempGaugeBlockResult.Item.LengthUnit}"));
                target.Prediction = JsonConvert.SerializeObject(tempGaugeBlockResult.Item.Prediction, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                target.WorkGroup = tempGaugeBlockResult.WorkGroup;
                target.InputEdge = tempGaugeBlockResult.InputEdge;
                target.CalculatedLength = tempGaugeBlockResult.Item.CalculatedEdgeLengths[tempGaugeBlockResult.InputEdge];
                target.InputLength = float.Parse(tempGaugeBlockResult.InputEdgeLength);
                target.LengthAccuracy = $"{1 - Math.Abs(tempGaugeBlockResult.Item.CalculatedEdgeLengths[tempGaugeBlockResult.InputEdge] - float.Parse(tempGaugeBlockResult.InputEdgeLength)) / Math.Abs(float.Parse(tempGaugeBlockResult.InputEdgeLength)):P2}";
                target.AreaAccuracy = $"{Math.Pow(float.Parse(tempGaugeBlockResult.InputEdgeLength), 2) * tempGaugeBlockResult.Item.CalculatedArea / Math.Pow(tempGaugeBlockResult.Item.CalculatedEdgeLengths[tempGaugeBlockResult.InputEdge], 2):F4}{LocalizeHelper.SQUARE_MILLIMETER} {1 - Math.Abs(tempGaugeBlockResult.Item.CalculatedArea - Math.Pow(float.Parse(tempGaugeBlockResult.InputEdgeLength), 2) * tempGaugeBlockResult.Item.CalculatedArea / Math.Pow(tempGaugeBlockResult.Item.CalculatedEdgeLengths[tempGaugeBlockResult.InputEdge], 2)) / Math.Abs(Math.Pow(float.Parse(tempGaugeBlockResult.InputEdgeLength), 2) * tempGaugeBlockResult.Item.CalculatedArea / Math.Pow(tempGaugeBlockResult.Item.CalculatedEdgeLengths[tempGaugeBlockResult.InputEdge], 2)):P2}";
                target.LastReviser = $"{apiBLL.LoginUserInfo.Username}-{apiBLL.LoginUserInfo.Nickname}";
                target.LastModifiedTime = DateTime.Now;
                var ok = connection.Update(target);
                if (ok == 0)
                {
                    throw new Exception(LocalizeHelper.UPDATE_SUBJECT_FAILED);
                }
            }
            AddOrUpdateTracer(target);
            return target;
        }

        private void AddOrUpdateTracer(GaugeBlockMethodResult gaugeBlockMethodResult)
        {
            // 查询tracer是否存在
            var tracer = connection.Table<ScaleAccuracyTracer>()
                .FirstOrDefault(t => gaugeBlockMethodResult.ScaleId == t.ScaleId && gaugeBlockMethodResult.InputLength == t.MeasuredLength);
            if (tracer == null)
            {
                // 构造tracer
                tracer = new ScaleAccuracyTracer
                {
                    ScaleId = gaugeBlockMethodResult.ScaleId,
                    MeasuredLength = gaugeBlockMethodResult.InputLength,
                    Creator = gaugeBlockMethodResult.Analyst,
                    CreateTime = DateTime.Now
                };
                var ok = connection.Insert(tracer);
                if (ok == 0)
                {
                    throw new Exception(LocalizeHelper.ADD_ACCURACY_TRACER_FAILED);
                }
            }
            else
            {
                tracer.LastReviser = $"{apiBLL.LoginUserInfo.Username}-{apiBLL.LoginUserInfo.Nickname}";
                tracer.LastModifiedTime = DateTime.Now;
                var ok = connection.Update(tracer);
                if (ok == 0)
                {
                    throw new Exception(LocalizeHelper.UPDATE_ACCURACY_TRACER_FAILED);
                }
            }
            // 更新该比例尺下的所有精度信息
            var tracersInSameScale = connection.Table<ScaleAccuracyTracer>().Where(t => gaugeBlockMethodResult.ScaleId == t.ScaleId);
            foreach (var scaleTracer in tracersInSameScale)
            {
                // 获取相同scale下所有的测量数据
                var resultsInSameScale = connection.Table<GaugeBlockMethodResult>().Where(t => scaleTracer.ScaleId == t.ScaleId).ToList();
                // mpe
                var mpe = resultsInSameScale.Max(t => Math.Abs(t.CalculatedLength - scaleTracer.MeasuredLength));
                scaleTracer.MPE = mpe;
                // 获取相同scale下相同(scaleTracer.MeasuredLength)的测量数据
                var resultsInMeasurement = connection.Table<GaugeBlockMethodResult>().Where(t => scaleTracer.ScaleId == t.ScaleId && scaleTracer.MeasuredLength == t.InputLength).ToList();
                // 1. 输入校验
                if (resultsInMeasurement != null && resultsInMeasurement.Count >= 2)
                {
                    // 2. 计算样本均值
                    var mean = resultsInMeasurement.Average(t => t.CalculatedLength);
                    scaleTracer.Average = mean;
                    // 3. 计算样本方差（无偏估计，分母为n-1）
                    double sumOfSquares = resultsInMeasurement.Sum(x => Math.Pow(x.CalculatedLength - mean, 2));
                    double variance = sumOfSquares / (resultsInMeasurement.Count - 1);
                    // 4. 计算标准差（随机误差）
                    double standardDeviation = Math.Sqrt(variance);
                    scaleTracer.StandardDeviation = standardDeviation;
                    // 5. 计算标准误差, 平均值的标准不确定度：
                    double standardError = standardDeviation / Math.Sqrt(resultsInMeasurement.Count);
                    scaleTracer.StandardError = standardError;
                    // 合成不确定度
                    double uncertainty = Math.Sqrt(Math.Pow(mpe, 2) + Math.Pow(standardError, 2));
                    scaleTracer.Uncertainty = uncertainty;
                    FormatConfidence(scaleTracer, uncertainty, resultsInMeasurement.Select(t => t.CalculatedLength).ToArray());
                }
                var ok = connection.Update(scaleTracer);
                if (ok == 0)
                {
                    throw new Exception(LocalizeHelper.UPDATE_ACCURACY_TRACER_FAILED);
                }
            }
        }

        private void FormatConfidence(ScaleAccuracyTracer scaleTracer, double uncertainty, double[] doubles)
        {
            var (pct1Sigma, pct2Sigma, pct3Sigma) = CalculateSigmaConfidence(doubles);
            scaleTracer.Pct1Sigma = pct1Sigma;
            scaleTracer.Pct2Sigma = pct2Sigma;
            scaleTracer.Pct3Sigma = pct3Sigma;
            if (pct1Sigma < 0.6827f)
            {
                scaleTracer.DisplayName = $"±{uncertainty:F4}mm(μ±1σ:{pct1Sigma:P2})(理论68.27%)";
            }
            else
            {
                if (pct2Sigma < 0.9545f)
                {
                    scaleTracer.DisplayName = $"±{uncertainty:F4}mm(k=1,68.27%)(实际{pct1Sigma:P2})";
                }
                else
                {
                    if (pct3Sigma < 0.9973f)
                    {
                        scaleTracer.DisplayName = $"±{uncertainty:F4}mm(k=2,95.45%)(实际{pct2Sigma:P2})";
                    }
                    else
                    {
                        scaleTracer.DisplayName = $"±{uncertainty:F4}mm(k=3,99.73%)(实际{pct3Sigma:P2})";
                    }
                }
            }
        }

        /// <summary>
        /// 计算数据落在 μ±kσ 范围内的比例（k=1,2,3）
        /// 宽松控制（k=2）：允许 5% 的误差超限（如一般工业检测）；
        /// 严格控制（k=3）：仅允许 0.3% 的误差超限（如航空航天、医疗设备）。
        /// </summary>
        /// <param name="data">输入数据数组</param>
        /// <returns>元组：(比例1σ, 比例2σ, 比例3σ)</returns>
        private (double pct1Sigma, double pct2Sigma, double pct3Sigma)
            CalculateSigmaConfidence(double[] data)
        {
            // 1. 输入校验
            if (data == null || data.Length == 0)
                throw new ArgumentException("输入数据不能为null或空数组");

            // 2. 计算均值和标准差
            double mean = data.Average();
            double variance = data.Sum(x => Math.Pow(x - mean, 2)) / (data.Length - 1);
            double stdDev = Math.Sqrt(variance);

            // 3. 统计落在不同区间的数量
            int count1Sigma = 0, count2Sigma = 0, count3Sigma = 0;
            foreach (double x in data)
            {
                double deviation = Math.Abs(x - mean);
                if (deviation <= 1 * stdDev) count1Sigma++;
                if (deviation <= 2 * stdDev) count2Sigma++;
                if (deviation <= 3 * stdDev) count3Sigma++;
            }

            // 4. 计算比例（置信度）
            double total = data.Length;
            return (
                count1Sigma / total,
                count2Sigma / total,
                count3Sigma / total
            );
        }

        public List<int> LoadOriginalResultFromDB(GaugeBlockResult originalGaugeBlockResult, string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                originalGaugeBlockResult.Id = 0;
                originalGaugeBlockResult.OriginImagePath = string.Empty;
                originalGaugeBlockResult.RenderImagePath = string.Empty;
                originalGaugeBlockResult.WorkGroup = string.Empty;
                originalGaugeBlockResult.Analyst = $"{apiBLL.LoginUserInfo.Username}-{apiBLL.LoginUserInfo.Nickname}";
                originalGaugeBlockResult.InputEdge = string.Empty;
                originalGaugeBlockResult.InputEdgeLength = string.Empty;
                originalGaugeBlockResult.Item = null;
                return [];
            }
            // sorted by testNo，then sorted by position
            var allSorted = connection.Table<GaugeBlockMethodResult>().OrderBy(t => t.CreateTime).ThenBy(t => t.ScaleId).ThenBy(t => t.InputLength).ToList();
            var body = allSorted.FirstOrDefault(t => t.Id.ToString().Equals(id)) ?? throw new Exception($"{LocalizeHelper.CERTAIN_ID(id)}{LocalizeHelper.HAVE_NO_SUBJECT}，{LocalizeHelper.PLEASE_CONTACT_ADMIN}");
            var scaleAtThatTime = connection.Table<CalculateScale>().FirstOrDefault(x => x.Id.Equals(body.ScaleId));
            originalGaugeBlockResult.Id = body.Id;
            originalGaugeBlockResult.OriginImagePath = body.OriginImagePath;
            originalGaugeBlockResult.RenderImagePath = body.RenderImagePath;
            originalGaugeBlockResult.WorkGroup = body.WorkGroup;
            originalGaugeBlockResult.Analyst = body.Analyst;
            originalGaugeBlockResult.CalculateScale = scaleAtThatTime; // must before items
            originalGaugeBlockResult.Item = new GaugeBlock(JsonConvert.DeserializeObject<QuadrilateralSegmentation>(body.Prediction), scaleAtThatTime);
            originalGaugeBlockResult.IsUploaded = GetOrAddTracerExitsInDB(body).IsUploaded; // promot before saving
            return allSorted.Select(t => t.Id).ToList();
        }
        public List<ScaleAccuracyTracer> GetTracerListByScaleId(string scaleId)
        {
            if (string.IsNullOrEmpty(scaleId))
            {
                throw new Exception(LocalizeHelper.ID_IS_EMPTY);
            }
            var list = connection.Table<ScaleAccuracyTracer>().Where(t => t.ScaleId.Equals(scaleId)).ToList();
            return list;
        }

        /// <summary>
        /// show MPE Precision、Accuracy、Uncertainty
        /// part1: 刻度0
        /// part2: 0.15毫米/像素边长
        /// part3: MPE=±0.06mm
        /// part4: ±0.6135mm(k=3,99.73%)(实际100.00%)
        /// </summary>
        /// <param name="calculateScale"></param>
        /// <returns></returns>
        public (string topGraduations, string lengthScale, string areaScale, string mpe, string accuracy) GetScaleParts(CalculateScale calculateScale)
        {
            // 刻度0;0.15毫米/像素边长;MPE=±0.06mm;±0.6135mm(k=3,99.73%)(实际100.00%)
            var settings = JsonConvert.DeserializeObject<GaugeBlockScaleItem>(calculateScale.Settings);
            var tracers = GetTracerListByScaleId(calculateScale.Id.ToString());
            var tracerInMaxMPE = tracers.OrderBy(t => t.MPE).FirstOrDefault();
            if (tracerInMaxMPE != null)
            {
                return (
                    $"{LocalizeHelper.SCALE_GRADE_TITLE}{settings?.TopGraduations}",
                    $"{calculateScale.Value:F2}{LocalizeHelper.LENGTH_SCALE_CACULATED_RATIO_UNIT}",
                    $"{Math.Pow(calculateScale.Value, 2):F4}{LocalizeHelper.AREA_SCALE_CACULATED_RATIO_UNIT}",
                    $"MPE=±{tracerInMaxMPE.MPE:F4}mm",
                    $"{tracerInMaxMPE.DisplayName}"
                 );
            }
            else
            {
                return (
                    $"{LocalizeHelper.SCALE_GRADE_TITLE}{settings?.TopGraduations}",
                    $"{calculateScale.Value:F2}{LocalizeHelper.LENGTH_SCALE_CACULATED_RATIO_UNIT}",
                    $"{Math.Pow(calculateScale.Value, 2):F4}{LocalizeHelper.AREA_SCALE_CACULATED_RATIO_UNIT}",
                    string.Empty,
                    string.Empty
                );
            }
        }
        public ScaleAccuracyTracer GetOrAddTracerExitsInDB(GaugeBlockMethodResult result)
        {
            var target = connection.Table<ScaleAccuracyTracer>()
                .FirstOrDefault(t => result.ScaleId == t.ScaleId &&
                                     result.InputLength == t.MeasuredLength);
            if (target == null)
            {
                // 补齐tracer
                var summary = new ScaleAccuracyTracer
                {
                    ScaleId = result.ScaleId,
                    MeasuredLength = result.InputLength,
                    Creator = result.Analyst,
                    CreateTime = DateTime.Now
                };
                var ok = connection.Insert(summary);
                if (ok == 0)
                {
                    throw new Exception(LocalizeHelper.ADD_ACCURACY_TRACER_FAILED);
                }
            }
            return target;
        }

        public CalculateScale GetCurrentScale()
        {
            return connection.Table<CalculateScale>().OrderByDescending(x => x.Id).FirstOrDefault(x => x.Key.Equals("GaugeBlock"));
        }

        public void SaveScaleSetting(CalculateScale add)
        {
            var ok = connection.Insert(add);
            if (ok == 0)
            {
                throw new Exception(LocalizeHelper.ADD_SUBJECT_FAILED);
            }
        }
    }
}
