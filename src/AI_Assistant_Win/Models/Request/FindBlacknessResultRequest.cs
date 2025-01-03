using Newtonsoft.Json;

namespace AI_Assistant_Win.Models.Request
{
    public class FindBlacknessResultRequest
    {
        [JsonProperty("coilNumber")]
        public string CoilNumber { get; set; }

        [JsonProperty("testNo")]
        public string TestNo { get; set; }
    }
}
