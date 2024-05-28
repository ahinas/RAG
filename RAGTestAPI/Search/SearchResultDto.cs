
namespace RAGTestAPI.Search
{
    public class SearchResultDto
    {
        public string Content { get; internal set; }
        public IEnumerable<string> Urls { get; internal set; }
        public IEnumerable<string> Filepaths { get; internal set; }
    }
}