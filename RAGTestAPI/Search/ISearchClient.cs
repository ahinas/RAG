namespace RAGTestAPI.Search
{
    public interface ISearchClient
    {
        SearchResultDto Search(string searchText);
    }
}
