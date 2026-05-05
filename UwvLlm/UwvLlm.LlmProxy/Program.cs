using gAPI.Core.Server.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UwvLlm.Llm.Core;
using UwvLlm.Llm.Core.Handlers;
using UwvLlm.Llm.Core.Services;
using UwvLlm.Llm.Extensions;
using UwvLlm.Shared.Private.Interfaces;
using UwvLlm.Shared.Private.Services;

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