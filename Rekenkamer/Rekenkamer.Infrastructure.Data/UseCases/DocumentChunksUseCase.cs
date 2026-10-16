using gAPI.Core.Server.Entities;
using gAPI.Core.Server.Interfaces;
using Microsoft.EntityFrameworkCore;
using Rekenkamer.Infrastructure.Data.Entities;

namespace Rekenkamer.Infrastructure.Data.UseCases;

public class DocumentChunksUseCase(
    ApplicationDbContext db,
    IAuthenticationService<Rekenkamer.Infrastructure.Data.Entities.User, Rekenkamer.Shared.Dtos.State> authenticationService)
    : gAPI.Core.Interfaces.IUseCase<Rekenkamer.Infrastructure.Data.Entities.DocumentChunk, Rekenkamer.Shared.Dtos.DocumentChunk, Guid>
{
    public async Task<bool> IsAllowedAsync(CancellationToken ct) => true;
    public async Task<bool> CanListAsync(CancellationToken ct) => true;
    public async Task<bool> CanCreateAsync(CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanCreateAsync(Rekenkamer.Shared.Dtos.DocumentChunk dto, CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanReadAsync(Rekenkamer.Shared.Dtos.DocumentChunk dto, CancellationToken ct) => true;
    public async Task<bool> CanUpdateAsync(Rekenkamer.Shared.Dtos.DocumentChunk dto, CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanDeleteAsync(Rekenkamer.Shared.Dtos.DocumentChunk dto, CancellationToken ct) => authenticationService.State.User != null;

    public async Task<DocumentChunk?> FindByMatchAsync(Rekenkamer.Shared.Dtos.DocumentChunk dto, CancellationToken ct) 
        => null; // If you implement this, also use includes
    public async Task<DocumentChunk?> FindByIdAsync(Guid id, CancellationToken ct) 
        => await db.DocumentChunks
            .Include("Document") // Add your filter query
            .FirstOrDefaultAsync(a => a.Id == id, ct);
    public IQueryable<DocumentChunk> ListAll()
        => db.DocumentChunks; // Add your filter query, no need for includes here

    public async Task<bool> AddAsync(DocumentChunk entityToAdd, CancellationToken ct) 
    {
        await db.DocumentChunks.AddAsync(entityToAdd, ct);
        await db.SaveChangesAsync(ct);
        return true;
    }
    public async Task<bool> UpdateAsync(DocumentChunk updatedEntity, Rekenkamer.Shared.Dtos.DocumentChunk dto, CancellationToken ct)
    {
        await db.SaveChangesAsync();
        return true;
    }
    public async Task<bool> RemoveAsync(DocumentChunk entity, CancellationToken ct)
    {
        db.DocumentChunks.Remove(entity);
        await db.SaveChangesAsync();
        return true;
    }
}
