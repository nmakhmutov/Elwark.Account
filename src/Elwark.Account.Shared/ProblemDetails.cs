using Newtonsoft.Json;

namespace Elwark.Account.Shared
{
    public class ProblemDetails
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "status")]
        public int Status { get; set; }

        [JsonProperty(PropertyName = "detail")]
        public string Detail { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "instance")]
        public string? Instance { get; set; }
    }
}