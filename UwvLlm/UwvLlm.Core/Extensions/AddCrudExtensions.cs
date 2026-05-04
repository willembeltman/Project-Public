using gAPI.Interfaces;
using UwvLlm.Core.CrudUseCases;
using UwvLlm.Core.Mappings;

namespace UwvLlm.Core.Extensions;

public static class AddCrudExtensions
{
    public static IServiceCollection AddCrudUseCases(this IServiceCollection services)
    {
        services.AddScoped<IUseCase<Infrastructure.Data.MailMessage, Shared.Dtos.MailMessage, Guid>, MailMessagesUseCase>();
        services.AddScoped<IUseCase<Infrastructure.Data.UserNotification, Shared.Dtos.UserNotification, long>, UserNotificationsUseCase>();
        services.AddScoped<IUseCase<Infrastructure.Data.UserNotificationQuickOption, Shared.Dtos.UserNotificationQuickOption, long>, UserNotificationQuickOptionsUseCase>();
        services.AddScoped<IUseCase<Infrastructure.Data.User, Shared.Dtos.User, Guid>, UsersUseCase>();
        return services;
    }

    public static IServiceCollection AddCrudMappings(this IServiceCollection services)
    {
        services.AddScoped<Mapping<Infrastructure.Data.MailMessage, Shared.Dtos.MailMessage>, MailMessagesMapping>();
        services.AddScoped<Mapping<Infrastructure.Data.UserNotification, Shared.Dtos.UserNotification>, UserNotificationsMapping>();
        services.AddScoped<Mapping<Infrastructure.Data.UserNotificationQuickOption, Shared.Dtos.UserNotificationQuickOption>, UserNotificationQuickOptionsMapping>();
        services.AddScoped<Mapping<Infrastructure.Data.User, Shared.Dtos.User>, UsersMapping>();
        return services;
    }
}