using gAPI.Interfaces;
using UwvLlm.Core.CrudUseCases;
using UwvLlm.Core.Mappings;

namespace UwvLlm.Core.Extensions;

public static class AddCrudExtensions
{
    public static IServiceCollection AddCrudUseCases(this IServiceCollection services)
    {
        services.AddScoped<IUseCase<UwvLlm.Core.Infrastructure.Data.MailMessage, UwvLlm.Shared.Dtos.MailMessage, Guid>, MailMessagesUseCase>();
        services.AddScoped<IUseCase<UwvLlm.Core.Infrastructure.Data.MailMessageToUser, UwvLlm.Shared.Dtos.MailMessageToUser, long>, MailMessageToUsersUseCase>();
        services.AddScoped<IUseCase<UwvLlm.Core.Infrastructure.Data.User, UwvLlm.Shared.Dtos.User, Guid>, UsersUseCase>();
        return services;
    }

    public static IServiceCollection AddCrudMappings(this IServiceCollection services)
    {
        services.AddScoped<Mapping<UwvLlm.Core.Infrastructure.Data.MailMessage, UwvLlm.Shared.Dtos.MailMessage>, MailMessagesMapping>();
        services.AddScoped<Mapping<UwvLlm.Core.Infrastructure.Data.MailMessageToUser, UwvLlm.Shared.Dtos.MailMessageToUser>, MailMessageToUsersMapping>();
        services.AddScoped<Mapping<UwvLlm.Core.Infrastructure.Data.User, UwvLlm.Shared.Dtos.User>, UsersMapping>();
        return services;
    }
}