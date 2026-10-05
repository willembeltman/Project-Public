using gAPI.Core.Client.Config;
using gAPI.Generated;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Globalization;
using TinderWithStats.Frontend.Webassembly;

var invariantCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentCulture = invariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = invariantCulture;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiUrl = builder.Configuration["ApiBackendUrl"]
    ?? throw new InvalidOperationException("ApiBackendUrl ontbreekt.");
var wssUrl = builder.Configuration["WssBackendUrl"]
    ?? throw new InvalidOperationException("WssBackendUrl ontbreekt.");
var logLevel = builder.Configuration.GetValue<LogLevel>("Logging:LogLevel:gAPI", LogLevel.Error);

var clientConfig = new ClientConfig(apiUrl, wssUrl, logLevel);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddAutoWssClient(clientConfig);
builder.Services.AddAutoAuthClient(clientConfig);

await builder.Build().RunAsync();
