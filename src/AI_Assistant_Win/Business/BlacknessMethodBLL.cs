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
            // if t.Id.ToString().Equals(id), will throw not function toString(), funny!
            var item = connection.Table<BlacknessMethodResult>().FirstOrDefault(t => t.Id.Equals(id));
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
                    ScaleId = tempBlacknessResult.Items.FirstOrDefault()?.CalculateScale.Id, //tempBlacknessResult.CalculateScale.Id,
                    CreateTime = DateTime.Now
                };
                #endregion
                var ok = connection.Insert(blacknessMethodResult);
                if (ok == 0)
                {
                    throw new Exception(LocalizeHelper.ADD_SUBJECT_FAILED);
                }
                // 构造item
                var itemList = tempBlacknessResult.Items.Select(t => new BlacknessMethodItem
                {
                    ResultId = blacknessMethodResult.Id,
                    Location = t.Location,
                    Level = t.Level,
                    Score = t.Score,
                    Width = t.CalculatedWidth,
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
                var items = connection.Table<BlacknessMethodItem>().Where(t => t.ResultId == result.Id).ToList();
                // no batch in sqlitenet
                items.ForEach(t =>
                {
                    var ok = connection.Delete(t);
                    if (ok == 0)
                    {
                        throw new Exception(LocalizeHelper.DELETE_DETAILS_FAILED);
                    }
                });
                result.OriginImagePath = tempBlacknessResult.OriginImagePath;
                result.RenderImagePath = tempBlacknessResult.RenderImagePath;
                result.WorkGroup = tempBlacknessResult.WorkGroup;
                result.TestNo = tempBlacknessResult.TestNo;
                result.CoilNumber = tempBlacknessResult.CoilNumber;
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
                // 构造item
                var itemList = tempBlacknessResult.Items.Select(t => new BlacknessMethodItem
                {
                    ResultId = result.Id,
                    Location = t.Location,
                    Level = t.Level,
                    Score = t.Score,
                    Width = t.CalculatedWidth,
                    Prediction = JsonConvert.SerializeObject(t.Prediction)
                });
                ok = connection.InsertAll(itemList);
                if (ok == 0)
                {
                    throw new Exception(LocalizeHelper.SAVE_DETAILS_FAILED);
                }
                return result;
            }
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
                originalBlacknessResult.Size = string.Empty;
                originalBlacknessResult.Analyst = $"{apiBLL.LoginUserInfo.Username}-{apiBLL.LoginUserInfo.Nickname}";
                originalBlacknessResult.Items = [];
                return [];
            }
            var allSorted = connection.Table<BlacknessMethodResult>().OrderBy(t => t.Id).ToList();
            // if t.Id.Equals(id), will find null, double funny!!!
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
            originalBlacknessResult.Size = body.Size;
            originalBlacknessResult.Analyst = body.Analyst;
            originalBlacknessResult.CalculateScale = scaleAtThatTime; // must before items
            originalBlacknessResult.Items = items;
            originalBlacknessResult.IsUploaded = body.IsUploaded; // promot before saving
            return allSorted.Select(t => t.Id).ToList();
        }

        public async Task<List<GetTestNoListResponse>> GetTestNoListAsync()
        {
            var list = await apiBLL.GetTestNoListAsync();
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
