using gAPI.Core.Server.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UwvLlm.Api.Core.Infrastructure.Llm.Clients;
using UwvLlm.Api.Core.Infrastructure.Llm.Interfaces;
using UwvLlm.Infrastructure.Messaging.Interfaces;
using UwvLlm.Infrastructure.Messaging.Services;
using UwvLlm.LlmProxy.Core.Handlers;
using UwvLlm.LlmProxy.Extensions;

var builder = Host.CreateApplicationBuilder(args);
var serverConfig = builder.Configuration.CreateServerConfig();

builder.Services.AddStorage(serverConfig);
builder.Services.AddCommenServices(serverConfig);
builder.Services.AddDatabase(serverConfig);
builder.Services.AddSingleton<IConsoleService, ConsoleService>();
builder.Services.AddSingleton<ILlmClient, OllamaClient>();

builder.Services.AddSingleton<IRabbitConnectionProvider, RabbitConnectionProvider>();
builder.Services.AddSingleton<IHandlerRegistry, HandlerRegistry>();
builder.Services.AddSingleton<IServiceBusReceiver, ServiceBusReceiver>();
builder.Services.AddSingleton<IServiceBusSender, ServiceBusSender>();

builder.Services.AddTransient<GenerateAutoReplyRequestHandler>();

var app = builder.Build();
await app.StartConsoleAsync();