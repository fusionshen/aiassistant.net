using Newtonsoft.Json;

namespace AI_Assistant_Win.Models.Response
{
    public class UploadFileResponse
    {
        [JsonProperty("isOk")]
        public bool IsOK { get; set; }

        [JsonProperty("msg")]
        public string Message { get; set; }

        [JsonProperty("fileManagerId")]
        public int FileManagerId { get; set; }
    }
}
