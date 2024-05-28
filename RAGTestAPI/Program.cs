using OpenAI;
using RAGTestAPI.LLM;
using RAGTestAPI.Search;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ILLMClient, OpenAISemanticKernel>();
builder.Services.AddTransient<ISearchClient, AzureAiSearch>();
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
