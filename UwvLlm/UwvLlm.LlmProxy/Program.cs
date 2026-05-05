using gAPI.Core.Server.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UwvLlm.Llm.Core;
using UwvLlm.Llm.Core.Services;
using UwvLlm.Llm.Extensions;

var builder = Host.CreateApplicationBuilder(args);

var serverConfig = builder.Configuration.CreateServerConfig();

// registreer je services
builder.Services.AddSingleton<ServiceBusReceiverService>();
builder.Services.AddSingleton<ConsoleService>();
builder.Services.AddStorage(serverConfig);
builder.Services.AddCommenServices(serverConfig);
builder.Services.AddDatabase(serverConfig);

var app = builder.Build();
await app.CustomStartAsync();