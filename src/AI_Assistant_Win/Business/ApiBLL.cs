using AI_Assistant_Win.Models;
using AI_Assistant_Win.Models.Request;
using AI_Assistant_Win.Models.Response;
using AI_Assistant_Win.Utils;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AI_Assistant_Win.Business
{
    public class ApiBLL : INotifyPropertyChanged
    {
        private LoginToken loginToken;
        public LoginToken LoginToken
        {
            get { return loginToken; }
            set
            {
                loginToken = value;
                if (value == null)
                {
                    // TODO: when token has expired, relogin
                    OnPropertyChanged(nameof(LoginToken));
                }
            }

        }

        private GetUserInfoResponse loginUserInfo;

        public GetUserInfoResponse LoginUserInfo
        {
            get { return loginUserInfo; }
            set
            {
                if (loginUserInfo != value)
                {
                    loginUserInfo = value;
                    OnPropertyChanged(nameof(LoginUserInfo));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly HttpClient httpClient = Utils.HttpClientHandler.Instance.GetHttpClient();

        private readonly SQLiteConnection connection = SQLiteHandler.Instance.GetSQLiteConnection();

        public async Task<bool> LoginAsync(string username, string password)
        {
            var loginUrl = connection.Table<SystemConfig>().LastOrDefault(t => t.Key.Equals("LoginURL"))?.Value;

            if (string.IsNullOrEmpty(loginUrl))
            {
                throw new Exception("登录接口未指定，请联系管理员");
            }

            var json = JsonConvert.SerializeObject(new LoginRequest { Username = $"{username}@lims", Password = password });

            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(loginUrl, data);

            var jsonStr = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ResponseBody<LoginResponse>>(jsonStr) ?? throw new Exception("登录接口解析有误，请联系管理员");

            if (result.Status != 200)
            {
                throw new Exception(result.Message);
            }
            if (result.Data == null)
            {
                throw new Exception("登录接口返回有误，请联系管理员");
            }
            if (result.Data.Token == null || result.Data.Result != 1)
            {
                throw new Exception(result.Data.ResultText);
            }
            LoginToken = result.Data.Token;
            return true;
        }

        public async Task<bool> GetUserInfoAsync()
        {
            var getUserInfoUrl = connection.Table<SystemConfig>().LastOrDefault(t => t.Key.Equals("GetUserInfoUrl"))?.Value;

            if (string.IsNullOrEmpty(getUserInfoUrl))
            {
                throw new Exception("用户信息接口未指定，请联系管理员");
            }

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginToken.AccessToken);

            var jsonStr = await httpClient.GetStringAsync(getUserInfoUrl);

            var result = JsonConvert.DeserializeObject<ResponseBody<GetUserInfoResponse>>(jsonStr) ?? throw new Exception("用户信息接口解析有误，请联系管理员");

            if (result.Status != 200)
            {
                throw new Exception(result.Message);
            }
            if (result.Data == null)
            {
                throw new Exception("用户信息接口返回有误，请联系管理员");
            }
            LoginUserInfo = result.Data;
            return true;
        }

        public async Task<Image> GetUserAvatarAsync()
        {
            var fileRootDictionary = connection.Table<SystemConfig>().LastOrDefault(t => t.Key.Equals("FileRootDictionary"))?.Value;

            if (string.IsNullOrEmpty(fileRootDictionary))
            {
                throw new Exception("平台文件根目录未指定，请联系管理员");
            }
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginToken.AccessToken);
            var response = await httpClient.GetAsync($"{fileRootDictionary}{loginUserInfo.Avatar}", HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode(); // 抛出异常如果状态码表示错误
            using var responseStream = await response.Content.ReadAsStreamAsync();
            using var ms = new MemoryStream();
            await responseStream.CopyToAsync(ms);
            ms.Seek(0, SeekOrigin.Begin); // 重置流的位置到开始
            Image image = Image.FromStream(ms);
            return image;
        }

        public void Logout()
        {
            LoginUserInfo = null;
        }

        public async Task<List<GetTestNoListResponse>> GetTestNoListAsync()
        {
            var getTestNoListUrl = connection.Table<SystemConfig>().LastOrDefault(t => t.Key.Equals("GetTestNoListUrl"))?.Value;

            if (string.IsNullOrEmpty(getTestNoListUrl))
            {
                throw new Exception("试样编号接口未指定，请联系管理员");
            }

            getTestNoListUrl = $"{getTestNoListUrl}?{HttpHelper.ToQueryString(new GetTestNoListRequest { })}";
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginToken.AccessToken);
            var jsonStr = await httpClient.GetStringAsync(getTestNoListUrl);

            var result = JsonConvert.DeserializeObject<ResponseBody<PagableBody<List<List<GetTestNoListResponse>>>>>(jsonStr) ?? throw new Exception("试样编号接口解析有误，请联系管理员");

            if (result.Status != 200)
            {
                throw new Exception(result.Message);
            }
            if (result.Data == null || result.Data.Data == null)
            {
                throw new Exception("试样编号接口返回有误，请联系管理员");
            }
            return result.Data.Data.FirstOrDefault();
        }

        public async Task<List<GetExternalTestNoListResponse>> GetExternalTestNoListAsync(string itemId = null, string sampleId = null)
        {
            var getExternalTestNoListUrl = connection.Table<SystemConfig>().LastOrDefault(t => t.Key.Equals("GetExternalTestNoListUrl"))?.Value;

            if (string.IsNullOrEmpty(getExternalTestNoListUrl))
            {
                throw new Exception("委外试样编号接口未指定，请联系管理员");
            }

            getExternalTestNoListUrl = $"{getExternalTestNoListUrl}?{HttpHelper.ToQueryString(new GetExternalTestNoListRequest { ItemId = itemId, SampleId = sampleId })}";
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginToken.AccessToken);
            var jsonStr = await httpClient.GetStringAsync(getExternalTestNoListUrl);
            var result = JsonConvert.DeserializeObject<ResponseBody<List<GetExternalTestNoListResponse>>>(jsonStr) ?? throw new Exception("委外试样编号接口解析有误，请联系管理员");

            if (result.Status != 200)
            {
                throw new Exception(result.Message);
            }
            if (result.Data == null || result.Data == null)
            {
                throw new Exception("试样编号接口返回有误，请联系管理员");
            }
            return result.Data;
        }

        public async Task<List<GetFileCategoryListResponse>> GetFileCategoryTreeAsync()
        {
            var getFileCategoryListUrl = connection.Table<SystemConfig>().LastOrDefault(t => t.Key.Equals("GetFileCategoryListUrl"))?.Value;

            if (string.IsNullOrEmpty(getFileCategoryListUrl))
            {
                throw new Exception("文件系统目录接口未指定，请联系管理员");
            }

            var json = JsonConvert.SerializeObject(new GetFileCategoryListRequest { });


            var data = new StringContent(json, Encoding.UTF8, "application/json");

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginToken.AccessToken);

            var response = await httpClient.PostAsync(getFileCategoryListUrl, data);

            var jsonStr = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ResponseBody<List<GetFileCategoryListResponse>>>(jsonStr) ?? throw new Exception("文件系统目录接口解析有误，请联系管理员");

            if (result.Status != 200)
            {
                throw new Exception(result.Message);
            }
            if (result.Data == null)
            {
                throw new Exception("文件系统目录接口返回有误，请联系管理员");
            }
            return result.Data;
        }

        public async Task<int> CreateCategoryAsync(string categoryName)
        {
            var createCategoryUrl = connection.Table<SystemConfig>().LastOrDefault(t => t.Key.Equals("CreateCategoryUrl"))?.Value;

            if (string.IsNullOrEmpty(createCategoryUrl))
            {
                throw new Exception("创建文件系统目录接口未指定，请联系管理员");
            }

            var json = JsonConvert.SerializeObject(new CreateCategoryRequest { CategoryName = categoryName });

            var data = new StringContent(json, Encoding.UTF8, "application/json");

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginToken.AccessToken);

            var response = await httpClient.PostAsync(createCategoryUrl, data);

            var jsonStr = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ResponseBody<int>>(jsonStr) ?? throw new Exception("创建文件系统目录接口解析有误，请联系管理员");

            if (result.Status != 200)
            {
                throw new Exception(result.Message);
            }
            if (result.Data == 0)
            {
                throw new Exception("创建文件系统目录接口返回有误，请联系管理员");
            }
            return result.Data;
        }

        public async Task<int> UploadFileAsync(FileUploader uploadResult)
        {
            var uploadFileUrl = connection.Table<SystemConfig>().LastOrDefault(t => t.Key.Equals("UploadFileUrl"))?.Value;

            if (string.IsNullOrEmpty(uploadFileUrl))
            {
                throw new Exception("上传文件接口未指定，请联系管理员");
            }

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginToken.AccessToken);
            // 创建 MultipartFormDataContent 实例
            using var content = new MultipartFormDataContent
            {
                { new StringContent(uploadResult.FileManagerId.ToString()), "fileManagerId" },
                { new StringContent(uploadResult.FileName), "fileName" },
                { new StringContent(string.Empty), "fileUrl" },
                { new StringContent("4"), "fileType" },
                { new StringContent(uploadResult.FileCategory), "fileCategory" },
                { new StringContent(uploadResult.FileCategoryId.ToString()), "fileCategoryId" },
                { new StringContent(uploadResult.FileVersion), "fileVersion" },
                { new StringContent(LoginUserInfo.Username), "creator" },
                { new StringContent(string.Empty), "fileTag" },
                { new StringContent("1"), "isClassify" },
                { new StringContent(uploadResult.UploadFileId ?? "null"), "uploadFileId" },
                { new StringContent(uploadResult.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")), "createTime" },
                { new StringContent(string.Empty), "fileIntroduction" },
                { new StringContent("WU_FILE_1"), "id" },
                { new StringContent(uploadResult.FileName), "name" },
                { new StringContent("application/pdf"), "type" }
            };
            FileInfo fileInfo = new(uploadResult.LocalFilePath);
            content.Add(new StringContent(TimeHelper.FormateJavaScriptToString(fileInfo.LastWriteTime)), "lastModifiedDate");
            // 读取二进制文件数据
            byte[] fileBytes = File.ReadAllBytes(uploadResult.LocalFilePath); // 替换为你的文件路径
            // 创建 ByteArrayContent 实例来封装二进制数据
            using var byteArrayContent = new ByteArrayContent(fileBytes);
            // 设置二进制数据的 Content-Type（如果需要，默认为 application/octet-stream）
            byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            // 将二进制数据添加到 MultipartFormDataContent 中
            content.Add(new StringContent(fileBytes.Length.ToString()), "size");
            // 第三个参数是文件名（可选，但通常用于服务器端处理）
            // "{\"error\":null,\"data\":{\"isOk\":false,\"msg\":\"上传失败：请求不包含文件\"},\"status\":200,\"msg\":\"操作成功\",\"duration\":745}"
            content.Add(byteArrayContent, "file", uploadResult.FileName); // 可以只传递文件名，如果不需要完整路径
            var response = await httpClient.PostAsync(uploadFileUrl, content);
            var jsonStr = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseBody<UploadFileResponse>>(jsonStr) ?? throw new Exception("上传文件接口解析有误，请联系管理员");

            if (result.Status != 200)
            {
                throw new Exception(result.Message);
            }
            if (result.Data == null)
            {
                throw new Exception("上传文件接口返回有误，请联系管理员");
            }
            if (!result.Data.IsOK)
            {
                throw new Exception(result.Data.Message);
            }
            return result.Data.FileManagerId;
        }

        public async Task<GetFileByIdResponse> GetFileByIdAsync(int fileManagerId)
        {
            var getFileByIdUrl = connection.Table<SystemConfig>().LastOrDefault(t => t.Key.Equals("GetFileByIdUrl"))?.Value;

            if (string.IsNullOrEmpty(getFileByIdUrl))
            {
                throw new Exception("获取文件信息接口未指定，请联系管理员");
            }

            getFileByIdUrl = $"{getFileByIdUrl}?id={fileManagerId}";
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginToken.AccessToken);
            var jsonStr = await httpClient.GetStringAsync(getFileByIdUrl);

            var result = JsonConvert.DeserializeObject<ResponseBody<GetFileByIdResponse>>(jsonStr) ?? throw new Exception("获取文件信息接口解析有误，请联系管理员");

            if (result.Status != 200)
            {
                throw new Exception(result.Message);
            }

            if (result.Data == null)
            {
                throw new Exception("获取文件信息接口返回有误，请联系管理员");
            }

            return result.Data;
        }

        public async Task<BlacknessResultResponse> FindBlacknessResultAsync(string testNo, string coilNumber)
        {
            var findBlacknessResultUrl = connection.Table<SystemConfig>().LastOrDefault(t => t.Key.Equals("FindBlacknessResultUrl"))?.Value;

            if (string.IsNullOrEmpty(findBlacknessResultUrl))
            {
                throw new Exception("查询黑度试验接口未指定，请联系管理员");
            }

            var json = JsonConvert.SerializeObject(new FindBlacknessResultRequest { CoilNumber = coilNumber, TestNo = testNo });

            var data = new StringContent(json, Encoding.UTF8, "application/json");

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginToken.AccessToken);

            var response = await httpClient.PostAsync(findBlacknessResultUrl, data);

            var jsonStr = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ResponseBody<List<BlacknessResultResponse>>>(jsonStr) ?? throw new Exception("查询黑度试验接口解析有误，请联系管理员");

            if (result.Status != 200)
            {
                throw new Exception(result.Message);
            }
            if (result.Data == null)
            {
                throw new Exception("查询黑度试验接口返回有误，请联系管理员");
            }
            if (result.Data.Count > 1)
            {
                throw new Exception("业务系统中黑度试验数据重复，请联系管理员");
            }
            return result.Data.FirstOrDefault();
        }

        public async Task<string> UploadBlacknessResultAsync(BlacknessResultResponse uploadInfo)
        {
            var uploadBlacknessResultUrl = connection.Table<SystemConfig>().LastOrDefault(t => t.Key.Equals("UploadBlacknessResultUrl"))?.Value;

            if (string.IsNullOrEmpty(uploadBlacknessResultUrl))
            {
                throw new Exception("上传黑度试验接口未指定，请联系管理员");
            }

            var json = JsonConvert.SerializeObject(uploadInfo);

            var data = new StringContent(json, Encoding.UTF8, "application/json");

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginToken.AccessToken);

            var response = await httpClient.PostAsync(uploadBlacknessResultUrl, data);

            var jsonStr = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ResponseBody<BlacknessResultResponse>>(jsonStr) ?? throw new Exception("上传黑度试验接口解析有误，请联系管理员");

            if (result.Status != 200)
            {
                throw new Exception(result.Message);
            }

            if (result.Data == null || string.IsNullOrEmpty(result.Data.Id))
            {
                throw new Exception("上传黑度试验接口返回有误，请联系管理员");
            }

            return result.Data.Id;
        }

        public async Task DeleteUploadedFileAsync(int fileManagerId)
        {
            var deleteUploadedFileUrl = connection.Table<SystemConfig>().LastOrDefault(t => t.Key.Equals("DeleteUploadedFileUrl"))?.Value;

            if (string.IsNullOrEmpty(deleteUploadedFileUrl))
            {
                throw new Exception("删除已上传文件接口未指定，请联系管理员");
            }

            var json = JsonConvert.SerializeObject(new int[] { fileManagerId });

            var data = new StringContent(json, Encoding.UTF8, "application/json");

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginToken.AccessToken);

            var response = await httpClient.PostAsync(deleteUploadedFileUrl, data);

            var jsonStr = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ResponseBody<string>>(jsonStr) ?? throw new Exception("删除已上传文件接口解析有误，请联系管理员");

            if (result.Status != 200)
            {
                throw new Exception(result.Message);
            }
        }
    }
}
