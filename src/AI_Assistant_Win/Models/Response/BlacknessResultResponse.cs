using AI_Assistant_Win.Models.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AI_Assistant_Win.Models.Response
{
    public class BlacknessResultResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("coilNumber")]
        public string CoilNumber { get; set; }

        [JsonProperty("testNo")]
        public string TestNo { get; set; }

        [JsonProperty("originImagePath")]
        public string OriginImagePath { get; set; }

        [JsonProperty("renderImagePath")]
        public string RenderImagePath { get; set; }

        [JsonProperty("size")]
        public string Size { get; set; }

        [JsonProperty("isOK")]
        public bool IsOK { get; set; }

        [JsonProperty("surfaceOPLevel")]
        public string SurfaceOPLevel { get; set; }

        [JsonProperty("surfaceOPWidth")]
        public float SurfaceOPWidth { get; set; }

        [JsonProperty("surfaceOPScore")]
        public float SurfaceOPScore { get; set; }

        [JsonProperty("surfaceCELevel")]
        public string SurfaceCELevel { get; set; }

        [JsonProperty("surfaceCEWidth")]
        public float? SurfaceCEWidth { get; set; }

        [JsonProperty("surfaceCEScore")]
        public float? SurfaceCEScore { get; set; }

        [JsonProperty("surfaceDRLevel")]
        public string SurfaceDRLevel { get; set; }

        [JsonProperty("surfaceDRWidth")]
        public float SurfaceDRWidth { get; set; }

        [JsonProperty("surfaceDRScore")]
        public float SurfaceDRScore { get; set; }

        [JsonProperty("insideOPLevel")]
        public string InsideOPLevel { get; set; }

        [JsonProperty("insideOPWidth")]
        public float InsideOPWidth { get; set; }

        [JsonProperty("insideOPScore")]
        public float InsideOPScore { get; set; }

        [JsonProperty("insideCELevel")]
        public string InsideCELevel { get; set; }

        [JsonProperty("insideCEWidth")]
        public float InsideCEWidth { get; set; }

        [JsonProperty("insideCEScore")]
        public float InsideCEScore { get; set; }

        [JsonProperty("insideDRLevel")]
        public string InsideDRLevel { get; set; }

        [JsonProperty("insideDRWidth")]
        public float InsideDRWidth { get; set; }

        [JsonProperty("insideDRScore")]
        public float InsideDRScore { get; set; }

        [JsonProperty("isUploaded")]
        public bool IsUploaded { get; set; }

        [JsonProperty("uploader")]
        public string Uploader { get; set; }

        [JsonProperty("uploadTime")]
        public DateTime? UploadTime { get; set; }

        [JsonProperty("workGroup")]
        public string WorkGroup { get; set; }

        [JsonProperty("analyst")]
        public string Analyst { get; set; }

        [JsonProperty("createTime")]
        public DateTime? CreateTime { get; set; }

        [JsonProperty("lastReviser")]
        public string LastReviser { get; set; }

        [JsonProperty("lastModifiedTime")]
        public DateTime? LastModifiedTime { get; set; }

        [JsonProperty("reportFileId")]
        public string ReportFileId { get; set; }

        [JsonProperty("detail")]
        public List<BlacknessItemResponse> Details { get; set; }

        [JsonProperty("entityState")]
        public EntityStateKind? EntityState { get; set; }
    }

    public class BlacknessItemResponse
    {
        [JsonProperty("location")]
        public BlacknessLocationKind Location { get; set; }

        [JsonProperty("level")]
        public string Level { get; set; }

        [JsonProperty("score")]
        public float Score { get; set; }

        [JsonProperty("width")]
        public float Width { get; set; }

        [JsonProperty("prediction")]
        public string Prediction { get; set; }
    }
}
