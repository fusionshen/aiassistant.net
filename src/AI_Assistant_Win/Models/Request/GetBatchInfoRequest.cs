using Newtonsoft.Json;

namespace AI_Assistant_Win.Models.Request
{
    public class GetBatchInfoRequest
    {
        //[JsonProperty("startTime")]
        //public string StartTime { get; set; } = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd 00:00:00");

        //[JsonProperty("endTime")]
        //public string EndTime { get; set; } = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");

        [JsonProperty("whereString")]
        public string WhereString { get; set; }

        [JsonProperty("tableName")]
        public string TableName { get; set; } = "V_G1_SC_BATCH_INFO";

        //[JsonProperty("queryTimetype")]
        //public string QueryType { get; set; } = " ARRIVETIME";

        //[JsonProperty("pageIndex")]
        //public int PageIndex { get; set; }

        //[JsonProperty("pageSize")]
        //public int PageSize { get; set; }

        [JsonProperty("disablePage")]
        public bool DisablePage { get; set; } = true;
    }
}
