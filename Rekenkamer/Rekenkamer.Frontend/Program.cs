using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Rekenkamer.Frontend;
using gAPI.Generated;
using gAPI.Core.Client.Config;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiUrl = builder.Configuration["ApiBackendUrl"]
    ?? throw new InvalidOperationException("ApiBackendUrl ontbreekt.");

var wssUrl = builder.Configuration["WssBackendUrl"]
    ?? throw new InvalidOperationException("WssBackendUrl ontbreekt.");

var maxPackageSize = Convert.ToInt32(builder.Configuration["MaxPackageSize"]
    ?? (8 * 1024 * 1024).ToString());

var logLevel = builder.Configuration.GetValue<LogLevel>("Logging:LogLevel:gAPI", LogLevel.Error);

#if DEBUG
Console.WriteLine($"[Debug WASM] API URL = {apiUrl}");
Console.WriteLine($"[Debug WASM] WSS URL = {wssUrl}");
#endif

var clientConfig = new ClientConfig(apiUrl, wssUrl, maxPackageSize, logLevel);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddAutoWssClient(clientConfig);
builder.Services.AddAutoAuthClient(clientConfig);

await builder.Build().RunAsync();
