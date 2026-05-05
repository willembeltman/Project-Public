using gAPI.Core.Interfaces;
using UwvLlm.Api.Core.Mappings;
using UwvLlm.Api.Core.UseCases;

namespace UwvLlm.Api.Extensions;

public static class AddCrudExtensions
{
    public static IServiceCollection AddCrudUseCases(this IServiceCollection services)
    {
        services.AddScoped<IUseCase<UwvLlm.Api.Core.Infrastructure.Data.MailMessage, UwvLlm.Shared.Public.Dtos.MailMessage, Guid>, MailMessagesUseCase>();
        services.AddScoped<IUseCase<UwvLlm.Api.Core.Infrastructure.Data.UserNotification, UwvLlm.Shared.Public.Dtos.UserNotification, long>, UserNotificationsUseCase>();
        services.AddScoped<IUseCase<UwvLlm.Api.Core.Infrastructure.Data.User, UwvLlm.Shared.Public.Dtos.User, Guid>, UsersUseCase>();
        return services;
    }

    public static IServiceCollection AddCrudMappings(this IServiceCollection services)
    {
        services.AddScoped<Mapping<UwvLlm.Api.Core.Infrastructure.Data.MailMessage, UwvLlm.Shared.Public.Dtos.MailMessage>, MailMessagesMapping>();
        services.AddScoped<Mapping<UwvLlm.Api.Core.Infrastructure.Data.UserNotification, UwvLlm.Shared.Public.Dtos.UserNotification>, UserNotificationsMapping>();
        services.AddScoped<Mapping<UwvLlm.Api.Core.Infrastructure.Data.User, UwvLlm.Shared.Public.Dtos.User>, UsersMapping>();
        return services;
    }
}