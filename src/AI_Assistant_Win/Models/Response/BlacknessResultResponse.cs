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

        [JsonProperty("testSource")]
        public int Source { get; set; }

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
        public double SurfaceOPWidth { get; set; }

        [JsonProperty("surfaceOPScore")]
        public double SurfaceOPScore { get; set; }

        [JsonProperty("surfaceCELevel")]
        public string SurfaceCELevel { get; set; }

        [JsonProperty("surfaceCEWidth")]
        public double? SurfaceCEWidth { get; set; }

        [JsonProperty("surfaceCEScore")]
        public double? SurfaceCEScore { get; set; }

        [JsonProperty("surfaceDRLevel")]
        public string SurfaceDRLevel { get; set; }

        [JsonProperty("surfaceDRWidth")]
        public double SurfaceDRWidth { get; set; }

        [JsonProperty("surfaceDRScore")]
        public double SurfaceDRScore { get; set; }

        [JsonProperty("insideOPLevel")]
        public string InsideOPLevel { get; set; }

        [JsonProperty("insideOPWidth")]
        public double InsideOPWidth { get; set; }

        [JsonProperty("insideOPScore")]
        public double InsideOPScore { get; set; }

        [JsonProperty("insideCELevel")]
        public string InsideCELevel { get; set; }

        [JsonProperty("insideCEWidth")]
        public double InsideCEWidth { get; set; }

        [JsonProperty("insideCEScore")]
        public double InsideCEScore { get; set; }

        [JsonProperty("insideDRLevel")]
        public string InsideDRLevel { get; set; }

        [JsonProperty("insideDRWidth")]
        public double InsideDRWidth { get; set; }

        [JsonProperty("insideDRScore")]
        public double InsideDRScore { get; set; }

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
        [JsonProperty("testCount")]
        public int Nth { get; set; }

        [JsonProperty("location")]
        public BlacknessLocationKind Location { get; set; }

        [JsonProperty("level")]
        public string Level { get; set; }

        [JsonProperty("score")]
        public double Score { get; set; }

        [JsonProperty("width")]
        public double Width { get; set; }

        [JsonProperty("prediction")]
        public string Prediction { get; set; }
    }
}
