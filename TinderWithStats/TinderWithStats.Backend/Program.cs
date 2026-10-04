using gAPI.Core.Server.Extensions;
using gAPI.Generated;
using System.Globalization;
using TinderWithStats.Backend.Extensions;

var invariantCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentCulture = invariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = invariantCulture;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoWssServer(builder.Configuration);
builder.Services.AddAutoAuthServer(builder.Configuration);
builder.Services.AddStorage(builder.Configuration);
builder.Services.AddCrudMappings();
builder.Services.AddCrudUseCases();

var app = builder.Build();

app.MapAutoWssServer();
app.MapAutoAuthServer();

app.UseHttpsRedirection();
app.Run();
