using gAPI.Core.Interfaces;
using gAPI.Core.Server;
using gAPI.Core.Server.Extensions;
using gAPI.Core.Server.Mappings;
using gAPI.Generated;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using UwvLlm.Api.Core.Enums;
using UwvLlm.Api.Extensions;
using UwvLlm.Core.Extensions;
using UwvLlm.Infrastructure.Data.Entities;
using UwvLlm.Infrastructure.Data.Mappings;
using UwvLlm.Infrastructure.Messaging.Interfaces;
using UwvLlm.Infrastructure.Messaging.Services;
using UwvLlm.LlmProxy.Core.Handlers;
using UwvLlm.Shared;
using UwvLlm.Shared.Dtos;

var builder = WebApplication.CreateBuilder(args);

var serverConfig = builder.Configuration.CreateServerConfig();

builder.Services.AddOpenApi();
builder.Services.AddAutoApi(serverConfig);
builder.Services.AddAutoSse(serverConfig);
builder.Services.AddStorage(serverConfig);
builder.Services.AddCommenServices(serverConfig);
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddAuthenticationServices<UwvLlm.Infrastructure.Data.Entities.User, State>();
builder.Services.AddScoped<IStateMapping<UwvLlm.Infrastructure.Data.Entities.User, State>, StateMapping>();
builder.Services.AddScoped<IStateUserMapping<UwvLlm.Infrastructure.Data.Entities.User, StateUser>, StateUserMapping>();
builder.Services.AddScoped<IStateParser<State>, StateParser>();
builder.Services.AddCrudMappings();
builder.Services.AddCrudUseCases();
builder.Services.AddSingleton<IConsoleService, ConsoleService>();

builder.Services.AddSingleton<IRabbitConnectionProvider, RabbitConnectionProvider>();
builder.Services.AddSingleton<IHandlerRegistry, HandlerRegistry>();
builder.Services.AddSingleton<IServiceBusReceiver, ServiceBusReceiver>();
builder.Services.AddSingleton<IServiceBusSender, ServiceBusSender>();

builder.Services.AddTransient<GenerateAutoReplyResponseHandler>();

var app = builder.Build();

app.MapAutoApi<AuthenticationMiddleware<UwvLlm.Infrastructure.Data.Entities.User, State>>();
app.MapAutoSse();
app.UseHttpsRedirection();
app.MapOpenApi();
app.MapScalarApiReference();

using (var scope = app.Services.CreateScope())
{
    var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
    var db = factory.CreateDbContext();

    db.Database.Migrate();
}

var receiver = app.Services.GetRequiredService<IServiceBusReceiver>();
_ = Task.Run(async () => await receiver.StartAsync(Receipent.Api, CancellationToken.None));

app.Run();