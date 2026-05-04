using gAPI.Core.Server;
using gAPI.Core.Server.Mappings;
using gAPI.Extensions;
using gAPI.Generated;
using gAPI.Interfaces;
using Scalar.AspNetCore;
using UwvLlm.Core.Extensions;
using UwvLlm.Core.Infrastructure.Data;
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
builder.Services.AddAuthenticationServices<User, StateDto>();
builder.Services.AddScoped<IStateMapping<User, StateDto>, StateMapping>();
builder.Services.AddScoped<IStateUserMapping<User, StateUserDto>, StateUserMapping>();
builder.Services.AddScoped<IStateParser<StateDto>, StateParser>();

var app = builder.Build();

app.MapAutoApi<AuthenticationMiddleware<User, StateDto>>();
app.MapAutoSse();
app.UseHttpsRedirection();
app.MapOpenApi();
app.MapScalarApiReference();

app.Run();