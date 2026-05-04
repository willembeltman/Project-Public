using gAPI.Core.Server;
using gAPI.Core.Server.Mappings;
using gAPI.Extensions;
using gAPI.Generated;
using gAPI.Interfaces;
using Scalar.AspNetCore;
using UwvLlm.Core.Extensions;
using UwvLlm.Core.Mappings;
using UwvLlm.Shared;
using UwvLlm.Shared.Dtos;

var builder = WebApplication.CreateBuilder(args);

var serverConfig = builder.Configuration.CreateServerConfig();

builder.Services.AddOpenApi();
builder.Services.AddAutoApi(serverConfig);
builder.Services.AddAutoSse(serverConfig);
builder.Services.AddStorage(serverConfig);
builder.Services.AddCommenServices(serverConfig);
builder.Services.AddDatabase(serverConfig);
builder.Services.AddAuthenticationServices<UwvLlm.Core.Infrastructure.Data.User, State>();
builder.Services.AddScoped<IStateMapping<UwvLlm.Core.Infrastructure.Data.User, State>, StateMapping>();
builder.Services.AddScoped<IStateUserMapping<UwvLlm.Core.Infrastructure.Data.User, StateUser>, StateUserMapping>();
builder.Services.AddScoped<IStateParser<State>, StateParser>();
builder.Services.AddCrudMappings();
builder.Services.AddCrudUseCases();

var app = builder.Build();

app.MapAutoApi<AuthenticationMiddleware<UwvLlm.Core.Infrastructure.Data.User, State>>();
app.MapAutoSse();
app.UseHttpsRedirection();
app.MapOpenApi();
app.MapScalarApiReference();

app.Run();