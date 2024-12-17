using AI_Assistant_Win.Models;
using AI_Assistant_Win.Models.Enums;
using AI_Assistant_Win.Models.Middle;
using AI_Assistant_Win.Utils;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace AI_Assistant_Win.Business
{
    public class BlacknessMethodBLL
    {
        private readonly SQLiteConnection connection;

        public BlacknessMethodBLL()
        {
            connection = SQLiteHandler.Instance.GetSQLiteConnection();
        }

        public List<BlacknessMethodResult> GetResultListFromDB()
        {
            var query = connection.Table<BlacknessMethodResult>().ToList();
            return query;
        }

        public BlacknessMethodResult GetResultById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("blacknesss method item id is none!");
            }
            // if t.Id.ToString()Equals(id), will throw not function toString(), funny!
            var item = connection.Table<BlacknessMethodResult>().FirstOrDefault(t => t.Id.Equals(id));
            return item;
        }

        internal int SaveResult(BlacknessResult tempBlacknessResult)
        {
            // 构造主体
            var blacknessMethodResult = new BlacknessMethodResult
            {
                Id = tempBlacknessResult.Id,
                CoilNumber = tempBlacknessResult.CoilNumber,
                Size = tempBlacknessResult.Size,
                WorkGroup = tempBlacknessResult.WorkGroup,
                Analyst = tempBlacknessResult.Analyst,
                OriginImagePath = tempBlacknessResult.OriginImagePath,
                RenderImagePath = tempBlacknessResult.RenderImagePath,
                SurfaceOPLevel = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_OP)).Level,
                SurfaceOPWidth = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_OP)).Width,
                SurfaceOPScore = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_OP)).Score,
                SurfaceCELevel = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_CE)).Level,
                SurfaceCEWidth = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_CE)).Width,
                SurfaceCEScore = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_CE)).Score,
                SurfaceDRLevel = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_DR)).Level,
                SurfaceDRWidth = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_DR)).Width,
                SurfaceDRScore = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_DR)).Score,
                InsideOPLevel = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_OP)).Level,
                InsideOPWidth = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_OP)).Width,
                InsideOPScore = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_OP)).Score,
                InsideCELevel = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_CE)).Level,
                InsideCEWidth = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_CE)).Width,
                InsideCEScore = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_CE)).Score,
                InsideDRLevel = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_DR)).Level,
                InsideDRWidth = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_DR)).Width,
                InsideDRScore = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_DR)).Score,
                IsOK = tempBlacknessResult.Items.All(t => new List<string> { "3", "4", "5" }.Contains(t.Level)),
            };
            if (blacknessMethodResult.Id == 0)
            {
                blacknessMethodResult.CreateTime = DateTime.Now;
                var ok = connection.Insert(blacknessMethodResult);
                if (ok == 0)
                {
                    return ok;
                }
                // 构造item
                var itemList = tempBlacknessResult.Items.Select(t => new BlacknessMethodItem
                {
                    ResultId = blacknessMethodResult.Id,
                    Location = t.Location,
                    Level = t.Level,
                    Score = t.Score,
                    Width = t.Width,
                    Prediction = JsonSerializer.Serialize(t.Prediction)
                });
                // TODO：transaction
                ok = connection.InsertAll(itemList);
                if (ok == 0)
                {
                    return ok;
                }
                return blacknessMethodResult.Id;
            }
            else // update
            {
                var items = connection.Table<BlacknessMethodItem>().Where(t => t.ResultId == blacknessMethodResult.Id).ToList();
                // no batch in sqlitenet
                items.ForEach(t => {
                    var ok = connection.Delete(t);
                    if (ok  == 0)
                    {
                        throw new Exception("保存失败");
                    }
                });
              
                blacknessMethodResult.LastModifiedTime = DateTime.Now;
                var ok = connection.Update(blacknessMethodResult);
                if (ok == 0)
                {
                    return ok;
                }
                // 构造item
                var itemList = tempBlacknessResult.Items.Select(t => new BlacknessMethodItem
                {
                    ResultId = blacknessMethodResult.Id,
                    Location = t.Location,
                    Level = t.Level,
                    Score = t.Score,
                    Width = t.Width,
                    Prediction = JsonSerializer.Serialize(t.Prediction)
                });
                // TODO：transaction
                ok = connection.InsertAll(itemList);
                if (ok == 0)
                {
                    return ok;
                }
                return blacknessMethodResult.Id;
            }
        }
    }
}
