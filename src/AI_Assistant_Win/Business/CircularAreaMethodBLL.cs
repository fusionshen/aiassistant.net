using AI_Assistant_Win.Models;
using AI_Assistant_Win.Models.Enums;
using AI_Assistant_Win.Models.Middle;
using AI_Assistant_Win.Models.Response;
using AI_Assistant_Win.Utils;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoloDotNet.Models;

namespace AI_Assistant_Win.Business
{
    public class CircularAreaMethodBLL
    {
        private readonly SQLiteConnection connection = SQLiteHandler.Instance.GetSQLiteConnection();

        private readonly ApiBLL apiBLL = ApiHandler.Instance.GetApiBLL();

        public List<BlacknessMethodResult> GetResultListByConditions(DateTime? startDate, DateTime? endDate, string text)
        {
            var all = connection.Table<BlacknessMethodResult>().ToList();
            var filtered = all.Where(t => Filter(t, startDate, endDate, text)).ToList();
            var sorted = filtered.OrderByDescending(t => t.CreateTime).ToList();
            return sorted;
        }

        private bool Filter(BlacknessMethodResult t, DateTime? startDate, DateTime? endDate, string text)
        {
            var result = true;
            if (startDate != null)
            {
                result = result && t.CreateTime != null && t.CreateTime >= startDate;
            }
            if (endDate != null)
            {
                result = result && t.CreateTime != null && t.CreateTime <= endDate;
            }
            if (!string.IsNullOrEmpty(text))
            {
                result = result && (t.Id.ToString().Equals(text) ||
                    (!string.IsNullOrEmpty(t.TestNo) && t.TestNo.Contains(text)) ||
                    (!string.IsNullOrEmpty(t.CoilNumber) && t.CoilNumber.Contains(text)) ||
                    (!string.IsNullOrEmpty(t.Size) && t.Size.Contains(text)) ||
                    (t.IsOK && text.Contains("OK", StringComparison.CurrentCultureIgnoreCase)) ||
                    (!t.IsOK && text.Contains("NG", StringComparison.CurrentCultureIgnoreCase)) ||
                    (t.IsUploaded && text.Equals("已上传")) ||
                    (!t.IsUploaded && text.Equals("未上传")) ||
                    (!string.IsNullOrEmpty(t.Uploader) && t.Uploader.Contains(text)) ||
                    (!string.IsNullOrEmpty(t.WorkGroup) && t.WorkGroup.Contains(text)) ||
                    (!string.IsNullOrEmpty(t.Analyst) && t.Analyst.Contains(text)) ||
                    (!string.IsNullOrEmpty(t.LastReviser) && t.LastReviser.Contains(text))
                    );
            }
            return result;
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
            var target = GetResultExitsInDB(tempCircularAreaResult);
            if (target == null)
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
                target.Diameter = tempCircularAreaResult.Item.Diameter;
                target.Prediction = JsonConvert.SerializeObject(tempCircularAreaResult.Item.Prediction, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                target.WorkGroup = tempCircularAreaResult.WorkGroup;
                target.TestNo = tempCircularAreaResult.TestNo;
                target.CoilNumber = tempCircularAreaResult.CoilNumber;
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

        private void AddOrUpdateSummary(CircularAreaMethodResult circularAreaMethodResult)
        {
            // 查询summary是否存在
            var summary = connection.Table<CircularAreaMethodSummary>().FirstOrDefault(t => circularAreaMethodResult.TestNo.Equals(t.TestNo) && circularAreaMethodResult.CoilNumber.Equals(t.CoilNumber));
            if (summary == null)
            {
                // 构造summary
                summary = new CircularAreaMethodSummary
                {
                    TestNo = circularAreaMethodResult.TestNo,
                    CoilNumber = circularAreaMethodResult.CoilNumber,
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
                var ok = connection.Update(summary);
                if (ok == 0)
                {
                    throw new Exception(LocalizeHelper.UPDATE_SUMMARY_FAILED);
                }
            }
        }

        public List<int> LoadOriginalResultFromDB(CircularAreaResult originalCircularAreaResult, string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                originalCircularAreaResult.Id = 0;
                originalCircularAreaResult.TestNo = string.Empty;
                originalCircularAreaResult.CoilNumber = string.Empty;
                originalCircularAreaResult.Position = string.Empty;
                originalCircularAreaResult.OriginImagePath = string.Empty;
                originalCircularAreaResult.RenderImagePath = string.Empty;
                originalCircularAreaResult.WorkGroup = string.Empty;
                originalCircularAreaResult.Analyst = $"{apiBLL.LoginUserInfo.Username}-{apiBLL.LoginUserInfo.Nickname}";
                originalCircularAreaResult.Item = null;
                return [];
            }
            // sorted by testNo or coilNumber，then sorted by position
            var allSorted = connection.Table<CircularAreaMethodResult>().OrderBy(t => t.CreateTime).ThenBy(t => t.TestNo).ThenBy(t => t.Position).ToList();
            var body = allSorted.FirstOrDefault(t => t.Id.ToString().Equals(id)) ?? throw new Exception($"{LocalizeHelper.CERTAIN_ID(id)}{LocalizeHelper.HAVE_NO_SUBJECT}，{LocalizeHelper.PLEASE_CONTACT_ADMIN}");
            var scaleAtThatTime = connection.Table<CalculateScale>().FirstOrDefault(x => x.Id.Equals(body.ScaleId));
            originalCircularAreaResult.Id = body.Id;
            originalCircularAreaResult.TestNo = body.TestNo;
            originalCircularAreaResult.Position = PositionList.FirstOrDefault(t => body.Position.Equals(t.Key)).Value;
            originalCircularAreaResult.CoilNumber = body.CoilNumber;
            originalCircularAreaResult.OriginImagePath = body.OriginImagePath;
            originalCircularAreaResult.RenderImagePath = body.RenderImagePath;
            originalCircularAreaResult.WorkGroup = body.WorkGroup;
            originalCircularAreaResult.Analyst = body.Analyst;
            originalCircularAreaResult.CalculateScale = scaleAtThatTime; // must before items
            originalCircularAreaResult.Item = new CircularArea(JsonConvert.DeserializeObject<Segmentation>(body.Prediction), scaleAtThatTime);
            originalCircularAreaResult.IsUploaded = GetSummaryExitsInDB(body).IsUploaded; // promot before saving
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

        public async Task<List<GetTestNoListResponse>> GetTestNoListAsync()
        {
            var list = await apiBLL.GetTestNoListAsync();
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
