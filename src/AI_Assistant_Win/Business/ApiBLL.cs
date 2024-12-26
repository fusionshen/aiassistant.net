using AI_Assistant_Win.Models;
using AI_Assistant_Win.Models.Request;
using AI_Assistant_Win.Models.Response;
using AI_Assistant_Win.Utils;
using Newtonsoft.Json;
using SQLite;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AI_Assistant_Win.Business
{
    /// <summary>
    /// TODO: upload
    /// </summary>
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
            var json = JsonConvert.SerializeObject(new LoginRequest { Username = $"{username}@lims", Password = password });

            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var loginUrl = connection.Table<SystemConfig>().LastOrDefault(t => t.Key.Equals("LoginURL"))?.Value;

            if (string.IsNullOrEmpty(loginUrl))
            {
                throw new Exception("登录接口未指定，请联系管理员");
            }

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
    }
}
