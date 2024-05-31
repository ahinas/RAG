using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace RAGTestAPI.LLM
{
    public class LLMClientSemanticKernel : ILLMClient
    {
        private SecretAppsettingReader _secretAppsettingReader { get; }

        public LLMClientSemanticKernel(SecretAppsettingReader secretAppsettingReader)
        {
            _secretAppsettingReader = secretAppsettingReader;
        }

        public async Task<string> GetCompletionAsync(string question, string searchResult)
        {
          
            if (string.IsNullOrEmpty(searchResult))
                return "Tyvärr, jag kan inte svara på den frågan";


            var apiKey = _secretAppsettingReader.GetLLMApiKey();
            var endpoint = _secretAppsettingReader.GetLLMEndpoint();
            var modelName = _secretAppsettingReader.GetLLMModelName();

            var builder = Kernel.CreateBuilder()
            .AddAzureOpenAIChatCompletion(modelName, endpoint, apiKey);
          
            var kernel = builder.Build();

            var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
         
            var systemMessagePrompt = new ChatMessageContent(AuthorRole.System, @$"Ditt namn är AgeroBot och du är en svensk hjälpsam och glad assistant på företaget Agero. 
                    Ditt jobb är ENDAST att svarar på frågor om företagets personalinformation och INGET ANNAT.
                    Du svarar sammanfattat och enkelt och avslutar alltid med en rolig och passande emoji.

                    Utifrån informationen under rubriken information, svara på frågan. 
                    Om du inte kan svara på frågan med hjälp av Informationen så svara att du inte kan hjälpa till, men att användaren gärna får ställa en annan fråga.
                    =======            
                    Information:
                    {searchResult}     
                    =======");

            var userMessagePrompt = new ChatMessageContent(AuthorRole.User, @$"
                    =======
                    Fråga:
                    {question}
                    =======

                    Svar:");

            var history = new ChatHistory
            {
                systemMessagePrompt,
                userMessagePrompt
            };

            var result = await chatCompletionService.GetChatMessageContentsAsync(
                history,
                null,
                kernel);

            return result.First().ToString();
        }
    }

}
