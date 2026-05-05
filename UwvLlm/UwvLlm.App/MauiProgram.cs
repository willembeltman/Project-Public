using gAPI.Core.Client;
using gAPI.Generated;
using gAPI.Core.Interfaces;
using Microsoft.Extensions.Logging;
using UwvLlm.App.Pages;
using UwvLlm.App.Services;
using UwvLlm.App.Core.ViewModels;
using UwvLlm.Shared;
using UwvLlm.Shared.Dtos;
using UwvLlm.Shared.Interfaces;
using UwvLlm.App.Core.Interfaces;
using UwvLlm.App.Core.Services;

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

        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<EmailPage>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<NotificationsPage>();
        builder.Services.AddTransient<RegisterPage>();

        builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
        builder.Services.AddScoped<IEmailService, EmailService>();
        builder.Services.AddScoped<IDispatcherService, DispatcherService>();
        builder.Services.AddScoped<NavigationService>();
        builder.Services.AddScoped<INavigationService>(sp => sp.GetRequiredService<NavigationService>());
        builder.Services.AddScoped<INavigationManager>(sp => sp.GetRequiredService<NavigationService>());
        builder.Services.AddScoped<INotificationHub>(sp => sp.GetRequiredService<NotificationPageViewModel>());
        builder.Services.AddScoped<IUiService, UiService>();

        builder.Services.AddScoped<NotificationHubViewModel>();
        builder.Services.AddScoped<EmailViewModel>();
        builder.Services.AddScoped<LoginViewModel>();
        builder.Services.AddScoped<MainPageViewModel>();
        builder.Services.AddScoped<NotificationPageViewModel>();
        builder.Services.AddScoped<RegisterViewModel>();

        builder.Services.AddAutoApiClient(); 
        builder.Services.AddAutoSseClient();
        builder.Services.AddAuthenticationServices<State>(builder.Configuration["FrontendConfig:ApiBackendUrl"] ?? "https://localhost:7281");
        builder.Services.AddScoped<IStateParser<State>, StateParser>();

        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        Routing.RegisterRoute(nameof(EmailPage), typeof(EmailPage));
        Routing.RegisterRoute(nameof(NotificationsPage), typeof(NotificationsPage));

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
