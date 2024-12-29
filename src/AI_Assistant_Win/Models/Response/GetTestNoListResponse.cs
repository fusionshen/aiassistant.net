using Newtonsoft.Json;

namespace AI_Assistant_Win.Models.Response
{
    public class GetTestNoListResponse
    {
        /// <summary>
        /// 试样编号
        /// </summary>
        [JsonProperty("samplenum")]
        public string TestNo { get; set; }
        /// <summary>
        /// 样品编号
        /// </summary>
        [JsonProperty("sampleid")]
        public string SampleNo { get; set; }
        /// <summary>
        /// 试批号
        /// </summary>
        [JsonProperty("samplelotno")]
        public string BatchNo { get; set; }
        /// <summary>
        /// 牌号（钢级）
        /// </summary>
        [JsonProperty("sg_sign")]
        public string SteelGrade { get; set; }
        /// <summary>
        /// 代表材料号（钢卷号）
        /// </summary>
        [JsonProperty("rep_mat_no")]
        public string CoilNumber { get; set; }
        /// <summary>
        /// 代表材料号（钢卷号）
        /// </summary>
        [JsonProperty("rep_mat_no1")]
        public string OtherCoilNumber { get; set; }
    }
}
