namespace RAGTestAPI.LLM
{
    public interface ILLMClient
    {
        Task<string> GetCompletionAsync(string question, string context);
    }
}
