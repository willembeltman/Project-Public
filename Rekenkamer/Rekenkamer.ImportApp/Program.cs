using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OllamaSharp;
using Rekenkamer.ConsoleApp.Services;
using Rekenkamer.Infrastructure.Data.Entities;
using Rekenkamer.Infrastructure.Data.Extensions;
using Rekenkamer.Infrastructure.Import;

var builder = Host.CreateApplicationBuilder(args);

#if DEBUG
using var stream = File.OpenRead("appsettings.Development.json");
if (stream != null)
    builder.Configuration.AddJsonStream(stream);
#endif

// Config
var llmConfig = builder.Configuration.CreateLlmConfig();
var pdfConfig = builder.Configuration.CreatePdfConfig();
builder.Services.AddSingleton(pdfConfig);

// Service
builder.Services.AddHostedService<ReaderService>();
builder.Services.AddSingleton<PdfProcessor>();

// Database
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
{
    var dbConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(dbConnectionString, o => o.UseVector());
});

// Embedding
builder.Services.AddSingleton<IEmbeddingGenerator<string, Embedding<float>>>(sp =>
    new OllamaApiClient(new Uri(llmConfig.OllamaUrl), llmConfig.VectorModel));

var app = builder.Build();
app.Run();