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
using UwvLlm.Shared.Interfaces;

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

        builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
        builder.Services.AddTransient<IEmailService, EmailService>();
        builder.Services.AddTransient<INavigationService, NavigationService>();
        builder.Services.AddTransient<INavigationManager, NavigationService>();
        builder.Services.AddTransient<INotificationHub>(sp => sp.GetRequiredService<NotificationPageViewModel>());
        builder.Services.AddTransient<IUiService, UiService>();

        builder.Services.AddTransient<NotificationHubViewModel>();
        builder.Services.AddTransient<EmailViewModel>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<MainPageViewModel>();
        builder.Services.AddTransient<NotificationPageViewModel>();
        builder.Services.AddTransient<RegisterViewModel>();

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
