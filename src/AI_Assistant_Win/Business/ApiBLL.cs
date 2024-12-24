using AI_Assistant_Win.Models;
using AI_Assistant_Win.Models.Request;
using AI_Assistant_Win.Models.Response;
using AI_Assistant_Win.Utils;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AI_Assistant_Win.Business
{
    /// <summary>
    /// TODO: login、upload
    /// </summary>
    public class ApiBLL
    {
        private readonly HttpClient httpClient = ApiHandler.Instance.GetHttpClient();

        private readonly SQLiteConnection connection = SQLiteHandler.Instance.GetSQLiteConnection();

        public async Task<LoginToken> LoginAsync(string username, string password)
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

            return result.Data.Token;
        }
    }
}
