using gAPI.Core.Interfaces;
using gAPI.Core.Server;
using gAPI.Core.Server.Extensions;
using gAPI.Core.Server.Mappings;
using gAPI.Generated;
using Scalar.AspNetCore;
using UwvLlm.Api.Core.Mappings;
using UwvLlm.Api.Extensions;
using UwvLlm.Core.Extensions;
using UwvLlm.Shared.Private.Interfaces;
using UwvLlm.Shared.Private.Services;
using UwvLlm.Shared.Public;
using UwvLlm.Shared.Public.Dtos;

var builder = WebApplication.CreateBuilder(args);

var serverConfig = builder.Configuration.CreateServerConfig();

builder.Services.AddOpenApi();
builder.Services.AddAutoApi(serverConfig);
builder.Services.AddAutoSse(serverConfig);
builder.Services.AddStorage(serverConfig);
builder.Services.AddCommenServices(serverConfig);
builder.Services.AddDatabase(serverConfig);
builder.Services.AddAuthenticationServices<UwvLlm.Api.Core.Infrastructure.Data.User, State>();
builder.Services.AddScoped<IStateMapping<UwvLlm.Api.Core.Infrastructure.Data.User, State>, StateMapping>();
builder.Services.AddScoped<IStateUserMapping<UwvLlm.Api.Core.Infrastructure.Data.User, StateUser>, StateUserMapping>();
builder.Services.AddScoped<IStateParser<State>, StateParser>();
builder.Services.AddCrudMappings();
builder.Services.AddCrudUseCases();

builder.Services.AddSingleton<IRabbitConnectionProvider, RabbitConnectionProvider>();
builder.Services.AddSingleton<HandlerRegistry>();
builder.Services.AddSingleton<ServiceBusReceiverService>();
builder.Services.AddSingleton<ServiceBusSenderService>();

builder.Services.AddScoped<IHandler, GenerateAutoReplyResponseHandler>();

var app = builder.Build();

app.MapAutoApi<AuthenticationMiddleware<UwvLlm.Api.Core.Infrastructure.Data.User, State>>();
app.MapAutoSse();
app.UseHttpsRedirection();
app.MapOpenApi();
app.MapScalarApiReference();

app.Run();