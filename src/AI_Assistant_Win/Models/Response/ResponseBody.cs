using Newtonsoft.Json;

namespace AI_Assistant_Win.Models.Response
{
    public class ResponseBody<T>
    {
        [JsonProperty("status")]
        public int Status { get; set; }
        [JsonProperty("msg")]
        public string Message { get; set; }
        [JsonProperty("duration")]
        public long Duration { get; set; }
        [JsonProperty("error")]
        public string Error { get; set; }
        [JsonProperty("data")]
        public T Data { get; set; }
    }
}
