using RAGTestAPI;
using RAGTestAPI.LLM;
using RAGTestAPI.Search;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ILLMClient, LLMClientSemanticKernel>();
builder.Services.AddTransient<ISearchClient, AzureAiSearchClient>();
builder.Services.AddTransient<SecretAppsettingReader>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.RoutePrefix = "swagger";
    c.SwaggerEndpoint(url: "/swagger/v1/swagger.json", "RAG-Test");
});

app.Run();
