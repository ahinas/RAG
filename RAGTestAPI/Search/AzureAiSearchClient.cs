using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using System.Text.Json.Serialization;

namespace RAGTestAPI.Search
{
    public class AzureAiSearchClient : ISearchClient
    {
        private SecretAppsettingReader _secretAppsettingReader;

        public AzureAiSearchClient(SecretAppsettingReader secretAppsettingReader)
        {
            _secretAppsettingReader = secretAppsettingReader;
        }

        public SearchResultDto Search(string question)
        {
            var apiKey = _secretAppsettingReader.GetAzureSearchApiKey();
            var searchServiceEndPoint = _secretAppsettingReader.GetAzureSearchEndPoint();
            var indexName = _secretAppsettingReader.GetAzureSearchIndex();

            var serviceEndpoint = new Uri(searchServiceEndPoint);
            var credential = new AzureKeyCredential(apiKey);
            var srchclient = new SearchClient(serviceEndpoint, indexName, credential);

            var options = new SearchOptions()
            {
                IncludeTotalCount = true,
            };

            options.Select.Add("content");
            options.Select.Add("url");
            options.Select.Add("filepath");

            var searchResponse = (SearchResults<AzureSearchResponse>)srchclient.Search<AzureSearchResponse>(question, options);

            var topResponses = searchResponse.GetResults().OrderByDescending(r => r.Score).Take(3);

            return new SearchResultDto
            {
                Content = string.Join("\n\n\n", topResponses.Select(searchResponse => searchResponse.Document.Content)),
                Urls = topResponses.Select(searchResponse => searchResponse.Document.Url).Distinct(),
                Filepaths = topResponses.Select(searchResponse => searchResponse.Document.Filepath).Distinct()
            };

        }
    }

    sealed class AzureSearchResponse
    {
        [JsonPropertyName("content")]
        public required string Content { get; set; }
        [JsonPropertyName("url")]
        public required string Url { get; set; }
        [JsonPropertyName("filepath")]
        public required string Filepath { get; set; }
    }

}
