using gAPI.Core.Server.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UwvLlm.Api.Core.Interfaces;
using UwvLlm.Api.Core.Services;
using UwvLlm.LlmProxy.Core;
using UwvLlm.LlmProxy.Core.Handlers;
using UwvLlm.LlmProxy.Core.Services;
using UwvLlm.LlmProxy.Extensions;

var builder = Host.CreateApplicationBuilder(args);
var serverConfig = builder.Configuration.CreateServerConfig();

builder.Services.AddStorage(serverConfig);
builder.Services.AddCommenServices(serverConfig);
builder.Services.AddDatabase(serverConfig);
builder.Services.AddSingleton<ConsoleService>();

builder.Services.AddSingleton<IRabbitConnectionProvider, RabbitConnectionProvider>();
builder.Services.AddSingleton<HandlerRegistry>();
builder.Services.AddSingleton<ServiceBusReceiver>();
builder.Services.AddSingleton<ServiceBusSender>();

builder.Services.AddScoped<IHandler, GenerateAutoReplyRequestHandler>();

var app = builder.Build();
await app.StartProxyAsync();