using Newtonsoft.Json;
using System;

namespace AI_Assistant_Win.Models.Response
{
    public class GetExternalTestNoListResponse
    {
        /// <summary>
        /// 委托单号
        /// </summary>
        [JsonProperty("orderId")]
        public string OrderId { get; set; }
        /// <summary>
        /// 样品号
        /// </summary>
        [JsonProperty("sampleId")]
        public string SampleId { get; set; }
        /// <summary>
        /// <summary>
        /// 试批号
        /// </summary>
        [JsonProperty("sampleNo")]
        public string SampleNo { get; set; }
        /// <summary>
        /// 外来样片号
        /// </summary>
        [JsonProperty("outSampleId")]
        public string OutSampleId { get; set; }
        /// <summary>
        /// 材料号
        /// </summary>
        [JsonProperty("materialId")]
        public string MaterialId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("trialId")]
        public string TrialId { get; set; }
        /// <summary>
        /// 样片位置
        /// </summary>
        [JsonProperty("samplePosition")]
        public string SamplePosition { get; set; }
        /// <summary>
        /// 位置
        /// </summary>
        [JsonProperty("shiftClasses")]
        public string ShiftClasses { get; set; }
        /// <summary>
        /// 是否确认
        /// </summary>
        [JsonProperty("isConfirm")]
        public int IsConfirm { get; set; }
        /// <summary>
        /// 确认时间
        /// </summary>
        [JsonProperty("confirmTime")]
        public DateTime ConfirmTime { get; set; }
        /// <summary>
        /// 序列号
        /// </summary>
        [JsonProperty("seqNo")]
        public string SeqNo { get; set; }
        /// <summary>
        /// 录入状态
        /// </summary>
        [JsonProperty("stateTag")]
        public int StateTag { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [JsonProperty("entrustReason")]
        public string EntrustReason { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("dataId")]
        public string DataId { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [JsonProperty("creator")]
        public string Creator { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [JsonProperty("createTime")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 最近修改人
        /// </summary>
        [JsonProperty("revisor")]
        public string LastReviser { get; set; }
        /// <summary>
        /// 最近修改时间
        /// </summary>
        [JsonProperty("reviseTime")]
        public DateTime? LastModifiedTime { get; set; }
        /// <summary>
        /// 备案状态
        /// </summary>
        [JsonProperty("archiveFlag")]
        public int ArchiveFlag { get; set; }
        /// <summary>
        /// 操作状态
        /// </summary>
        [JsonProperty("entityState")]
        public int EntityState { get; set; }
    }
}
