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
using Yolov8.Net;

namespace AI_Assistant_Win.Business
{
    public class BlacknessMethodBLL
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
                    (t.IsExternal && text.Contains("委外")) ||
                    (!t.IsExternal && text.Contains("内部")) ||
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

        public BlacknessMethodResult GetResultById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new Exception(LocalizeHelper.ID_IS_EMPTY);
            }
            var item = connection.Table<BlacknessMethodResult>().ToList().FirstOrDefault(t => id.Equals(t.Id.ToString()));
            return item;
        }

        public int SaveResult(BlacknessResult tempBlacknessResult)
        {
            connection.BeginTransaction();
            try
            {
                var result = AddOrUpdateByTransaction(tempBlacknessResult);
                connection.Commit();
                return result.Id;
            }
            catch (Exception)
            {
                connection.Rollback();
                throw;
            }
        }

        private BlacknessMethodResult AddOrUpdateByTransaction(BlacknessResult tempBlacknessResult)
        {
            if (tempBlacknessResult.Id == 0)
            {
                #region 构造主体
                var blacknessMethodResult = new BlacknessMethodResult
                {
                    OriginImagePath = tempBlacknessResult.OriginImagePath,
                    RenderImagePath = tempBlacknessResult.RenderImagePath,
                    WorkGroup = tempBlacknessResult.WorkGroup,
                    TestNo = tempBlacknessResult.TestNo,
                    CoilNumber = tempBlacknessResult.CoilNumber,
                    Nth = tempBlacknessResult.Nth,
                    Size = tempBlacknessResult.Size,
                    Analyst = tempBlacknessResult.Analyst,
                    SurfaceOPLevel = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_OP)).Level,
                    SurfaceOPWidth = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_OP)).CalculatedWidth,
                    SurfaceOPScore = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_OP)).Score,
                    SurfaceCELevel = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_CE)).Level,
                    SurfaceCEWidth = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_CE)).CalculatedWidth,
                    SurfaceCEScore = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_CE)).Score,
                    SurfaceDRLevel = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_DR)).Level,
                    SurfaceDRWidth = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_DR)).CalculatedWidth,
                    SurfaceDRScore = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_DR)).Score,
                    InsideOPLevel = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_OP)).Level,
                    InsideOPWidth = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_OP)).CalculatedWidth,
                    InsideOPScore = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_OP)).Score,
                    InsideCELevel = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_CE)).Level,
                    InsideCEWidth = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_CE)).CalculatedWidth,
                    InsideCEScore = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_CE)).Score,
                    InsideDRLevel = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_DR)).Level,
                    InsideDRWidth = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_DR)).CalculatedWidth,
                    InsideDRScore = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_DR)).Score,
                    IsOK = tempBlacknessResult.Items.All(t => new List<string> { "3", "4", "5" }.Contains(t.Level)),
                    IsExternal = TaskHelper.SafeExecuteAsync(apiBLL.GetExternalTestNoListAsync("V60", tempBlacknessResult.TestNo.Split("-")[0])).Result.Count != 0,
                    ScaleId = tempBlacknessResult.Items.FirstOrDefault()?.CalculateScale.Id, //tempBlacknessResult.CalculateScale.Id,
                    CreateTime = DateTime.Now
                };
                #endregion
                var ok = connection.Insert(blacknessMethodResult);
                if (ok == 0)
                {
                    throw new Exception(LocalizeHelper.ADD_SUBJECT_FAILED);
                }
                var sameTestNoResults = connection.Table<BlacknessMethodResult>()
                    .Where(t => blacknessMethodResult.TestNo.Equals(t.TestNo) &&
                    blacknessMethodResult.CoilNumber.Equals(t.CoilNumber)).ToList();
                // all items
                var sameTestNoItems = connection.Table<BlacknessMethodItem>().ToList()
                    .Where(t => sameTestNoResults.Any(r => r.Id.Equals(t.ResultId))).ToList();
                // 构造item
                var itemList = tempBlacknessResult.Items.Select(t => new BlacknessMethodItem
                {
                    ResultId = blacknessMethodResult.Id,
                    Location = t.Location,
                    Level = t.Level,
                    Score = t.Score,
                    Width = t.CalculatedWidth,
                    Nth = CalculateLocationNth(sameTestNoItems, t.Location),
                    Prediction = JsonConvert.SerializeObject(t.Prediction)
                });
                ok = connection.InsertAll(itemList);
                if (ok == 0)
                {
                    throw new Exception(LocalizeHelper.SAVE_DETAILS_FAILED);
                }
                return blacknessMethodResult;
            }
            else // update
            {
                // find result
                var result = GetResultById(tempBlacknessResult.Id.ToString()) ?? throw new Exception(LocalizeHelper.FIND_SUBJECT_FAILED);
                result.OriginImagePath = tempBlacknessResult.OriginImagePath;
                result.RenderImagePath = tempBlacknessResult.RenderImagePath;
                result.WorkGroup = tempBlacknessResult.WorkGroup;
                result.TestNo = tempBlacknessResult.TestNo;
                result.CoilNumber = tempBlacknessResult.CoilNumber;
                result.Nth = tempBlacknessResult.Nth;
                result.Size = tempBlacknessResult.Size;
                result.Analyst = tempBlacknessResult.Analyst;
                result.SurfaceOPLevel = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_OP)).Level;
                result.SurfaceOPWidth = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_OP)).CalculatedWidth;
                result.SurfaceOPScore = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_OP)).Score;
                result.SurfaceCELevel = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_CE)).Level;
                result.SurfaceCEWidth = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_CE)).CalculatedWidth;
                result.SurfaceCEScore = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_CE)).Score;
                result.SurfaceDRLevel = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_DR)).Level;
                result.SurfaceDRWidth = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_DR)).CalculatedWidth;
                result.SurfaceDRScore = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_DR)).Score;
                result.InsideOPLevel = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_OP)).Level;
                result.InsideOPWidth = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_OP)).CalculatedWidth;
                result.InsideOPScore = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_OP)).Score;
                result.InsideCELevel = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_CE)).Level;
                result.InsideCEWidth = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_CE)).CalculatedWidth;
                result.InsideCEScore = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_CE)).Score;
                result.InsideDRLevel = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_DR)).Level;
                result.InsideDRWidth = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_DR)).CalculatedWidth;
                result.InsideDRScore = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_DR)).Score;
                result.IsOK = tempBlacknessResult.Items.All(t => new List<string> { "3", "4", "5" }.Contains(t.Level));
                result.ScaleId = tempBlacknessResult.Items.FirstOrDefault()?.CalculateScale.Id; //tempBlacknessResult.CalculateScale.Id;
                result.LastReviser = $"{apiBLL.LoginUserInfo.Username}-{apiBLL.LoginUserInfo.Nickname}";
                result.LastModifiedTime = DateTime.Now;
                var ok = connection.Update(result);
                if (ok == 0)
                {
                    throw new Exception(LocalizeHelper.UPDATE_SUBJECT_FAILED);
                }
                // all items
                var sameTestNoResults = connection.Table<BlacknessMethodResult>()
                    .Where(t => result.TestNo.Equals(t.TestNo) &&
                    result.CoilNumber.Equals(t.CoilNumber)).ToList();
                var sameTestNoItems = connection.Table<BlacknessMethodItem>().ToList()
                    .Where(t => sameTestNoResults.Any(r => r.Id.Equals(t.ResultId))).ToList();
                // 更新item
                var items = connection.Table<BlacknessMethodItem>().Where(t => t.ResultId == result.Id).ToList();
                var updateItems = items.Select(t =>
                {
                    // must exists
                    var target = tempBlacknessResult.Items.FirstOrDefault(r => t.Location.Equals(r.Location));
                    t.Level = target.Level;
                    t.Score = target.Score;
                    t.Width = target.CalculatedWidth;
                    t.Prediction = JsonConvert.SerializeObject(target.Prediction);
                    t.Nth = t.Nth != null ? t.Nth.Value : CalculateLocationNth(sameTestNoItems, t.Location);   // TODO:第一次没做后面做了，再修改第一次，实际是部位的第二次
                    return t;
                });
                ok = connection.UpdateAll(updateItems);
                if (ok == 0)
                {
                    throw new Exception(LocalizeHelper.SAVE_DETAILS_FAILED);
                }
                return result;
            }
        }

        private int? CalculateLocationNth(List<BlacknessMethodItem> sameTestNoItems, BlacknessLocationKind location)
        {
            var nth = sameTestNoItems.Where(t => location.Equals(t.Location) && !string.IsNullOrEmpty(t.Level)).Count();
            return nth + 1;
        }

        public int? GetNthOfMethod(string testNo, string coilNumber, int originResultId = 0)
        {
            if (string.IsNullOrEmpty(testNo) || string.IsNullOrEmpty(coilNumber))
            {
                throw new Exception(LocalizeHelper.INVALID_PARAMETER);
            }
            if (originResultId != 0)
            {
                // find result
                var result = GetResultById(originResultId.ToString()) ?? throw new Exception(LocalizeHelper.FIND_SUBJECT_FAILED);
                if (testNo.Equals(result.TestNo) && coilNumber.Equals(result.CoilNumber))
                {
                    return result.Nth;
                }
            }
            var item = connection.Table<BlacknessMethodResult>()
                .Where(t => testNo.Equals(t.TestNo) && coilNumber.Equals(t.CoilNumber))
                .OrderByDescending(t => t.Nth)
                .FirstOrDefault();
            return item == null ? 1 : item.Nth + 1;
        }

        public List<int> LoadOriginalResultFromDB(BlacknessResult originalBlacknessResult, string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                originalBlacknessResult.Id = 0;
                originalBlacknessResult.OriginImagePath = string.Empty;
                originalBlacknessResult.RenderImagePath = string.Empty;
                originalBlacknessResult.WorkGroup = string.Empty;
                originalBlacknessResult.TestNo = string.Empty;
                originalBlacknessResult.CoilNumber = string.Empty;
                originalBlacknessResult.Nth = null;
                originalBlacknessResult.Size = string.Empty;
                originalBlacknessResult.Analyst = $"{apiBLL.LoginUserInfo.Username}-{apiBLL.LoginUserInfo.Nickname}";
                originalBlacknessResult.Items = [];
                return [];
            }
            var allSorted = connection.Table<BlacknessMethodResult>().OrderBy(t => t.TestNo).OrderBy(t => t.Nth).ToList();
            var body = allSorted.FirstOrDefault(t => t.Id.ToString().Equals(id)) ?? throw new Exception($"{LocalizeHelper.CERTAIN_ID(id)}{LocalizeHelper.HAVE_NO_SUBJECT}，{LocalizeHelper.PLEASE_CONTACT_ADMIN}");
            var scaleAtThatTime = connection.Table<CalculateScale>().FirstOrDefault(x => x.Id.Equals(body.ScaleId));
            var items = connection.Table<BlacknessMethodItem>()
                .Where(t => t.ResultId.Equals(id))
                .Select(t => new Blackness(t.Location, JsonConvert.DeserializeObject<Prediction>(t.Prediction), scaleAtThatTime))
                .OrderBy(t => t.Location)
                .ToList();
            if (items == null || items.Count == 0)
            {
                throw new Exception($"{LocalizeHelper.CERTAIN_ID(id)}{LocalizeHelper.HAVE_NO_DETAILS}，{LocalizeHelper.PLEASE_CONTACT_ADMIN}");
            }
            originalBlacknessResult.Id = body.Id;
            originalBlacknessResult.OriginImagePath = body.OriginImagePath;
            originalBlacknessResult.RenderImagePath = body.RenderImagePath;
            originalBlacknessResult.WorkGroup = body.WorkGroup;
            originalBlacknessResult.TestNo = body.TestNo;
            originalBlacknessResult.CoilNumber = body.CoilNumber;
            originalBlacknessResult.Nth = body.Nth;
            originalBlacknessResult.Size = body.Size;
            originalBlacknessResult.Analyst = body.Analyst;
            originalBlacknessResult.CalculateScale = scaleAtThatTime; // must before items
            originalBlacknessResult.Items = items;
            originalBlacknessResult.IsUploaded = body.IsUploaded; // promot before saving
            return allSorted.Select(t => t.Id).ToList();
        }

        public async Task<List<TestItem>> GetTestNoListAsync()
        {
            var task = TaskHelper.SafeExecuteAsync(apiBLL.GetTestNoListAsync("G1SL"));
            var externalTask = TaskHelper.SafeExecuteAsync(apiBLL.GetExternalTestNoListAsync("V60"));
            await Task.WhenAll(task, externalTask);
            var list = task.Result.Select(t => new TestItem
            {
                TestNo = t.TestNo,
                CoilNumber = string.IsNullOrEmpty(t.CoilNumber) ? t.OtherCoilNumber : t.CoilNumber
            }).ToList();
            // L32503140083-1T1P1C试片编号|SAMPLELOTNO(L32503140083)-试样号|SAMPLE_NO(1T1)试验项目代码|TEST_ITEM_CODE(P1)试验方向代码|TEST_DIRECT_CODE(C)
            var externalList = externalTask.Result.Select(t => new TestItem
            {
                TestNo = $"{t.SampleId}-1{t.SamplePosition}1SL",
                CoilNumber = t.MaterialId,
                IsExternal = true
            }).ToList();
            list.AddRange(externalList);
            return list;
        }

        public CalculateScale GetCurrentScale()
        {
            return connection.Table<CalculateScale>().OrderByDescending(x => x.Id).FirstOrDefault(x => x.Key.Equals("Blackness"));
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
