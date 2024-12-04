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
        /// <summary>
        /// 检测时暂存的检测结果，用于界面上结果判定区数据展现和保存时逻辑判断
        /// </summary>
        private List<Blackness> tempMethodResultList;

        private readonly SQLiteConnection connection;

        public BlacknessMethodBLL()
        {
            connection = SQLiteHandler.Instance.GetSQLiteConnection();
        }

        public List<Blackness> GetTempMethodResultList()
        {
            return tempMethodResultList;
        }

        public List<Blackness> ParsePredictions(Prediction[] predictions)
        {
            var sorted = predictions.OrderByDescending(t => t.Rectangle.X).ToArray();
            // TODO: 现场根据X具体位置判断，因为可能出现未贴完6个部位或者未识别出六个部位的情况
            tempMethodResultList =
                [
                    new(BlacknessLocationKind.SURFACE_OP, sorted[0]),
                    new(BlacknessLocationKind.SURFACE_CE, sorted[1]),
                    new(BlacknessLocationKind.SURFACE_DR, sorted[2]),
                    new(BlacknessLocationKind.INSIDE_OP, sorted[3]),
                    new(BlacknessLocationKind.INSIDE_CE, sorted[4]),
                    new(BlacknessLocationKind.INSIDE_DR, sorted[5])
                ];
            return tempMethodResultList;
        }

        public int SaveResult(BlacknessMethodResult blacknessMethodResult)
        {
            // 构造主体
            blacknessMethodResult.CreateTime = DateTime.Now;
            blacknessMethodResult.SurfaceOPLevel = tempMethodResultList.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_OP)).Level;
            blacknessMethodResult.SurfaceOPWidth = tempMethodResultList.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_OP)).Width;
            blacknessMethodResult.SurfaceOPScore = tempMethodResultList.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_OP)).Score;
            blacknessMethodResult.SurfaceCELevel = tempMethodResultList.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_CE)).Level;
            blacknessMethodResult.SurfaceCEWidth = tempMethodResultList.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_CE)).Width;
            blacknessMethodResult.SurfaceCEScore = tempMethodResultList.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_CE)).Score;
            blacknessMethodResult.SurfaceDRLevel = tempMethodResultList.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_DR)).Level;
            blacknessMethodResult.SurfaceDRWidth = tempMethodResultList.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_DR)).Width;
            blacknessMethodResult.SurfaceDRScore = tempMethodResultList.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_DR)).Score;
            blacknessMethodResult.InsideOPLevel = tempMethodResultList.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_OP)).Level;
            blacknessMethodResult.InsideOPWidth = tempMethodResultList.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_OP)).Width;
            blacknessMethodResult.InsideOPScore = tempMethodResultList.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_OP)).Score;
            blacknessMethodResult.InsideCELevel = tempMethodResultList.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_CE)).Level;
            blacknessMethodResult.InsideCEWidth = tempMethodResultList.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_CE)).Width;
            blacknessMethodResult.InsideCEScore = tempMethodResultList.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_CE)).Score;
            blacknessMethodResult.InsideDRLevel = tempMethodResultList.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_DR)).Level;
            blacknessMethodResult.InsideDRWidth = tempMethodResultList.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_DR)).Width;
            blacknessMethodResult.InsideDRScore = tempMethodResultList.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_DR)).Score;
            blacknessMethodResult.IsOK = tempMethodResultList.All(t => new List<string> { "3", "4", "5" }.Contains(t.Level));
            var ok = connection.Insert(blacknessMethodResult);
            if (ok == 0)
            {
                return ok;
            }
            // 构造item
            var itemList = tempMethodResultList.Select(t => new BlacknessMethodItem
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
    }
}
