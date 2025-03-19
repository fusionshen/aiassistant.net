using AI_Assistant_Win.Models.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AI_Assistant_Win.Models.Response
{
    public class CircularAreaResultResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("testNo")]
        public string TestNo { get; set; }

        [JsonProperty("coilNumber")]
        public string CoilNumber { get; set; }

        [JsonProperty("testSource")]
        public int Source { get; set; }

        [JsonProperty("itemId")]
        public string ItemId { get; set; } = "DXCZLCW";

        [JsonProperty("items")]
        public List<CircularAreaItemResponse> Items { get; set; }

        [JsonProperty("creator")]
        public string Creator { get; set; }

        [JsonProperty("createTime")]
        public DateTime? CreateTime { get; set; }

        [JsonProperty("revisor")]
        public string LastReviser { get; set; }

        [JsonProperty("reviseTime")]
        public DateTime? LastModifiedTime { get; set; }

        [JsonProperty("entityState")]
        public EntityStateKind? EntityState { get; set; }
    }

    public class CircularAreaItemResponse
    {
        [JsonProperty("workGroup")]
        public string WorkGroup { get; set; }

        [JsonProperty("scaleId")]
        public int ScaleId { get; set; }

        [JsonProperty("position")]
        public CircularPositionKind Position { get; set; }

        [JsonProperty("testCount")]
        public int Nth { get; set; }

        [JsonProperty("pixels")]
        public long Pixels { get; set; }

        [JsonProperty("confidence")]
        public double Confidence { get; set; }

        [JsonProperty("area")]
        public double Area { get; set; }

        [JsonProperty("diameter")]
        public double Diameter { get; set; }

        [JsonProperty("prediction")]
        public string Prediction { get; set; }

        [JsonProperty("creator")]
        public string Analyst { get; set; }

        [JsonProperty("createTime")]
        public DateTime? CreateTime { get; set; }

        [JsonProperty("revisor")]
        public string LastReviser { get; set; }

        [JsonProperty("reviseTime")]
        public DateTime? LastModifiedTime { get; set; }

        [JsonProperty("entityState")]
        public EntityStateKind? EntityState { get; set; }
    }
}
