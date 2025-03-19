using AI_Assistant_Win.Models;
using AI_Assistant_Win.Models.Enums;
using AI_Assistant_Win.Models.Middle;
using AI_Assistant_Win.Utils;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AI_Assistant_Win.Business
{
    public class CircularAreaMethodBLL
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
                    (t.Summary.IsExternal && text.Contains("委外")) ||
                    (!t.Summary.IsExternal && text.Contains("内部")) ||
                    (t.Summary.IsUploaded && text.Equals("已上传")) ||
                    (!t.Summary.IsUploaded && text.Equals("未上传")) ||
                    (!string.IsNullOrEmpty(t.Summary.Creator) && t.Summary.Creator.Contains(text)) ||
                    (!string.IsNullOrEmpty(t.Summary.Uploader) && t.Summary.Uploader.Contains(text)) ||
                    (!string.IsNullOrEmpty(t.Summary.LastReviser) && t.Summary.LastReviser.Contains(text))
                    );
            }
            return result;
        }

        public CircularAreaSummaryHistory GetSummaryHistoryById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new Exception(LocalizeHelper.ID_IS_EMPTY);
            }
            var item = connection.Table<CircularAreaMethodSummary>().ToList().Single(t => id.Equals(t.Id.ToString()));
            var methodList = connection.Table<CircularAreaMethodResult>()
                .Where(t => item.TestNo.Equals(t.TestNo) &&
                            item.CoilNumber.Equals(t.CoilNumber) &&
                            item.Nth.Equals(t.Nth))
                .OrderBy(t => t.Position).ToList();
            return new CircularAreaSummaryHistory
            {
                Summary = item,
                MethodList = methodList
            };
        }

        public CircularAreaSummaryHistory GetSummaryHistoryByMethodId(string methodId)
        {
            if (string.IsNullOrEmpty(methodId))
            {
                throw new Exception(LocalizeHelper.ID_IS_EMPTY);
            }
            var method = GetResultById(methodId) ?? throw new Exception(LocalizeHelper.FIND_SUBJECT_FAILED);
            var item = connection.Table<CircularAreaMethodSummary>().ToList().Single(t => method.TestNo.Equals(t.TestNo) &&
                            method.CoilNumber.Equals(t.CoilNumber) &&
                            method.Nth.Equals(t.Nth));
            var methodList = connection.Table<CircularAreaMethodResult>()
                .Where(t => item.TestNo.Equals(t.TestNo) &&
                            item.CoilNumber.Equals(t.CoilNumber) &&
                            item.Nth.Equals(t.Nth))
                .OrderBy(t => t.Position).ToList();
            return new CircularAreaSummaryHistory
            {
                Summary = item,
                MethodList = methodList
            };
        }
        public CircularAreaMethodResult GetResultById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new Exception(LocalizeHelper.ID_IS_EMPTY);
            }
            var item = connection.Table<CircularAreaMethodResult>().ToList().FirstOrDefault(t => id.Equals(t.Id.ToString()));
            return item;
        }

        public int SaveResult(CircularAreaResult tempCircularAreaResult)
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

        public CircularAreaMethodResult GetResultExitsInDB(CircularAreaResult tempCircularAreaResult)
        {
            var positionEnum = PositionList.FirstOrDefault(t => tempCircularAreaResult.Position.Equals(t.Value)).Key;
            var target = connection.Table<CircularAreaMethodResult>()
                .FirstOrDefault(t => tempCircularAreaResult.TestNo.Equals(t.TestNo) &&
                                     tempCircularAreaResult.CoilNumber.Equals(t.CoilNumber) &&
                                     positionEnum.Equals(t.Position));
            return target;
        }

        private CircularAreaMethodResult AddOrUpdateByTransaction(CircularAreaResult tempCircularAreaResult)
        {
            CircularAreaMethodResult target = new();
            if (tempCircularAreaResult.Id == 0)
            {
                target = new CircularAreaMethodResult
                {
                    OriginImagePath = tempCircularAreaResult.OriginImagePath,
                    RenderImagePath = tempCircularAreaResult.RenderImagePath,
                    ScaleId = tempCircularAreaResult.Item.CalculateScale.Id, //tempBlacknessResult.CalculateScale.Id,
                    Pixels = tempCircularAreaResult.Item.AreaOfPixels,
                    Confidence = tempCircularAreaResult.Item.Confidence,
                    Area = tempCircularAreaResult.Item.CalculatedArea,
                    Diameter = tempCircularAreaResult.Item.Diameter,
                    Prediction = JsonConvert.SerializeObject(tempCircularAreaResult.Item.Prediction, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }),
                    WorkGroup = tempCircularAreaResult.WorkGroup,
                    TestNo = tempCircularAreaResult.TestNo,
                    CoilNumber = tempCircularAreaResult.CoilNumber,
                    Position = PositionList.FirstOrDefault(t => tempCircularAreaResult.Position.Equals(t.Value)).Key,
                    Nth = tempCircularAreaResult.Nth,
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
                // find result
                target = GetResultById(tempCircularAreaResult.Id.ToString()) ?? throw new Exception(LocalizeHelper.FIND_SUBJECT_FAILED);
                target.OriginImagePath = tempCircularAreaResult.OriginImagePath;
                target.RenderImagePath = tempCircularAreaResult.RenderImagePath;
                target.ScaleId = tempCircularAreaResult.Item.CalculateScale.Id; //tempBlacknessResult.CalculateScale.Id,
                target.Pixels = tempCircularAreaResult.Item.AreaOfPixels;
                target.Confidence = tempCircularAreaResult.Item.Confidence;
                target.Area = tempCircularAreaResult.Item.CalculatedArea;
                target.Diameter = tempCircularAreaResult.Item.Diameter;
                target.Prediction = JsonConvert.SerializeObject(tempCircularAreaResult.Item.Prediction, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                target.WorkGroup = tempCircularAreaResult.WorkGroup;
                target.TestNo = tempCircularAreaResult.TestNo;
                target.CoilNumber = tempCircularAreaResult.CoilNumber;
                target.Nth = tempCircularAreaResult.Nth;
                target.Position = PositionList.FirstOrDefault(t => tempCircularAreaResult.Position.Equals(t.Value)).Key;
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

        public int? GetNthOfMethod(string testNo, string coilNumber, string position, int originResultId = 0)
        {
            if (string.IsNullOrEmpty(testNo) || string.IsNullOrEmpty(coilNumber) || string.IsNullOrEmpty(position))
            {
                throw new Exception(LocalizeHelper.INVALID_PARAMETER);
            }
            var positonKey = PositionList.FirstOrDefault(t => position.Equals(t.Value)).Key;
            if (originResultId != 0)
            {
                // find result
                var result = GetResultById(originResultId.ToString()) ?? throw new Exception(LocalizeHelper.FIND_SUBJECT_FAILED);
                if (testNo.Equals(result.TestNo) && coilNumber.Equals(result.CoilNumber) && positonKey.Equals(result.Position))
                {
                    return result.Nth;
                }
            }
            var item = connection.Table<CircularAreaMethodResult>()
                .Where(t => testNo.Equals(t.TestNo) && coilNumber.Equals(t.CoilNumber) && positonKey.Equals(t.Position))
                .OrderByDescending(t => t.Nth)
                .FirstOrDefault();
            return item == null ? 1 : item.Nth + 1;
        }

        private CircularAreaMethodSummary AddOrUpdateSummary(CircularAreaMethodResult circularAreaMethodResult)
        {
            // 查询summary是否存在
            var summary = connection.Table<CircularAreaMethodSummary>()
                .FirstOrDefault(t => circularAreaMethodResult.TestNo.Equals(t.TestNo) &&
                                     circularAreaMethodResult.CoilNumber.Equals(t.CoilNumber) &&
                                     circularAreaMethodResult.Nth.Equals(t.Nth));
            if (summary == null)
            {
                // 构造summary
                summary = new CircularAreaMethodSummary
                {
                    TestNo = circularAreaMethodResult.TestNo,
                    CoilNumber = circularAreaMethodResult.CoilNumber,
                    Nth = circularAreaMethodResult.Nth,
                    IsExternal = TaskHelper.SafeExecuteAsync(apiBLL.GetExternalTestNoListAsync("DXCZLCW", circularAreaMethodResult.TestNo.Split("-")[0])).Result.Count != 0,
                    Creator = circularAreaMethodResult.Analyst,
                    CreateTime = DateTime.Now
                };
                var ok = connection.Insert(summary);
                if (ok == 0)
                {
                    throw new Exception(LocalizeHelper.ADD_SUMMARY_FAILED);
                }
            }
            else
            {
                summary.LastReviser = $"{apiBLL.LoginUserInfo.Username}-{apiBLL.LoginUserInfo.Nickname}";
                summary.LastModifiedTime = DateTime.Now;
                summary.IsExternal = TaskHelper.SafeExecuteAsync(apiBLL.GetExternalTestNoListAsync("DXCZLCW", circularAreaMethodResult.TestNo.Split("-")[0])).Result.Count != 0;
                var ok = connection.Update(summary);
                if (ok == 0)
                {
                    throw new Exception(LocalizeHelper.UPDATE_SUMMARY_FAILED);
                }
            }
            return summary;
        }

        public List<int> LoadOriginalResultFromDB(CircularAreaResult originalCircularAreaResult, string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                originalCircularAreaResult.Id = 0;
                originalCircularAreaResult.TestNo = string.Empty;
                originalCircularAreaResult.CoilNumber = string.Empty;
                originalCircularAreaResult.Nth = null;
                originalCircularAreaResult.Position = string.Empty;
                originalCircularAreaResult.OriginImagePath = string.Empty;
                originalCircularAreaResult.RenderImagePath = string.Empty;
                originalCircularAreaResult.WorkGroup = string.Empty;
                originalCircularAreaResult.Analyst = $"{apiBLL.LoginUserInfo.Username}-{apiBLL.LoginUserInfo.Nickname}";
                originalCircularAreaResult.Item = null;
                return [];
            }
            // sorted by testNo，then sorted by position
            var allSorted = connection.Table<CircularAreaMethodResult>().OrderBy(t => t.TestNo).OrderBy(t => t.Nth).ThenBy(t => t.Position).ToList();
            var body = allSorted.FirstOrDefault(t => t.Id.ToString().Equals(id)) ?? throw new Exception($"{LocalizeHelper.CERTAIN_ID(id)}{LocalizeHelper.HAVE_NO_SUBJECT}，{LocalizeHelper.PLEASE_CONTACT_ADMIN}");
            var scaleAtThatTime = connection.Table<CalculateScale>().FirstOrDefault(x => x.Id.Equals(body.ScaleId));
            originalCircularAreaResult.Id = body.Id;
            originalCircularAreaResult.TestNo = body.TestNo;
            originalCircularAreaResult.Position = PositionList.FirstOrDefault(t => body.Position.Equals(t.Key)).Value;
            originalCircularAreaResult.CoilNumber = body.CoilNumber;
            originalCircularAreaResult.Nth = body.Nth;
            originalCircularAreaResult.OriginImagePath = body.OriginImagePath;
            originalCircularAreaResult.RenderImagePath = body.RenderImagePath;
            originalCircularAreaResult.WorkGroup = body.WorkGroup;
            originalCircularAreaResult.Analyst = body.Analyst;
            originalCircularAreaResult.CalculateScale = scaleAtThatTime; // must before items
            originalCircularAreaResult.Item = new CircularArea(JsonConvert.DeserializeObject<SimpleSegmentation>(body.Prediction), scaleAtThatTime);
            originalCircularAreaResult.IsUploaded = AddOrUpdateSummary(body).IsUploaded; // promot before saving
            return allSorted.Select(t => t.Id).ToList();
        }

        public async Task<List<TestItem>> GetTestNoListAsync()
        {
            var task = TaskHelper.SafeExecuteAsync(apiBLL.GetTestNoListAsync("G1SH"));
            var externalTask = TaskHelper.SafeExecuteAsync(apiBLL.GetExternalTestNoListAsync("DXCZLCW"));
            await Task.WhenAll(task, externalTask);
            var list = task.Result.Select(t => new TestItem
            {
                TestNo = t.TestNo,
                CoilNumber = string.IsNullOrEmpty(t.CoilNumber) ? t.OtherCoilNumber : t.CoilNumber
            }).ToList();
            // L32503140083-1T1P1C试片编号|SAMPLELOTNO(L32503140083)-试样号|SAMPLE_NO(1T1)试验项目代码|TEST_ITEM_CODE(P1)试验方向代码|TEST_DIRECT_CODE(C)
            var externalList = externalTask.Result.Select(t => new TestItem
            {
                TestNo = $"{t.SampleId}-1{t.SamplePosition}1SH",
                CoilNumber = t.MaterialId,
                IsExternal = true
            }).ToList();
            list.AddRange(externalList);
            return list;
        }

        public CalculateScale GetCurrentScale()
        {
            return connection.Table<CalculateScale>().OrderByDescending(x => x.Id).FirstOrDefault(x => x.Key.Equals("CircularArea"));
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
