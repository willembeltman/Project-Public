using gAPI.Storage.Server;

var builder = WebApplication.CreateBuilder(args);

builder.AddStorageServer();

var app = builder.Build();

app.MapStorageServer();

app.UseHttpsRedirection();

app.Run();