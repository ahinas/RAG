using System.Text.Json.Serialization;

namespace RAGTestAPI.Search
{
    public class AzureSearchResponse
    {
        [JsonPropertyName("content")]
        public required string Content { get; set; }
        [JsonPropertyName("url")]
        public required string Url { get; set; }
        [JsonPropertyName("filepath")]
        public required string Filepath { get; set; }
    }


}
