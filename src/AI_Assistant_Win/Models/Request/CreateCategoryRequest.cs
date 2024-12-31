using Newtonsoft.Json;

namespace AI_Assistant_Win.Models.Request
{
    public class CreateCategoryRequest
    {
        [JsonProperty("parentId")]
        public int ParentId { get; set; } = 0;

        [JsonProperty("categoryName")]
        public string CategoryName { get; set; }
    }
}
