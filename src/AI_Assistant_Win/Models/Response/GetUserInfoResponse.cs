using Newtonsoft.Json;
using System;

namespace AI_Assistant_Win.Models.Response
{
    public class GetUserInfoResponse
    {
        [JsonProperty("userId")]
        public int UserId { get; set; }
        [JsonProperty("userName")]
        public string Username { get; set; }
        [JsonProperty("nickName")]
        public string Nickname { get; set; }
        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }
        [JsonProperty("isActive")]
        public bool IsActive { get; set; }
        [JsonProperty("lastLoginTime")]
        public DateTime? LastLoginTime { get; set; }
        [JsonProperty("createTime")]
        public DateTime? CreateTime { get; set; }
        [JsonProperty("jobNumber")]
        public string JobNumber { get; set; }
        [JsonProperty("departmentId")]
        public string DepartmentId { get; set; }
        [JsonProperty("departmentName")]
        public string DepartmentName { get; set; }
        [JsonProperty("mobilePhone")]
        public string MobilePhone { get; set; }
        [JsonProperty("tenantId")]
        public int TenantId { get; set; }
        [JsonProperty("tenantName")]
        public string TenantName { get; set; }
        [JsonProperty("tenantDisplay")]
        public string TenantDisplay { get; set; }
        [JsonProperty("tenantShortName")]
        public string TenantShortName { get; set; }
        [JsonProperty("groupId")]
        public int GroupId { get; set; }
        [JsonProperty("tenancyType")]
        public int TenancyType { get; set; }
        [JsonProperty("avatar")]
        public string Avatar { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("theme")]
        public string Theme { get; set; }
        [JsonProperty("bgImagePath")]
        public string BgImagePath { get; set; }
    }
}
