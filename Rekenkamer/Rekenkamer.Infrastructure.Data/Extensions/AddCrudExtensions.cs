using gAPI.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Rekenkamer.Infrastructure.Data.Mappings;
using Rekenkamer.Infrastructure.Data.UseCases;

namespace Rekenkamer.Infrastructure.Data.Extensions;

public static class AddCrudExtensions
{
    public static IServiceCollection AddCrudUseCases(this IServiceCollection services)
    {
        services.AddScoped<IUseCase<Rekenkamer.Infrastructure.Data.Entities.Document, Rekenkamer.Shared.Dtos.Document, Guid>, DocumentsUseCase>();
        services.AddScoped<IUseCase<Rekenkamer.Infrastructure.Data.Entities.DocumentChunk, Rekenkamer.Shared.Dtos.DocumentChunk, Guid>, DocumentChunksUseCase>();
        services.AddScoped<IUseCase<Rekenkamer.Infrastructure.Data.Entities.User, Rekenkamer.Shared.Dtos.User, Guid>, UsersUseCase>();
        return services;
    }

    public static IServiceCollection AddCrudMappings(this IServiceCollection services)
    {
        services.AddScoped<Mapping<Rekenkamer.Infrastructure.Data.Entities.Document, Rekenkamer.Shared.Dtos.Document>, DocumentsMapping>();
        services.AddScoped<Mapping<Rekenkamer.Infrastructure.Data.Entities.DocumentChunk, Rekenkamer.Shared.Dtos.DocumentChunk>, DocumentChunksMapping>();
        services.AddScoped<Mapping<Rekenkamer.Infrastructure.Data.Entities.User, Rekenkamer.Shared.Dtos.User>, UsersMapping>();
        return services;
    }
}