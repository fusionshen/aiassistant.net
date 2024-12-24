using Newtonsoft.Json;

namespace AI_Assistant_Win.Models.Response
{
    public class LoginResponse
    {
        [JsonProperty("identifier")]
        public Identifier Identifier { get; set; }
        [JsonProperty("token")]
        public LoginToken Token { get; set; }
        [JsonProperty("cookieOptions")]
        public CookieOptions CookieOptions { get; set; }
        [JsonProperty("retryLeft")]
        public int RetryLeft { get; set; }
        [JsonProperty("lockedTimeSpan")]
        public int LockedTimeSpan { get; set; }
        [JsonProperty("result")]
        public int Result { get; set; }
        [JsonProperty("resultText")]
        public string ResultText { get; set; }
    }

    public class Identifier
    {
        [JsonProperty("tenantId")]
        public int TenantId { get; set; }
        [JsonProperty("userId")]
        public int UserId { get; set; }
        [JsonProperty("userName")]
        public string Username { get; set; }
        [JsonProperty("tenantName")]
        public string TenantName { get; set; }
        [JsonProperty("tenancySide")]
        public int TenancySide { get; set; }
        [JsonProperty("groupId")]
        public int GroupId { get; set; }
    }

    public class LoginToken
    {
        [JsonProperty("userId")]
        public int UserId { get; set; }
        [JsonProperty("userName")]
        public string Username { get; set; }
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }
        [JsonProperty("expiresIn")]
        public int ExpiresIn { get; set; }
        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }
        [JsonProperty("refreshExpiresIn")]
        public int RefreshExpiresIn { get; set; }
    }

    public class CookieOptions
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("isEnabled")]
        public bool IsEnabled { get; set; }
        [JsonProperty("domain")]
        public string Domain { get; set; }
        [JsonProperty("expiresIn")]
        public int ExpiresIn { get; set; }
        [JsonProperty("loginPath")]
        public string LoginPath { get; set; }
        [JsonProperty("logoutPath")]
        public string LogoutPath { get; set; }
    }
}
