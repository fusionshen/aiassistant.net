using Newtonsoft.Json;

namespace AI_Assistant_Win.Models.Request
{
    public class GetFileCategoryListRequest
    {
        [JsonProperty("categoryId")]
        public string CategoryId { get; set; } = string.Empty;
    }
}
