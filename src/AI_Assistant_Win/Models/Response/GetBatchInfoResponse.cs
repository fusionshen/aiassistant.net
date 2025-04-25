using Newtonsoft.Json;

namespace AI_Assistant_Win.Models.Response
{
    public class GetBatchInfoResponse
    {
        /// <summary>
        /// 试块编号
        /// </summary>
        [JsonProperty("samplelotno")]
        public string TestNo { get; set; }
        /// <summary>
        /// 厚度
        /// </summary>
        [JsonProperty("order_thick")]
        public decimal? Thick { get; set; }
        /// <summary>
        /// 宽度
        /// </summary>
        [JsonProperty("order_width")]
        public decimal? Width { get; set; }
    }
}
