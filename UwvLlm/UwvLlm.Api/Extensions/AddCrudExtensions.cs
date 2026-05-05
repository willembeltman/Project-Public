using gAPI.Core.Interfaces;
using UwvLlm.Infrastructure.Data.Mappings;
using UwvLlm.Infrastructure.Data.UseCases;

namespace UwvLlm.Api.Extensions;

public static class AddCrudExtensions
{
    public static IServiceCollection AddCrudUseCases(this IServiceCollection services)
    {
        services.AddScoped<IUseCase<UwvLlm.Infrastructure.Data.Entities.MailMessage, UwvLlm.Shared.Dtos.MailMessage, Guid>, MailMessagesUseCase>();
        services.AddScoped<IUseCase<UwvLlm.Infrastructure.Data.Entities.UserNotification, UwvLlm.Shared.Dtos.UserNotification, long>, UserNotificationsUseCase>();
        services.AddScoped<IUseCase<UwvLlm.Infrastructure.Data.Entities.User, UwvLlm.Shared.Dtos.User, Guid>, UsersUseCase>();
        return services;
    }

    public static IServiceCollection AddCrudMappings(this IServiceCollection services)
    {
        services.AddScoped<Mapping<UwvLlm.Infrastructure.Data.Entities.MailMessage, UwvLlm.Shared.Dtos.MailMessage>, MailMessagesMapping>();
        services.AddScoped<Mapping<UwvLlm.Infrastructure.Data.Entities.UserNotification, UwvLlm.Shared.Dtos.UserNotification>, UserNotificationsMapping>();
        services.AddScoped<Mapping<UwvLlm.Infrastructure.Data.Entities.User, UwvLlm.Shared.Dtos.User>, UsersMapping>();
        return services;
    }
}