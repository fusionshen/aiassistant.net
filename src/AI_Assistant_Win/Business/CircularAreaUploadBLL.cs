﻿using AI_Assistant_Win.Models;
using AI_Assistant_Win.Models.Enums;
using AI_Assistant_Win.Models.Middle;
using AI_Assistant_Win.Models.Response;
using AI_Assistant_Win.Utils;
using SQLite;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AI_Assistant_Win.Business
{
    public class CircularAreaUploadBLL()
    {
        private readonly string FILE_CATEGORY_NAME = "镀锌板圆片面积检测报告";

        private readonly ApiBLL apiBLL = ApiHandler.Instance.GetApiBLL();

        private readonly SQLiteConnection connection = SQLiteHandler.Instance.GetSQLiteConnection();

        public async Task Upload(Bitmap memoryImage, CircularAreaSummaryHistory history, CircularAreaUploadResult lastUpload = null)
        {
            var uploadResult = await UploadPDF(memoryImage, history, lastUpload);
            // call bussiness
            BlacknessResultResponse info;
            try
            {
                info = await UploadInfo(history, uploadResult);
            }
            catch (Exception)
            {
                // cant delete the latest version in the server
                throw;
            }
            // add uploadResult & update methodResult
            connection.BeginTransaction();
            try
            {
                DoLocallyByTransaction(history, uploadResult);
                connection.Commit();
            }
            catch (Exception)
            {
                connection.Rollback();
                // should delete file at the server, but cant delete the latest version in the server
                // delete info
                await WithdrawInfo(info);
                throw;
            }
        }

        private async Task WithdrawInfo(BlacknessResultResponse info)
        {
            if (!string.IsNullOrEmpty(info.Id))
            {
                info.EntityState = EntityStateKind.Delete;
                await apiBLL.UploadBlacknessResultAsync(info);
            }
        }

        private async Task<BlacknessResultResponse> UploadInfo(CircularAreaSummaryHistory methodResult, CircularAreaUploadResult uploadResult)
        {
            // create a model
            //var model = new BlacknessResultResponse
            //{
            //    CoilNumber = methodResult.Summary.CoilNumber,
            //    TestNo = methodResult.Summary.TestNo,
            //    OriginImagePath = methodResult.OriginImagePath,
            //    RenderImagePath = methodResult.RenderImagePath,
            //    Size = methodResult.Size,
            //    IsOK = methodResult.IsOK,
            //    SurfaceOPLevel = methodResult.SurfaceOPLevel,
            //    SurfaceOPWidth = methodResult.SurfaceOPWidth,
            //    SurfaceOPScore = methodResult.SurfaceOPScore,
            //    SurfaceCELevel = methodResult.SurfaceCELevel,
            //    SurfaceCEWidth = methodResult.SurfaceCEWidth,
            //    SurfaceCEScore = methodResult.SurfaceCEScore,
            //    SurfaceDRLevel = methodResult.SurfaceDRLevel,
            //    SurfaceDRWidth = methodResult.SurfaceDRWidth,
            //    SurfaceDRScore = methodResult.SurfaceDRScore,
            //    InsideOPLevel = methodResult.InsideOPLevel,
            //    InsideOPWidth = methodResult.InsideOPWidth,
            //    InsideOPScore = methodResult.InsideOPScore,
            //    InsideCELevel = methodResult.InsideCELevel,
            //    InsideCEWidth = methodResult.InsideCEWidth,
            //    InsideCEScore = methodResult.InsideCEScore,
            //    InsideDRLevel = methodResult.InsideDRLevel,
            //    InsideDRWidth = methodResult.InsideDRWidth,
            //    InsideDRScore = methodResult.InsideDRScore,
            //    IsUploaded = methodResult.IsUploaded,
            //    Uploader = methodResult.Uploader,
            //    UploadTime = methodResult.UploadTime,
            //    WorkGroup = methodResult.WorkGroup,
            //    Analyst = methodResult.Analyst,
            //    CreateTime = methodResult.CreateTime,
            //    LastReviser = methodResult.LastReviser,
            //    LastModifiedTime = methodResult.LastModifiedTime,
            //    ReportFileId = uploadResult.UploadFileId,
            //    Details = CreateDetails(methodResult.Id),
            //    EntityState = EntityStateKind.Insert,
            //};
            //// find it exits
            //var finded = await apiBLL.FindBlacknessResultAsync(methodResult.TestNo, methodResult.CoilNumber);
            //if (finded != null)
            //{
            //    model.Id = finded.Id;
            //    model.EntityState = EntityStateKind.Update;
            //}
            //model.Id = await apiBLL.UploadBlacknessResultAsync(model);
            //return model;
            return await Task.FromResult(new BlacknessResultResponse());
        }

        private List<BlacknessItemResponse> CreateDetails(int id)
        {
            var items = connection.Table<BlacknessMethodItem>()
                .Where(t => t.ResultId.Equals(id))
                .Select(t => new BlacknessItemResponse
                {
                    Location = t.Location,
                    Level = t.Level,
                    Score = t.Score,
                    Width = t.Width,
                    Prediction = t.Prediction
                })
                .OrderBy(t => t.Location)
                .ToList();
            return items;
        }

        private async Task<CircularAreaUploadResult> UploadPDF(Bitmap memoryImage, CircularAreaSummaryHistory history, CircularAreaUploadResult lastUpload)
        {
            var uploadResult = new CircularAreaUploadResult
            {
                SummaryId = history.Summary.Id,
                TestNo = history.Summary.TestNo,
                CoilNumber = history.Summary.CoilNumber,
                Uploader = $"{apiBLL.LoginUserInfo.Username}-{apiBLL.LoginUserInfo.Nickname}",
                LocalFilePath = SaveLocallyAndReturnPath(memoryImage, history), // first save locally
                FileManagerId = lastUpload == null ? 0 : lastUpload.FileManagerId,
                FileName = history.Summary.TestNo + ".pdf",
                FileCategory = FILE_CATEGORY_NAME,
                FileCategoryId = await GetFileCategoryId(),
                FileVersion = $"{DateTime.Now:yyyyMMddHHmmss}",
                UploadFileId = lastUpload?.UploadFileId,
            };
            // upload pdf file
            uploadResult.FileManagerId = await apiBLL.UploadFileAsync(uploadResult);
            // get file details
            var uploadedFile = await apiBLL.GetFileByIdAsync(uploadResult.FileManagerId);
            uploadResult.FileManagerId = uploadedFile.FileManagerId;
            uploadResult.FileName = uploadedFile.FileName;
            uploadResult.FileCategoryId = uploadedFile.FileCategoryId;
            uploadResult.FileCategory = uploadedFile.FileCategory;
            uploadResult.FileVersionId = uploadedFile.FileVersionId;
            uploadResult.FileVersion = uploadedFile.FileVersion;
            uploadResult.UploadFileId = uploadedFile.UploadFileId;
            uploadResult.UploadTime = DateTime.Now;
            return uploadResult;
        }

        private void DoLocallyByTransaction(CircularAreaSummaryHistory methodResult, CircularAreaUploadResult uploadResult)
        {
            methodResult.Summary.IsUploaded = true;
            methodResult.Summary.Uploader = uploadResult.Uploader;
            methodResult.Summary.UploadTime = uploadResult.UploadTime;
            var ok = connection.Update(methodResult.Summary);
            if (ok == 0)
            {
                throw new Exception(LocalizeHelper.UPDATE_SUBJECT_FAILED);
            }
            ok = connection.Insert(uploadResult);
            if (ok == 0)
            {
                throw new Exception(LocalizeHelper.ADD_SUBJECT_FAILED);
            }
        }

        private async Task<int> GetFileCategoryId()
        {
            // find system directionary
            var tree = await apiBLL.GetFileCategoryTreeAsync();
            // find target
            var category = FindFileCategoryByName(tree, FILE_CATEGORY_NAME);
            if (category == null)
            {
                // create category
                return await apiBLL.CreateCategoryAsync(FILE_CATEGORY_NAME);
            }
            return category.Id;
        }

        private string SaveLocallyAndReturnPath(Bitmap memoryImage, CircularAreaSummaryHistory result)
        {
            string pdfDirectoryPath = $".\\Reports\\CircularArea";
            Directory.CreateDirectory(pdfDirectoryPath);
            string fullPath = Path.Combine(pdfDirectoryPath, $"{result.Summary.TestNo}-{DateTime.Now:yyyyMMddHHmmssfff}.pdf");
            FileHelper.SaveImageAsPDF(memoryImage, fullPath);
            return fullPath;
        }

        public CircularAreaUploadResult GetLastUploaded(CircularAreaSummaryHistory target)
        {
            var item = connection.Table<CircularAreaUploadResult>()
                .Where(t => t.TestNo.Equals(target.Summary.TestNo) && t.CoilNumber.Equals(target.Summary.CoilNumber))
                .OrderByDescending(t => t.FileVersion).FirstOrDefault();
            return item;
        }

        private GetFileCategoryListResponse FindFileCategoryByName(List<GetFileCategoryListResponse> tree, string target)
        {
            foreach (var item in tree)
            {
                if (target.Equals(item.Value.CategoryName))
                {
                    return item;
                }
                if (item.Children != null)
                {
                    var found = FindFileCategoryByName(item.Children, target);
                    if (found != null)
                    {
                        return found;
                    }
                }
            }
            return null;
        }
    }
}
