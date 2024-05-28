using Azure;
using Azure.AI.OpenAI;
using Microsoft.SemanticKernel;

namespace RAGTestAPI.LLM
{
    public class OpenAISemanticKernel : ILLMClient
    {
        private SecretAppsettingReader _secretAppsettingReader { get; }

        public OpenAISemanticKernel(SecretAppsettingReader secretAppsettingReader)
        {
            _secretAppsettingReader = secretAppsettingReader;
        }

        public async Task<string> GetCompletionAsync(string question, string context)
        {
            var apiKey = _secretAppsettingReader.ReadSection<string>("LLM:ApiKey");
            var endpoint = _secretAppsettingReader.ReadSection<string>("LLM:Endpoint");

            if (string.IsNullOrEmpty(context))
                return "Tyvärr, jag kan inte svara på den frågan";

            var client = new OpenAIClient(
            new Uri(endpoint),
            new AzureKeyCredential(apiKey));

            var kernel = Kernel.CreateBuilder()
            .AddAzureOpenAIChatCompletion("gpt-35-turbo", client)
            .Build();

            var template = kernel.CreateFunctionFromPrompt(@"
            <message role=""system"">{{$systemMessage}}</message>
            <message role=""user"">
            Beskrivning: 
            Utifrån informationen under rubriken information, svara på frågan. 
            Om du inte kan svara på frågan med hjälp av infomrationen så svara att du inte kan hjälpa till, men att användaren gärna får ställa en annan fråga.
            =======            
            Information:
            {{$context}}         
            =======

            =======
            Fråga:
            {{$question}}
            =======
            Svar:
            </message>
            ");

            var result = await kernel.InvokeAsync(template, new()
            {
                { "systemMessage", @"Du är en svensk hjälpsam och glad assistant på företaget Agero. 
                    Ditt jobb är att svarar på frågor om företagets personalinformation och inget annat.
                    Du svarar sammanfattat och enkelt och avslutar alltid med en rolig och passande emoji"},
                { "context", context },
                { "question", question },

            });

            return result.ToString();
        }
    }
}
