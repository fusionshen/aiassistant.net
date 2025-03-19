using AI_Assistant_Win.Models;
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
            CircularAreaResultResponse info = new();
            CircularAreaUploadResult uploadResult = new();
            connection.BeginTransaction();
            // call bussiness
            try
            {
                uploadResult = await UploadPDF(memoryImage, history, lastUpload);
                var ok = connection.Insert(uploadResult);
                if (ok == 0)
                {
                    throw new Exception(LocalizeHelper.ADD_SUBJECT_FAILED);
                }
                info = await UploadInfo(history, uploadResult);
                history.Summary.IsUploaded = true;
                history.Summary.Uploader = uploadResult.Uploader;
                history.Summary.UploadTime = uploadResult.UploadTime;
                ok = connection.Update(history.Summary);
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
                    await apiBLL.UploadCircularAreaResultAsync(info);
                }
                throw;
            }
        }

        private async Task<CircularAreaResultResponse> UploadInfo(CircularAreaSummaryHistory history, CircularAreaUploadResult uploadResult)
        {
            // create a model
            var model = new CircularAreaResultResponse
            {
                TestNo = history.Summary.IsExternal ? history.Summary.TestNo.Split("-")[0] : history.Summary.TestNo,
                CoilNumber = history.Summary.CoilNumber,
                Source = !history.Summary.IsExternal ? 1 : 2,
                Items = CreateDetails(history.MethodList),
                Creator = history.Summary.Creator,
                CreateTime = history.Summary.CreateTime,
                LastReviser = history.Summary.LastReviser,
                LastModifiedTime = history.Summary.LastModifiedTime,
                EntityState = EntityStateKind.Insert,
            };
            // find it exits
            var finded = await apiBLL.FindCircularAreaResultAsync(history.Summary.TestNo, history.Summary.CoilNumber);
            if (finded != null)
            {
                model.Id = finded.Id;
                model.EntityState = EntityStateKind.Update;
            }
            model.Id = await apiBLL.UploadCircularAreaResultAsync(model);
            return model;
        }

        private List<CircularAreaItemResponse> CreateDetails(List<CircularAreaMethodResult> methodList)
        {
            var items = methodList.Select(t => new CircularAreaItemResponse
            {
                WorkGroup = t.WorkGroup,
                ScaleId = t.ScaleId.Value,
                Position = t.Position,
                Nth = t.Nth ?? 1,
                Pixels = t.Pixels,
                Confidence = t.Confidence,
                Area = t.Area,
                Diameter = t.Diameter,
                Prediction = t.Prediction,
                Analyst = t.Analyst,
                CreateTime = t.CreateTime,
                LastReviser = t.LastReviser ?? t.Analyst,
                LastModifiedTime = t.LastModifiedTime ?? t.CreateTime,
                EntityState = EntityStateKind.Insert,
            })
                .OrderBy(t => t.Position)
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
                Nth = history.Summary.Nth,
                Uploader = $"{apiBLL.LoginUserInfo.Username}-{apiBLL.LoginUserInfo.Nickname}",
                LocalFilePath = SaveLocallyAndReturnPath(memoryImage, history), // first save locally
                FileManagerId = lastUpload == null ? 0 : lastUpload.FileManagerId,
                FileName = $"{history.Summary.TestNo}_{history.Summary.CoilNumber}_{history.Summary.Nth}.pdf",
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
                .Where(t => target.Summary.Id.Equals(t.SummaryId))
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
