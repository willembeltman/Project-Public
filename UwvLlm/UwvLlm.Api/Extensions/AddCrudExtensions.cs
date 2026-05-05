using gAPI.Core.Interfaces;
using UwvLlm.Infrastructure.Data.Entities;
using UwvLlm.Infrastructure.Data.Mappings;
using UwvLlm.Infrastructure.Data.UseCases;

namespace UwvLlm.Api.Extensions;

public static class AddCrudExtensions
{
    public static IServiceCollection AddCrudUseCases(this IServiceCollection services)
    {
        services.AddScoped<IUseCase<MailMessage, UwvLlm.Shared.Dtos.MailMessage, Guid>, MailMessagesUseCase>();
        services.AddScoped<IUseCase<UserNotification, UwvLlm.Shared.Dtos.UserNotification, long>, UserNotificationsUseCase>();
        services.AddScoped<IUseCase<User, UwvLlm.Shared.Dtos.User, Guid>, UsersUseCase>();
        return services;
    }

    public static IServiceCollection AddCrudMappings(this IServiceCollection services)
    {
        services.AddScoped<Mapping<MailMessage, UwvLlm.Shared.Dtos.MailMessage>, MailMessagesMapping>();
        services.AddScoped<Mapping<UserNotification, UwvLlm.Shared.Dtos.UserNotification>, UserNotificationsMapping>();
        services.AddScoped<Mapping<User, UwvLlm.Shared.Dtos.User>, UsersMapping>();
        return services;
    }
}