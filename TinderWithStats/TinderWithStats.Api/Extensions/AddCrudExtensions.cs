using gAPI.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using TinderWithStats.Backend.Mappings;
using TinderWithStats.Backend.UseCases;

namespace TinderWithStats.Api.Extensions;

public static class AddCrudExtensions
{
    public static IServiceCollection AddCrudUseCases(this IServiceCollection services)
    {
        services.AddScoped<IUseCase<TinderWithStats.Backend.Entities.Profile, TinderWithStats.Shared.Dtos.Profile, Guid>, ProfilesUseCase>();
        services.AddScoped<IUseCase<TinderWithStats.Backend.Entities.ProfilePicture, TinderWithStats.Shared.Dtos.ProfilePicture, Guid>, ProfilePicturesUseCase>();
        services.AddScoped<IUseCase<TinderWithStats.Backend.Entities.Location, TinderWithStats.Shared.Dtos.Location, int>, LocationsUseCase>();
        services.AddScoped<IUseCase<TinderWithStats.Backend.Entities.User, TinderWithStats.Shared.Dtos.User, Guid>, UsersUseCase>();
        return services;
    }

    public static IServiceCollection AddCrudMappings(this IServiceCollection services)
    {
        services.AddScoped<Mapping<TinderWithStats.Backend.Entities.Profile, TinderWithStats.Shared.Dtos.Profile>, ProfilesMapping>();
        services.AddScoped<Mapping<TinderWithStats.Backend.Entities.ProfilePicture, TinderWithStats.Shared.Dtos.ProfilePicture>, ProfilePicturesMapping>();
        services.AddScoped<Mapping<TinderWithStats.Backend.Entities.Location, TinderWithStats.Shared.Dtos.Location>, LocationsMapping>();
        services.AddScoped<Mapping<TinderWithStats.Backend.Entities.User, TinderWithStats.Shared.Dtos.User>, UsersMapping>();
        return services;
    }
}