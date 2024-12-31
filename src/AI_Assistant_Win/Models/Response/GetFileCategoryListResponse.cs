using Newtonsoft.Json;
using System.Collections.Generic;

namespace AI_Assistant_Win.Models.Response
{
    public class GetFileCategoryListResponse
    {
        [JsonProperty("parentId")]
        public int ParentId { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("value")]
        public CategoryValue Value { get; set; }

        [JsonProperty("children")]
        public List<GetFileCategoryListResponse> Children { get; set; }
    }

    public class CategoryValue
    {
        [JsonProperty("categoryId")]
        public int CategoryId { get; set; }

        [JsonProperty("parentId")]
        public int ParentId { get; set; }

        [JsonProperty("categoryName")]
        public string CategoryName { get; set; }

        [JsonProperty("seqNo")]
        public int SeqNo { get; set; }

        [JsonProperty("fullPath")]
        public string FullPath { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }
    }
}
