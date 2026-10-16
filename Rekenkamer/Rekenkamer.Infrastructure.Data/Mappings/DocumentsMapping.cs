using gAPI.Core.Server.Extensions;

namespace Rekenkamer.Infrastructure.Data.Mappings;

public class DocumentsMapping(
    gAPI.Core.Interfaces.IUseCase<Rekenkamer.Infrastructure.Data.Entities.Document, Rekenkamer.Shared.Dtos.Document, Guid> useCase) 
    : gAPI.Core.Interfaces.Mapping<Rekenkamer.Infrastructure.Data.Entities.Document, Rekenkamer.Shared.Dtos.Document>
{
    public override Rekenkamer.Infrastructure.Data.Entities.Document ToEntity(
        Rekenkamer.Shared.Dtos.Document dto, 
        Rekenkamer.Infrastructure.Data.Entities.Document entity)
    {
        entity.Id = dto.Id;
        entity.FullName = dto.FullName;
        entity.CreatedAt = dto.CreatedAt;
        entity.Size = dto.Size;
        entity.RelativePath = dto.RelativePath;
        entity.Name = dto.Name;

        return entity;
    }

    public override async Task<Rekenkamer.Shared.Dtos.Document> ToDtoAsync(
        Rekenkamer.Infrastructure.Data.Entities.Document entity, 
        Rekenkamer.Shared.Dtos.Document dto,
        CancellationToken ct)
    {
        dto.Id = entity.Id;
        dto.FullName = entity.FullName;
        dto.CreatedAt = entity.CreatedAt;
        dto.Size = entity.Size;
        dto.RelativePath = entity.RelativePath;
        dto.Name = entity.Name;
        
        await ExtendDto(dto, ct);

        return dto;
    }

    public override IAsyncEnumerable<Rekenkamer.Shared.Dtos.Document> ProjectToDtosAsync(
        IQueryable<Rekenkamer.Infrastructure.Data.Entities.Document> entities,
        string[]? orderby, 
        int? skip, 
        int? take,
        CancellationToken ct)
    {  
        var dtos = entities
            .Select(entity => new Rekenkamer.Shared.Dtos.Document()
            {
                Id = entity.Id,
                FullName = entity.FullName,
                CreatedAt = entity.CreatedAt,
                Size = entity.Size,
                RelativePath = entity.RelativePath,
                Name = entity.Name,
#nullable disable
#nullable enable
            })
            .ApplyOrderBy(orderby);

        if (skip != null)
        {
            dtos = dtos.Skip(skip.Value);
        }
        if (take != null)
        {
            dtos = dtos.Take(take.Value);
        }

        return EnumerateDtosAsync(dtos, ct);
    }

    public override async Task ExtendDto(
        Rekenkamer.Shared.Dtos.Document dto,
        CancellationToken ct)
    {
        dto.CanUpdate = await useCase.CanUpdateAsync(dto, ct);
        dto.CanDelete = await useCase.CanDeleteAsync(dto, ct);
    }
}