using Newtonsoft.Json;

namespace AI_Assistant_Win.Models.Request
{
    public class FindTestResultRequest
    {
        [JsonProperty("coilNumber")]
        public string CoilNumber { get; set; }

        [JsonProperty("testNo")]
        public string TestNo { get; set; }
    }
}
