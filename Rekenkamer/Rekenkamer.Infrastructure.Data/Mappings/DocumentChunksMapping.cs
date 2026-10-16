using gAPI.Core.Server.Extensions;

namespace Rekenkamer.Infrastructure.Data.Mappings;

public class DocumentChunksMapping(
    gAPI.Core.Interfaces.IUseCase<Rekenkamer.Infrastructure.Data.Entities.DocumentChunk, Rekenkamer.Shared.Dtos.DocumentChunk, Guid> useCase) 
    : gAPI.Core.Interfaces.Mapping<Rekenkamer.Infrastructure.Data.Entities.DocumentChunk, Rekenkamer.Shared.Dtos.DocumentChunk>
{
    public override Rekenkamer.Infrastructure.Data.Entities.DocumentChunk ToEntity(
        Rekenkamer.Shared.Dtos.DocumentChunk dto, 
        Rekenkamer.Infrastructure.Data.Entities.DocumentChunk entity)
    {
        entity.Id = dto.Id;
        entity.DocumentId = dto.DocumentId;
        entity.Content = dto.Content;
        entity.PageNumber = dto.PageNumber;
        entity.ChunkIndex = dto.ChunkIndex;
        entity.Left = dto.Left;
        entity.Width = dto.Width;
        entity.Top = dto.Top;
        entity.Height = dto.Height;
        entity.ParagraphConfidence = dto.ParagraphConfidence;

        return entity;
    }

    public override async Task<Rekenkamer.Shared.Dtos.DocumentChunk> ToDtoAsync(
        Rekenkamer.Infrastructure.Data.Entities.DocumentChunk entity, 
        Rekenkamer.Shared.Dtos.DocumentChunk dto,
        CancellationToken ct)
    {
        dto.Id = entity.Id;
        dto.DocumentId = entity.DocumentId;
        dto.Content = entity.Content;
        dto.PageNumber = entity.PageNumber;
        dto.ChunkIndex = entity.ChunkIndex;
        dto.Left = entity.Left;
        dto.Width = entity.Width;
        dto.Top = entity.Top;
        dto.Height = entity.Height;
        dto.ParagraphConfidence = entity.ParagraphConfidence;
        
        dto.DocumentName = 
            ("" + (entity?.Document?.FullName ?? default) + "") + " " + 
                (" (" + ((entity?.Document?.CreatedAt ?? default).Year) + "-" + ("0" + (entity?.Document?.CreatedAt ?? default).Month).Substring(("0" + (entity?.Document?.CreatedAt ?? default).Month).Length - 2) + "-" + ("0" + (entity?.Document?.CreatedAt ?? default).Day).Substring(("0" + (entity?.Document?.CreatedAt ?? default).Day).Length - 2) + ")");

        await ExtendDto(dto, ct);

        return dto;
    }

    public override IAsyncEnumerable<Rekenkamer.Shared.Dtos.DocumentChunk> ProjectToDtosAsync(
        IQueryable<Rekenkamer.Infrastructure.Data.Entities.DocumentChunk> entities,
        string[]? orderby, 
        int? skip, 
        int? take,
        CancellationToken ct)
    {  
        var dtos = entities
            .Select(entity => new Rekenkamer.Shared.Dtos.DocumentChunk()
            {
                Id = entity.Id,
                DocumentId = entity.DocumentId,
                Content = entity.Content,
                PageNumber = entity.PageNumber,
                ChunkIndex = entity.ChunkIndex,
                Left = entity.Left,
                Width = entity.Width,
                Top = entity.Top,
                Height = entity.Height,
                ParagraphConfidence = entity.ParagraphConfidence,
#nullable disable
                DocumentName = 
                    ("" + entity.Document.FullName + "") + " " + 
                        (" (" + (entity.Document.CreatedAt.Year) + "-" + ("0" + entity.Document.CreatedAt.Month).Substring(("0" + entity.Document.CreatedAt.Month).Length - 2) + "-" + ("0" + entity.Document.CreatedAt.Day).Substring(("0" + entity.Document.CreatedAt.Day).Length - 2) + ")"),
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
        Rekenkamer.Shared.Dtos.DocumentChunk dto,
        CancellationToken ct)
    {
        dto.CanUpdate = await useCase.CanUpdateAsync(dto, ct);
        dto.CanDelete = await useCase.CanDeleteAsync(dto, ct);
    }
}