using AI_Assistant_Win.Models;
using AI_Assistant_Win.Models.Enums;
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
    public class BlacknessUploadBLL()
    {
        private readonly string FILE_CATEGORY_NAME = "GA板V60黑度检测报告";

        private readonly ApiBLL apiBLL = ApiHandler.Instance.GetApiBLL();

        private readonly SQLiteConnection connection = SQLiteHandler.Instance.GetSQLiteConnection();

        public async Task Upload(Bitmap memoryImage, BlacknessMethodResult methodResult, BlacknessUploadResult lastUpload = null)
        {
            BlacknessResultResponse info = new();
            BlacknessUploadResult uploadResult = new();
            connection.BeginTransaction();
            try
            {
                uploadResult = await UploadPDF(memoryImage, methodResult, lastUpload);
                var ok = connection.Insert(uploadResult);
                if (ok == 0)
                {
                    throw new Exception(LocalizeHelper.ADD_SUBJECT_FAILED);
                }
                info = await UploadInfo(methodResult, uploadResult);
                methodResult.IsUploaded = true;
                methodResult.Uploader = uploadResult.Uploader;
                methodResult.UploadTime = uploadResult.UploadTime;
                ok = connection.Update(methodResult);
                if (ok == 0)
                {
                    throw new Exception(LocalizeHelper.UPDATE_SUBJECT_FAILED);
                }
                connection.Commit();
            }
            catch (Exception)
            {
                connection.Rollback();
                // should delete file at the server, but cant delete the latest version in the server, only delete FileManagerId.
                if (uploadResult.FileManagerId != 0)
                {
                    await apiBLL.DeleteUploadedFileAsync(uploadResult.FileManagerId);
                }
                // delete info
                if (!string.IsNullOrEmpty(info.Id))
                {
                    info.EntityState = EntityStateKind.Delete;
                    await apiBLL.UploadBlacknessResultAsync(info);
                }
                throw;
            }
        }

        private async Task<BlacknessResultResponse> UploadInfo(BlacknessMethodResult methodResult, BlacknessUploadResult uploadResult)
        {
            // create a model
            var model = new BlacknessResultResponse
            {
                CoilNumber = methodResult.CoilNumber,
                TestNo = methodResult.TestNo,
                Source = methodResult.IsExternal ? 1 : 2,
                OriginImagePath = methodResult.OriginImagePath,
                RenderImagePath = methodResult.RenderImagePath,
                Size = methodResult.Size,
                IsOK = methodResult.IsOK,
                SurfaceOPLevel = methodResult.SurfaceOPLevel,
                SurfaceOPWidth = methodResult.SurfaceOPWidth ?? 0f,
                SurfaceOPScore = methodResult.SurfaceOPScore ?? 0f,
                SurfaceCELevel = methodResult.SurfaceCELevel,
                SurfaceCEWidth = methodResult.SurfaceCEWidth ?? 0f,
                SurfaceCEScore = methodResult.SurfaceCEScore ?? 0f,
                SurfaceDRLevel = methodResult.SurfaceDRLevel,
                SurfaceDRWidth = methodResult.SurfaceDRWidth ?? 0f,
                SurfaceDRScore = methodResult.SurfaceDRScore ?? 0f,
                InsideOPLevel = methodResult.InsideOPLevel,
                InsideOPWidth = methodResult.InsideOPWidth ?? 0f,
                InsideOPScore = methodResult.InsideOPScore ?? 0f,
                InsideCELevel = methodResult.InsideCELevel,
                InsideCEWidth = methodResult.InsideCEWidth ?? 0f,
                InsideCEScore = methodResult.InsideCEScore ?? 0f,
                InsideDRLevel = methodResult.InsideDRLevel,
                InsideDRWidth = methodResult.InsideDRWidth ?? 0f,
                InsideDRScore = methodResult.InsideDRScore ?? 0f,
                IsUploaded = methodResult.IsUploaded,
                Uploader = methodResult.Uploader,
                UploadTime = methodResult.UploadTime,
                WorkGroup = methodResult.WorkGroup,
                Analyst = methodResult.Analyst,
                CreateTime = methodResult.CreateTime,
                LastReviser = methodResult.LastReviser,
                LastModifiedTime = methodResult.LastModifiedTime,
                ReportFileId = uploadResult.UploadFileId,
                Details = CreateDetails(methodResult.Id),
                EntityState = EntityStateKind.Insert,
            };
            // find it exits
            var finded = await apiBLL.FindBlacknessResultAsync(methodResult.TestNo, methodResult.CoilNumber);
            if (finded != null)
            {
                model.Id = finded.Id;
                model.EntityState = EntityStateKind.Update;
            }
            model.Id = await apiBLL.UploadBlacknessResultAsync(model);
            return model;
        }

        private List<BlacknessItemResponse> CreateDetails(int id)
        {
            var items = connection.Table<BlacknessMethodItem>()
                .Where(t => t.ResultId.Equals(id) && !string.IsNullOrEmpty(t.Level))
                .Select(t => new BlacknessItemResponse
                {
                    Location = t.Location,
                    Level = t.Level,
                    Score = t.Score ?? 0,
                    Width = t.Width ?? 0,
                    Nth = t.Nth ?? 1,
                    Prediction = t.Prediction
                })
                .OrderBy(t => t.Location)
                .ToList();
            return items;
        }

        private async Task<BlacknessUploadResult> UploadPDF(Bitmap memoryImage, BlacknessMethodResult methodResult, BlacknessUploadResult lastUpload)
        {
            var uploadResult = new BlacknessUploadResult
            {
                ResultId = methodResult.Id,
                TestNo = methodResult.TestNo,
                CoilNumber = methodResult.CoilNumber,
                Nth = methodResult.Nth,
                Uploader = $"{apiBLL.LoginUserInfo.Username}-{apiBLL.LoginUserInfo.Nickname}",
                LocalFilePath = SaveLocallyAndReturnPath(memoryImage, methodResult), // first save locally
                FileManagerId = lastUpload == null ? 0 : lastUpload.FileManagerId,
                FileName = $"{methodResult.TestNo}_{methodResult.CoilNumber}_{methodResult.Nth}.pdf",
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

        private string SaveLocallyAndReturnPath(Bitmap memoryImage, BlacknessMethodResult result)
        {
            string pdfDirectoryPath = $".\\Reports\\Blackness";
            Directory.CreateDirectory(pdfDirectoryPath);
            string fullPath = Path.Combine(pdfDirectoryPath, $"{result.TestNo}-{DateTime.Now:yyyyMMddHHmmssfff}.pdf");
            FileHelper.SaveImageAsPDF(memoryImage, fullPath);
            return fullPath;
        }

        public BlacknessUploadResult GetLastUploaded(BlacknessMethodResult target)
        {
            var item = connection.Table<BlacknessUploadResult>()
                .Where(t => target.Id.Equals(t.ResultId)).FirstOrDefault();
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
