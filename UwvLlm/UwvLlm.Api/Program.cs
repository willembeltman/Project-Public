using gAPI.Core.Interfaces;
using gAPI.Core.Server;
using gAPI.Core.Server.Extensions;
using gAPI.Core.Server.Mappings;
using gAPI.Generated;
using Scalar.AspNetCore;
using UwvLlm.Api.Extensions;
using UwvLlm.Core.Extensions;
using UwvLlm.LlmProxy.Core.Handlers;
using UwvLlm.Api.Core.Interfaces;
using UwvLlm.Api.Core.Services;
using UwvLlm.Shared;
using UwvLlm.Shared.Dtos;
using UwvLlm.Api.Core.Mappings;

var builder = WebApplication.CreateBuilder(args);

var serverConfig = builder.Configuration.CreateServerConfig();

builder.Services.AddOpenApi();
builder.Services.AddAutoApi(serverConfig);
builder.Services.AddAutoSse(serverConfig);
builder.Services.AddStorage(serverConfig);
builder.Services.AddCommenServices(serverConfig);
builder.Services.AddDatabase(serverConfig);
builder.Services.AddAuthenticationServices<UwvLlm.Infrastructure.Data.Entities.User, State>();
builder.Services.AddScoped<IStateMapping<UwvLlm.Infrastructure.Data.Entities.User, State>, StateMapping>();
builder.Services.AddScoped<IStateUserMapping<UwvLlm.Infrastructure.Data.Entities.User, StateUser>, StateUserMapping>();
builder.Services.AddScoped<IStateParser<State>, StateParser>();
builder.Services.AddCrudMappings();
builder.Services.AddCrudUseCases();

builder.Services.AddSingleton<IRabbitConnectionProvider, RabbitConnectionProvider>();
builder.Services.AddSingleton<HandlerRegistry>();
builder.Services.AddSingleton<ServiceBusReceiver>();
builder.Services.AddSingleton<ServiceBusSender>();

builder.Services.AddTransient<IHandler, GenerateAutoReplyResponseHandler>();

var app = builder.Build();

app.MapAutoApi<AuthenticationMiddleware<UwvLlm.Infrastructure.Data.Entities.User, State>>();
app.MapAutoSse();
app.UseHttpsRedirection();
app.MapOpenApi();
app.MapScalarApiReference();

app.Run();