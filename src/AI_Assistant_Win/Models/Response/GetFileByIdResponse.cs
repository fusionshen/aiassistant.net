using Newtonsoft.Json;
using System;

namespace AI_Assistant_Win.Models.Response
{
    public class GetFileByIdResponse
    {
        [JsonProperty("fileManagerId")]
        public int FileManagerId { get; set; }

        [JsonProperty("fileName")]
        public string FileName { get; set; }

        [JsonProperty("originalFileName")]
        public string OriginalFileName { get; set; }

        [JsonProperty("fileUrl")]
        public string FileUrl { get; set; }

        [JsonProperty("fileType")]
        public int FileType { get; set; }

        [JsonProperty("fileCategoryId")]
        public int FileCategoryId { get; set; }

        [JsonProperty("fileCategory")]
        public string FileCategory { get; set; }

        [JsonProperty("fileSize")]
        public float FileSize { get; set; }

        [JsonProperty("fileVersionId")]
        public int FileVersionId { get; set; }

        [JsonProperty("fileVersion")]
        public string FileVersion { get; set; }

        [JsonProperty("fileTag")]
        public string FileTag { get; set; }

        [JsonProperty("fileIntroduction")]
        public string FileIntroduction { get; set; }

        [JsonProperty("tenantId")]
        public int TenantId { get; set; }

        [JsonProperty("uploadFileId")]
        public string UploadFileId { get; set; }

        [JsonProperty("creator")]
        public string Creator { get; set; }

        [JsonProperty("createTime")]
        public DateTime CreateTime { get; set; }

        [JsonProperty("isClassify")]
        public int IsClassify { get; set; }
    }
}
