using Newtonsoft.Json;

namespace AI_Assistant_Win.Models.Request
{
    public class GetExternalTestNoListRequest
    {
        [JsonProperty("itemId")]
        public string ItemId { get; set; }

        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        [JsonProperty("sampleId")]
        public string SampleId { get; set; }
    }
}
