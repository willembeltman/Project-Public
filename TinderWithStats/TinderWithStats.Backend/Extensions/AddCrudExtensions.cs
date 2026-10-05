using gAPI.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using TinderWithStats.Backend.Mappings;
using TinderWithStats.Backend.UseCases;

namespace TinderWithStats.Backend.Extensions;

public static class AddCrudExtensions
{
    public static IServiceCollection AddCrudUseCases(this IServiceCollection services)
    {
        services.AddScoped<IUseCase<TinderWithStats.Backend.Entities.Profile, TinderWithStats.Shared.Dtos.Profile, Guid>, ProfilesUseCase>();
        services.AddScoped<IUseCase<TinderWithStats.Backend.Entities.ProfilePicture, TinderWithStats.Shared.Dtos.ProfilePicture, Guid>, ProfilePicturesUseCase>();
        services.AddScoped<IUseCase<TinderWithStats.Backend.Entities.Location, TinderWithStats.Shared.Dtos.Location, int>, LocationsUseCase>();
        services.AddScoped<IUseCase<TinderWithStats.Backend.Entities.Match, TinderWithStats.Shared.Dtos.Match, Guid>, MatchsUseCase>();
        services.AddScoped<IUseCase<TinderWithStats.Backend.Entities.ChatMessage, TinderWithStats.Shared.Dtos.ChatMessage, Guid>, ChatMessagesUseCase>();
        services.AddScoped<IUseCase<TinderWithStats.Backend.Entities.User, TinderWithStats.Shared.Dtos.User, Guid>, UsersUseCase>();
        return services;
    }

    public static IServiceCollection AddCrudMappings(this IServiceCollection services)
    {
        services.AddScoped<Mapping<TinderWithStats.Backend.Entities.Profile, TinderWithStats.Shared.Dtos.Profile>, ProfilesMapping>();
        services.AddScoped<Mapping<TinderWithStats.Backend.Entities.ProfilePicture, TinderWithStats.Shared.Dtos.ProfilePicture>, ProfilePicturesMapping>();
        services.AddScoped<Mapping<TinderWithStats.Backend.Entities.Location, TinderWithStats.Shared.Dtos.Location>, LocationsMapping>();
        services.AddScoped<Mapping<TinderWithStats.Backend.Entities.Match, TinderWithStats.Shared.Dtos.Match>, MatchsMapping>();
        services.AddScoped<Mapping<TinderWithStats.Backend.Entities.ChatMessage, TinderWithStats.Shared.Dtos.ChatMessage>, ChatMessagesMapping>();
        services.AddScoped<Mapping<TinderWithStats.Backend.Entities.User, TinderWithStats.Shared.Dtos.User>, UsersMapping>();
        return services;
    }
}