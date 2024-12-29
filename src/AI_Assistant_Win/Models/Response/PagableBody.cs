using Newtonsoft.Json;

namespace AI_Assistant_Win.Models.Response
{
    public class PagableBody<T>
    {
        [JsonProperty("totalRecords")]
        public int TotalRecords { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }
    }
}
