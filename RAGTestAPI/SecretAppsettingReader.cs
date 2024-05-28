namespace OpenAI
{
    public class SecretAppsettingReader
    {

        public string GetLLMApiKey()
        {
            return ReadSection<string>("LLM:ApiKey");
        }

        public string GetLLMEndpoint()
        {
            return ReadSection<string>("LLM:Endpoint");
        }

        public string GetAzureSearchApiKey()
        {
            return ReadSection<string>("Azure:SearchApiKey");
        }

        public string GetAzureSearchEndPoint()
        {

            return ReadSection<string>("Azure:SearchEndpont");

        }

        public string GetAzureSearchIndex()
        {

            return ReadSection<string>("Azure:SearchIndex");

        }


        public T ReadSection<T>(string sectionName)
        {
            var builder = new ConfigurationBuilder();
            builder.AddUserSecrets<Program>().AddEnvironmentVariables();
            var configurationRoot = builder.Build();

            return configurationRoot.GetSection(sectionName).Get<T>() ?? throw new Exception($"Value not found {sectionName}");
        }
    }
}
