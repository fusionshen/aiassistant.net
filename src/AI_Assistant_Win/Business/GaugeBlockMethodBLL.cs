using AI_Assistant_Win.Models;
using AI_Assistant_Win.Models.Enums;
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
            var allSummary = connection.Table<CircularAreaMethodSummary>().ToList();
            var allMethod = connection.Table<CircularAreaMethodResult>().ToList();
            var all = allSummary.Select(t => new CircularAreaSummaryHistory
            {
                Summary = t,
                MethodList = [.. allMethod.Where(x => t.TestNo.Equals(x.TestNo) && t.CoilNumber.Equals(x.CoilNumber)).OrderBy(x => x.Position)]
            }).ToList();
            var filtered = all.Where(t => Filter(t, startDate, endDate, text)).ToList();
            var sorted = filtered.OrderByDescending(t => t.Summary.CreateTime).ToList();
            return sorted;
        }

        private bool Filter(CircularAreaSummaryHistory t, DateTime? startDate, DateTime? endDate, string text)
        {
            var result = true;
            if (startDate != null)
            {
                result = result && t.Summary.CreateTime != null && t.Summary.CreateTime >= startDate;
            }
            if (endDate != null)
            {
                result = result && t.Summary.CreateTime != null && t.Summary.CreateTime <= endDate;
            }
            if (!string.IsNullOrEmpty(text))
            {
                result = result && (t.Summary.Id.ToString().Equals(text) ||
                    (!string.IsNullOrEmpty(t.Summary.TestNo) && t.Summary.TestNo.Contains(text)) ||
                    (!string.IsNullOrEmpty(t.Summary.CoilNumber) && t.Summary.CoilNumber.Contains(text)) ||
                    (t.Summary.IsUploaded && text.Equals("已上传")) ||
                    (!t.Summary.IsUploaded && text.Equals("未上传")) ||
                    (!string.IsNullOrEmpty(t.Summary.Creator) && t.Summary.Creator.Contains(text)) ||
                    (!string.IsNullOrEmpty(t.Summary.Uploader) && t.Summary.Uploader.Contains(text)) ||
                    (!string.IsNullOrEmpty(t.Summary.LastReviser) && t.Summary.LastReviser.Contains(text))
                    );
            }
            return result;
        }

        public CircularAreaMethodResult GetResultById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new Exception(LocalizeHelper.ID_IS_EMPTY);
            }
            // if t.Id.ToString().Equals(id), will throw not function toString(), funny!
            var item = connection.Table<CircularAreaMethodResult>().FirstOrDefault(t => t.Id.Equals(id));
            return item;
        }

        public int SaveResult(GaugeBlockResult tempCircularAreaResult)
        {
            connection.BeginTransaction();
            try
            {
                var result = AddOrUpdateByTransaction(tempCircularAreaResult);
                connection.Commit();
                return result.Id;
            }
            catch (Exception)
            {
                connection.Rollback();
                throw;
            }
        }

        public Dictionary<CircularPositionKind, string> PositionList => new()
        {
            { CircularPositionKind.UPPER_SURFACE_OP , LocalizeHelper.CIRCULAR_POSITION(CircularPositionKind.UPPER_SURFACE_OP)},
            { CircularPositionKind.UPPER_SURFACE_CE , LocalizeHelper.CIRCULAR_POSITION(CircularPositionKind.UPPER_SURFACE_CE)},
            { CircularPositionKind.UPPER_SURFACE_DR , LocalizeHelper.CIRCULAR_POSITION(CircularPositionKind.UPPER_SURFACE_DR)},
            { CircularPositionKind.LOWER_SURFACE_OP , LocalizeHelper.CIRCULAR_POSITION(CircularPositionKind.LOWER_SURFACE_OP)},
            { CircularPositionKind.LOWER_SURFACE_CE , LocalizeHelper.CIRCULAR_POSITION(CircularPositionKind.LOWER_SURFACE_CE)},
            { CircularPositionKind.LOWER_SURFACE_DR , LocalizeHelper.CIRCULAR_POSITION(CircularPositionKind.LOWER_SURFACE_DR)}
        };

        public GaugeBlockMethodResult GetResultExitsInDB(GaugeBlockResult tempGaugeBlockResult)
        {
            var target = connection.Table<GaugeBlockMethodResult>()
                .FirstOrDefault(t => tempGaugeBlockResult.CalculateScale.Id.Equals(t.ScaleId) &&
                                     tempGaugeBlockResult.InputEdgeLength.Equals(t.MeasuredLength));
            return target;
        }

        private GaugeBlockMethodResult AddOrUpdateByTransaction(GaugeBlockResult tempCircularAreaResult)
        {
            var target = GetResultExitsInDB(tempCircularAreaResult);
            if (target == null)
            {
                target = new GaugeBlockMethodResult
                {
                    OriginImagePath = tempCircularAreaResult.OriginImagePath,
                    RenderImagePath = tempCircularAreaResult.RenderImagePath,
                    ScaleId = tempCircularAreaResult.Item.CalculateScale.Id, //tempBlacknessResult.CalculateScale.Id,
                    Pixels = tempCircularAreaResult.Item.AreaOfPixels,
                    Confidence = tempCircularAreaResult.Item.Confidence,
                    Area = tempCircularAreaResult.Item.CalculatedArea,
                    //Diameter = tempCircularAreaResult.Item.Diameter,
                    Prediction = JsonConvert.SerializeObject(tempCircularAreaResult.Item.Prediction, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }),
                    WorkGroup = tempCircularAreaResult.WorkGroup,
                    //TestNo = tempCircularAreaResult.TestNo,
                    //CoilNumber = tempCircularAreaResult.CoilNumber,
                    //Position = PositionList.FirstOrDefault(t => tempCircularAreaResult.Position.Equals(t.Value)).Key,
                    Analyst = tempCircularAreaResult.Analyst,
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
                target.OriginImagePath = tempCircularAreaResult.OriginImagePath;
                target.RenderImagePath = tempCircularAreaResult.RenderImagePath;
                target.ScaleId = tempCircularAreaResult.Item.CalculateScale.Id; //tempBlacknessResult.CalculateScale.Id,
                target.Pixels = tempCircularAreaResult.Item.AreaOfPixels;
                target.Confidence = tempCircularAreaResult.Item.Confidence;
                target.Area = tempCircularAreaResult.Item.CalculatedArea;
                //target.Diameter = tempCircularAreaResult.Item.Diameter;
                target.Prediction = JsonConvert.SerializeObject(tempCircularAreaResult.Item.Prediction, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                target.WorkGroup = tempCircularAreaResult.WorkGroup;
                //target.TestNo = tempCircularAreaResult.TestNo;
                //target.CoilNumber = tempCircularAreaResult.CoilNumber;
                //target.Position = PositionList.FirstOrDefault(t => tempCircularAreaResult.Position.Equals(t.Value)).Key;
                target.LastReviser = $"{apiBLL.LoginUserInfo.Username}-{apiBLL.LoginUserInfo.Nickname}";
                target.LastModifiedTime = DateTime.Now;
                var ok = connection.Update(target);
                if (ok == 0)
                {
                    throw new Exception(LocalizeHelper.UPDATE_SUBJECT_FAILED);
                }
            }
            AddOrUpdateSummary(target);
            return target;
        }

        private void AddOrUpdateSummary(GaugeBlockMethodResult circularAreaMethodResult)
        {
            // 查询summary是否存在
            //var summary = connection.Table<CircularAreaMethodSummary>().FirstOrDefault(t => circularAreaMethodResult.ScaleId.Equals(t.TestNo) && circularAreaMethodResult.CoilNumber.Equals(t.CoilNumber));
            //if (summary == null)
            //{
            //    // 构造summary
            //    summary = new CircularAreaMethodSummary
            //    {
            //        TestNo = circularAreaMethodResult.TestNo,
            //        CoilNumber = circularAreaMethodResult.CoilNumber,
            //        Creator = circularAreaMethodResult.Analyst,
            //        CreateTime = DateTime.Now
            //    };
            //    var ok = connection.Insert(summary);
            //    if (ok == 0)
            //    {
            //        throw new Exception(LocalizeHelper.ADD_SUMMARY_FAILED);
            //    }
            //}
            //else
            //{
            //    summary.LastReviser = $"{apiBLL.LoginUserInfo.Username}-{apiBLL.LoginUserInfo.Nickname}";
            //    summary.LastModifiedTime = DateTime.Now;
            //    var ok = connection.Update(summary);
            //    if (ok == 0)
            //    {
            //        throw new Exception(LocalizeHelper.UPDATE_SUMMARY_FAILED);
            //    }
            //}
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
            var allSorted = connection.Table<CircularAreaMethodResult>().OrderBy(t => t.CreateTime).ThenBy(t => t.TestNo).ThenBy(t => t.Position).ToList();
            var body = allSorted.FirstOrDefault(t => t.Id.ToString().Equals(id)) ?? throw new Exception($"{LocalizeHelper.CERTAIN_ID(id)}{LocalizeHelper.HAVE_NO_SUBJECT}，{LocalizeHelper.PLEASE_CONTACT_ADMIN}");
            var scaleAtThatTime = connection.Table<CalculateScale>().FirstOrDefault(x => x.Id.Equals(body.ScaleId));
            originalGaugeBlockResult.Id = body.Id;
            //originalGaugeBlockResult.TestNo = body.TestNo;
            //originalGaugeBlockResult.Position = PositionList.FirstOrDefault(t => body.Position.Equals(t.Key)).Value;
            //originalGaugeBlockResult.CoilNumber = body.CoilNumber;
            originalGaugeBlockResult.OriginImagePath = body.OriginImagePath;
            originalGaugeBlockResult.RenderImagePath = body.RenderImagePath;
            originalGaugeBlockResult.WorkGroup = body.WorkGroup;
            originalGaugeBlockResult.Analyst = body.Analyst;
            originalGaugeBlockResult.CalculateScale = scaleAtThatTime; // must before items
            originalGaugeBlockResult.Item = new GaugeBlock(JsonConvert.DeserializeObject<QuadrilateralSegmentation>(body.Prediction), scaleAtThatTime);
            originalGaugeBlockResult.IsUploaded = GetSummaryExitsInDB(body).IsUploaded; // promot before saving
            return allSorted.Select(t => t.Id).ToList();
        }

        public CircularAreaMethodSummary GetSummaryExitsInDB(CircularAreaMethodResult result)
        {
            var target = connection.Table<CircularAreaMethodSummary>()
                .FirstOrDefault(t => result.TestNo.Equals(t.TestNo) &&
                                     result.CoilNumber.Equals(t.CoilNumber));
            if (target == null)
            {
                // 补齐summary
                var summary = new CircularAreaMethodSummary
                {
                    TestNo = result.TestNo,
                    CoilNumber = result.CoilNumber,
                    Creator = result.Analyst,
                    CreateTime = DateTime.Now
                };
                var ok = connection.Insert(summary);
                if (ok == 0)
                {
                    throw new Exception(LocalizeHelper.ADD_SUMMARY_FAILED);
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
