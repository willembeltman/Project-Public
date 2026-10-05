using gAPI.Core.Server.Interfaces;
using Microsoft.EntityFrameworkCore;
using TinderWithStats.Backend.Entities;

namespace TinderWithStats.Backend.UseCases;

public class MatchsUseCase(
    ApplicationDbContext db,
    IAuthenticationService<TinderWithStats.Backend.Entities.User, TinderWithStats.Shared.Dtos.State> authenticationService)
    : gAPI.Core.Interfaces.IUseCase<TinderWithStats.Backend.Entities.Match, TinderWithStats.Shared.Dtos.Match, Guid>
{
    public async Task<bool> IsAllowedAsync(CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanListAsync(CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanCreateAsync(CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanCreateAsync(TinderWithStats.Shared.Dtos.Match dto, CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanReadAsync(TinderWithStats.Shared.Dtos.Match dto, CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanUpdateAsync(TinderWithStats.Shared.Dtos.Match dto, CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanDeleteAsync(TinderWithStats.Shared.Dtos.Match dto, CancellationToken ct) => authenticationService.State.User != null;

    public async Task<Match?> FindByMatchAsync(TinderWithStats.Shared.Dtos.Match dto, CancellationToken ct) 
        => await db.Matches
            .Include("ProfileSender")
            .Include("ProfileReceiver") // Add your filter query
            .FirstOrDefaultAsync(a => 
                a.MatchCreated == dto.MatchCreated &&
                a.MatchAccepted == dto.MatchAccepted, ct);
    public async Task<Match?> FindByIdAsync(Guid id, CancellationToken ct) 
        => await db.Matches
            .Include("ProfileSender")
            .Include("ProfileReceiver") // Add your filter query
            .FirstOrDefaultAsync(a => a.Id == id, ct);
    public IQueryable<Match> ListAll()
        => db.Matches; // Add your filter query, no need for includes here

    public async Task<bool> AddAsync(Match entityToAdd, CancellationToken ct) 
    {
        await db.Matches.AddAsync(entityToAdd, ct);
        await db.SaveChangesAsync(ct);
        return true;
    }
    public async Task<bool> UpdateAsync(Match updatedEntity, TinderWithStats.Shared.Dtos.Match dto, CancellationToken ct)
    {
        await db.SaveChangesAsync();
        return true;
    }
    public async Task<bool> RemoveAsync(Match entity, CancellationToken ct)
    {
        db.Matches.Remove(entity);
        await db.SaveChangesAsync();
        return true;
    }
}
