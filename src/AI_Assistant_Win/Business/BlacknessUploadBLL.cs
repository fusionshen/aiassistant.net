using AI_Assistant_Win.Models;
using AI_Assistant_Win.Models.Response;
using AI_Assistant_Win.Utils;
using SQLite;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
            var uploadResult = new BlacknessUploadResult
            {
                ResultId = methodResult.Id,
                TestNo = methodResult.TestNo,
                CoilNumber = methodResult.CoilNumber,
                Uploader = $"{apiBLL.LoginUserInfo.Username}-{apiBLL.LoginUserInfo.Nickname}",
                LocalFilePath = SaveLocallyAndReturnPath(memoryImage, methodResult), // first save locally
                FileManagerId = lastUpload == null ? 0 : lastUpload.FileManagerId,
                FileName = methodResult.CoilNumber + ".pdf",
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
            uploadResult.UploadTime = uploadedFile.CreateTime;
            // add uploadResult & update methodResult
            connection.BeginTransaction();
            try
            {
                DoLocallyByTransaction(methodResult, uploadResult);
                connection.Commit();
            }
            catch (Exception)
            {
                connection.Rollback();
                // TODO: delete file at the server
                throw;
            }
        }

        private bool DoLocallyByTransaction(BlacknessMethodResult methodResult, BlacknessUploadResult uploadResult)
        {
            methodResult.IsUploaded = true;
            methodResult.Uploader = uploadResult.Uploader;
            methodResult.UploadTime = uploadResult.UploadTime;
            var ok = connection.Update(methodResult);
            if (ok == 0)
            {
                throw new Exception(LocalizeHelper.UPDATE_SUBJECT_FAILED);
            }
            ok = connection.Insert(uploadResult);
            if (ok == 0)
            {
                throw new Exception(LocalizeHelper.ADD_SUBJECT_FAILED);
            }
            return true;
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
            string fullPath = Path.Combine(pdfDirectoryPath, $"{result.CoilNumber}-{DateTime.Now:yyyyMMddHHmmssfff}.pdf");
            FileHelper.SaveImageAsPDF(memoryImage, fullPath);
            return fullPath;
        }

        public BlacknessUploadResult GetLastUploaded(BlacknessMethodResult target)
        {
            var item = connection.Table<BlacknessUploadResult>().Where(t => t.TestNo.Equals(target.TestNo))
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
