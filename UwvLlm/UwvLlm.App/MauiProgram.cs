using gAPI.Core.Client;
using gAPI.Generated;
using gAPI.Interfaces;
using Microsoft.Extensions.Logging;
using UwvLlm.App.Interfaces;
using UwvLlm.App.Pages;
using UwvLlm.App.Services;
using UwvLlm.App.ViewModels;
using UwvLlm.Shared;
using UwvLlm.Shared.Dtos;

namespace UwvLlm.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<IUiService, UiService>();
        builder.Services.AddTransient<INavigationManager>(sp => sp.GetRequiredService<IUiService>());

        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<EmailViewModel>();

        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<EmailPage>();

        builder.Services.AddAutoApiClient(); 
        builder.Services.AddAutoSseClient();
        builder.Services.AddAuthenticationServices(builder.Configuration["FrontendConfig:ApiBackendUrl"] ?? "https://localhost:7281");
        builder.Services.AddScoped<IStateParser<State>, StateParser>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
