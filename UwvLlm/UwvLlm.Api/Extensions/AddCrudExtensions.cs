using gAPI.Interfaces;
using UwvLlm.Api.Core.UseCases;
using UwvLlm.Api.Core.Mappings;

namespace UwvLlm.Api.Extensions;

public static class AddCrudExtensions
{
    public static IServiceCollection AddCrudUseCases(this IServiceCollection services)
    {
        services.AddScoped<IUseCase<UwvLlm.Api.Core.Infrastructure.Data.MailMessage, UwvLlm.Shared.Dtos.MailMessage, Guid>, MailMessagesUseCase>();
        services.AddScoped<IUseCase<UwvLlm.Api.Core.Infrastructure.Data.UserNotification, UwvLlm.Shared.Dtos.UserNotification, long>, UserNotificationsUseCase>();
        services.AddScoped<IUseCase<UwvLlm.Api.Core.Infrastructure.Data.User, UwvLlm.Shared.Dtos.User, Guid>, UsersUseCase>();
        return services;
    }

    public static IServiceCollection AddCrudMappings(this IServiceCollection services)
    {
        services.AddScoped<Mapping<UwvLlm.Api.Core.Infrastructure.Data.MailMessage, UwvLlm.Shared.Dtos.MailMessage>, MailMessagesMapping>();
        services.AddScoped<Mapping<UwvLlm.Api.Core.Infrastructure.Data.UserNotification, UwvLlm.Shared.Dtos.UserNotification>, UserNotificationsMapping>();
        services.AddScoped<Mapping<UwvLlm.Api.Core.Infrastructure.Data.User, UwvLlm.Shared.Dtos.User>, UsersMapping>();
        return services;
    }
}