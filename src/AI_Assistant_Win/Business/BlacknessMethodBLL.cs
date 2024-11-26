using AI_Assistant_Win.Models;
using AI_Assistant_Win.Models.Enums;
using AI_Assistant_Win.Models.Middle;
using AI_Assistant_Win.Utils;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Yolov8.Net;

namespace AI_Assistant_Win.Business
{
    public class BlacknessMethodBLL
    {
        private List<Blackness> resultList;

        private readonly SQLiteConnection connection;

        public BlacknessMethodBLL()
        {
            connection = SQLiteHandler.Instance.GetSQLiteConnection();
        }

        public List<Blackness> GetResultList()
        {
            return resultList;
        }

        public List<Blackness> ParsePredictions(Prediction[] predictions)
        {
            var sorted = predictions.OrderByDescending(t => t.Rectangle.X).ToArray();
            // TODO: 现场根据X具体位置判断，因为可能出现未贴完6个部位或者未识别出六个部位的情况
            resultList =
                [
                    new(BlacknessLocationKind.SURFACE_OP, sorted[0]),
                    new(BlacknessLocationKind.SURFACE_CE, sorted[1]),
                    new(BlacknessLocationKind.SURFACE_DR, sorted[2]),
                    new(BlacknessLocationKind.INSIDE_OP, sorted[3]),
                    new(BlacknessLocationKind.INSIDE_CE, sorted[4]),
                    new(BlacknessLocationKind.INSIDE_DR, sorted[5])
                ];
            return resultList;
        }

        public int SaveResult(BlacknessMethodResult blacknessMethodResult)
        {
            // 构造主体
            blacknessMethodResult.CreateTime = DateTime.Now;
            blacknessMethodResult.SurfaceOPLevel = resultList.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_OP)).Level;
            blacknessMethodResult.SurfaceOPWidth = resultList.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_OP)).Width;
            blacknessMethodResult.SurfaceOPScore = resultList.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_OP)).Score;
            blacknessMethodResult.SurfaceCELevel = resultList.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_CE)).Level;
            blacknessMethodResult.SurfaceCEWidth = resultList.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_CE)).Width;
            blacknessMethodResult.SurfaceCEScore = resultList.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_CE)).Score;
            blacknessMethodResult.SurfaceDRLevel = resultList.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_DR)).Level;
            blacknessMethodResult.SurfaceDRWidth = resultList.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_DR)).Width;
            blacknessMethodResult.SurfaceDRScore = resultList.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_DR)).Score;
            blacknessMethodResult.InsideOPLevel = resultList.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_OP)).Level;
            blacknessMethodResult.InsideOPWidth = resultList.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_OP)).Width;
            blacknessMethodResult.InsideOPScore = resultList.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_OP)).Score;
            blacknessMethodResult.InsideCELevel = resultList.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_CE)).Level;
            blacknessMethodResult.InsideCEWidth = resultList.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_CE)).Width;
            blacknessMethodResult.InsideCEScore = resultList.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_CE)).Score;
            blacknessMethodResult.InsideDRLevel = resultList.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_DR)).Level;
            blacknessMethodResult.InsideDRWidth = resultList.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_DR)).Width;
            blacknessMethodResult.InsideDRScore = resultList.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_DR)).Score;
            blacknessMethodResult.IsOK = resultList.All(t => new List<string> { "3", "4", "5" }.Contains(t.Level));
            var ok = connection.Insert(blacknessMethodResult);
            if (ok == 0)
            {
                return ok;
            }
            // 构造item
            var itemList = resultList.Select(t => new BlacknessMethodItem
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
