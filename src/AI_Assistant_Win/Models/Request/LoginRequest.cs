using Newtonsoft.Json;

namespace AI_Assistant_Win.Models.Request
{
    public class LoginRequest
    {
        [JsonProperty("userName")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("source")]
        public int Source { get; set; } = 1;
    }
}
