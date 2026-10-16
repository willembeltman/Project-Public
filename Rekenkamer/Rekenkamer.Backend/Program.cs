using gAPI.Core.Server.Extensions;
using gAPI.Generated;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using OllamaSharp;
using Rekenkamer.Backend.Extensions;
using Rekenkamer.Backend.Services;
using Rekenkamer.Infrastructure.Data.Entities;
using Rekenkamer.Infrastructure.Data.Extensions;
using Rekenkamer.Infrastructure.Import;

var builder = WebApplication.CreateBuilder(args);

// Confgig
var serverConfig = builder.Configuration.CreateServerConfig();
var llmConfig = builder.Configuration.CreateLlmConfig();

// To console = docker only
Console.WriteLine("API STARTED");
Console.WriteLine($"API FrontendUrl = {serverConfig.FrontendUrl}");
Console.WriteLine($"API OllamaUrl = {llmConfig.OllamaUrl}");
Console.WriteLine($"API VectorModel = {llmConfig.VectorModel}");
Console.WriteLine($"API VectorDimensions = {llmConfig.VectorDimensions}");

// gAPI generated services
builder.Services.AddAutoWssServer(serverConfig);
builder.Services.AddAutoAuthServer(serverConfig);
builder.Services.AddCrudUseCases();
builder.Services.AddCrudMappings();
//builder.Services.AddConsoleDashboard(serverConfig); // No dashboard needed in docker, but fun feature

// Overschrijven var datacontext met speciale vector context
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
{
    var dbConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(dbConnectionString, o => o.UseVector());
});

// Embedding / Llm stuff
builder.Services.AddSingleton(llmConfig);
builder.Services.AddSingleton(sp => new OllamaApiClient(new Uri(llmConfig.OllamaUrl), llmConfig.VectorModel));
builder.Services.AddSingleton<IEmbeddingGenerator<string, Embedding<float>>>(sp => sp.GetRequiredService<OllamaApiClient>());

// Upload service
builder.Services.AddHostedService<UploadHostedService>();
builder.Services.AddSingleton<UploadService>();
builder.Services.AddSingleton<PdfProcessor>();

// Build the app
var app = builder.Build();

// Map Wss endpoints
app.MapAutoWssServer();

// Map Authentication endpoints
app.MapAutoAuthServer();

// Initialize LLm Model(s)
await app.Services.TryInitilizeLlmModels(llmConfig);

// Then committing some docker crimes
app.Services.TryMigrateDatabase();

// And run it!
app.Run();

