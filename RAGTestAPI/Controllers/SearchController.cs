using Microsoft.AspNetCore.Mvc;
using RAGTestAPI.LLM;
using RAGTestAPI.Search;

namespace RAGTestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private ILLMClient _llmClient { get; }
        private ISearchClient _searchClient { get; }


        public SearchController(ILLMClient llmClient, ISearchClient searchClient)
        {
            this._llmClient = llmClient;
            this._searchClient = searchClient;
        }

        [HttpGet]
        public async Task<string> Get(string question)
        {
            var searchResult = _searchClient.Search(question);
            var answer = await _llmClient.GetCompletionAsync(question, searchResult.Content);

            return answer;
        }

    }
}
