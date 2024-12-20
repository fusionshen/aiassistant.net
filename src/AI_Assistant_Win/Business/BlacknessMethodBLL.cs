﻿using AI_Assistant_Win.Models;
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
        private readonly SQLiteConnection connection;

        public BlacknessMethodBLL()
        {
            connection = SQLiteHandler.Instance.GetSQLiteConnection();
        }

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
                throw new ArgumentNullException("blacknesss method item id is none!");
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
                    CreateTime = DateTime.Now
                };
                #endregion
                var ok = connection.Insert(blacknessMethodResult);
                if (ok == 0)
                {
                    throw new Exception("新增主体失败");
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
                ok = connection.InsertAll(itemList);
                if (ok == 0)
                {
                    throw new Exception("保存明细失败");
                }
                return blacknessMethodResult;
            }
            else // update
            {
                // find result
                var result = GetResultById(tempBlacknessResult.Id.ToString()) ?? throw new Exception("查找主体失败");
                var items = connection.Table<BlacknessMethodItem>().Where(t => t.ResultId == result.Id).ToList();
                // no batch in sqlitenet
                items.ForEach(t =>
                {
                    var ok = connection.Delete(t);
                    if (ok == 0)
                    {
                        throw new Exception("删除明细失败");
                    }
                });
                result.CoilNumber = tempBlacknessResult.CoilNumber;
                result.Size = tempBlacknessResult.Size;
                result.WorkGroup = tempBlacknessResult.WorkGroup;
                result.Analyst = tempBlacknessResult.Analyst;
                result.OriginImagePath = tempBlacknessResult.OriginImagePath;
                result.RenderImagePath = tempBlacknessResult.RenderImagePath;
                result.SurfaceOPLevel = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_OP)).Level;
                result.SurfaceOPWidth = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_OP)).Width;
                result.SurfaceOPScore = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_OP)).Score;
                result.SurfaceCELevel = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_CE)).Level;
                result.SurfaceCEWidth = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_CE)).Width;
                result.SurfaceCEScore = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_CE)).Score;
                result.SurfaceDRLevel = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_DR)).Level;
                result.SurfaceDRWidth = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_DR)).Width;
                result.SurfaceDRScore = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_DR)).Score;
                result.InsideOPLevel = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_OP)).Level;
                result.InsideOPWidth = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_OP)).Width;
                result.InsideOPScore = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_OP)).Score;
                result.InsideCELevel = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_CE)).Level;
                result.InsideCEWidth = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_CE)).Width;
                result.InsideCEScore = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_CE)).Score;
                result.InsideDRLevel = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_DR)).Level;
                result.InsideDRWidth = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_DR)).Width;
                result.InsideDRScore = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_DR)).Score;
                result.IsOK = tempBlacknessResult.Items.All(t => new List<string> { "3", "4", "5" }.Contains(t.Level));
                result.LastModifiedTime = DateTime.Now;
                var ok = connection.Update(result);
                if (ok == 0)
                {
                    throw new Exception("更新主体失败");
                }
                // 构造item
                var itemList = tempBlacknessResult.Items.Select(t => new BlacknessMethodItem
                {
                    ResultId = result.Id,
                    Location = t.Location,
                    Level = t.Level,
                    Score = t.Score,
                    Width = t.Width,
                    Prediction = JsonSerializer.Serialize(t.Prediction)
                });
                ok = connection.InsertAll(itemList);
                if (ok == 0)
                {
                    throw new Exception("保存明细失败");
                }
                return result;
            }
        }

        public List<int> LoadOriginalResultFromDB(BlacknessResult originalBlacknessResult, string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                originalBlacknessResult.Id = 0;
                originalBlacknessResult.CoilNumber = string.Empty;
                originalBlacknessResult.Size = string.Empty;
                originalBlacknessResult.OriginImagePath = string.Empty;
                originalBlacknessResult.RenderImagePath = string.Empty;
                originalBlacknessResult.WorkGroup = string.Empty;
                originalBlacknessResult.Analyst = string.Empty;
                originalBlacknessResult.Items = [];
                return [];
            }
            var allSorted = connection.Table<BlacknessMethodResult>().OrderBy(t => t.Id).ToList();
            // if t.Id.Equals(id), will find null, double funny!!!
            var body = allSorted.FirstOrDefault(t => t.Id.ToString().Equals(id)) ?? throw new Exception($"编号{id}没有主体数据，请联系管理员");
            var items = connection.Table<BlacknessMethodItem>()
                .Where(t => t.ResultId.Equals(id))
                .Select(t => new Blackness(t.Location, JsonSerializer.Deserialize<Prediction>(t.Prediction)))
                .OrderBy(t => t.Location)
                .ToList();
            if (items == null || items.Count == 0)
            {
                throw new Exception($"编号{id}没有明细数据，请联系管理员");
            }
            originalBlacknessResult.Id = body.Id;
            originalBlacknessResult.CoilNumber = body.CoilNumber;
            originalBlacknessResult.Size = body.Size;
            originalBlacknessResult.OriginImagePath = body.OriginImagePath;
            originalBlacknessResult.RenderImagePath = body.RenderImagePath;
            originalBlacknessResult.WorkGroup = body.WorkGroup;
            originalBlacknessResult.Analyst = body.Analyst;
            originalBlacknessResult.Items = items;
            return allSorted.Select(t => t.Id).ToList();
        }
    }
}
