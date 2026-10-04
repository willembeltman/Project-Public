using gAPI.Core.Server.Interfaces;
using Microsoft.EntityFrameworkCore;
using TinderWithStats.Backend.Entities;

namespace TinderWithStats.Backend.UseCases;

public class ProfilesUseCase(
    ApplicationDbContext db,
    IAuthenticationService<TinderWithStats.Backend.Entities.User, TinderWithStats.Shared.Dtos.State> authenticationService)
    : gAPI.Core.Interfaces.IUseCase<TinderWithStats.Backend.Entities.Profile, TinderWithStats.Shared.Dtos.Profile, Guid>
{
    public async Task<bool> IsAllowedAsync(CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanListAsync(CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanCreateAsync(CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanCreateAsync(TinderWithStats.Shared.Dtos.Profile dto, CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanReadAsync(TinderWithStats.Shared.Dtos.Profile dto, CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanUpdateAsync(TinderWithStats.Shared.Dtos.Profile dto, CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanDeleteAsync(TinderWithStats.Shared.Dtos.Profile dto, CancellationToken ct) => authenticationService.State.User != null;

    public async Task<Profile?> FindByMatchAsync(TinderWithStats.Shared.Dtos.Profile dto, CancellationToken ct) 
        => null; // If you implement this, also use includes
    public async Task<Profile?> FindByIdAsync(Guid id, CancellationToken ct) 
        => await db.Profiles
            .Include("User")
            .Include("Location") // Add your filter query
            .FirstOrDefaultAsync(a => a.Id == id, ct);
    public IQueryable<Profile> ListAll()
        => db.Profiles; // Add your filter query, no need for includes here

    public async Task<bool> AddAsync(Profile entityToAdd, CancellationToken ct) 
    {
        await db.Profiles.AddAsync(entityToAdd, ct);
        await db.SaveChangesAsync(ct);
        return true;
    }
    public async Task<bool> UpdateAsync(Profile updatedEntity, TinderWithStats.Shared.Dtos.Profile dto, CancellationToken ct)
    {
        await db.SaveChangesAsync();
        return true;
    }
    public async Task<bool> RemoveAsync(Profile entity, CancellationToken ct)
    {
        db.Profiles.Remove(entity);
        await db.SaveChangesAsync();
        return true;
    }
}
