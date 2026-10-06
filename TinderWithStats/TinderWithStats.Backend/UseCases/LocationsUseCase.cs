using gAPI.Core.Server.Interfaces;
using Microsoft.EntityFrameworkCore;
using TinderWithStats.Backend.Entities;

namespace TinderWithStats.Backend.UseCases;

public class LocationsUseCase(
    ApplicationDbContext db,
    IAuthenticationService<TinderWithStats.Backend.Entities.User, TinderWithStats.Shared.Dtos.State> authenticationService)
    : gAPI.Core.Interfaces.IUseCase<TinderWithStats.Backend.Entities.Location, TinderWithStats.Shared.Dtos.Location, int>
{
    public async Task<bool> IsAllowedAsync(CancellationToken ct)
    {
        if (authenticationService.State.User == null) return false;
        return await db.UserRoles.AnyAsync(a => a.UserId == authenticationService.State.User.Id && a.Role!.Name == "Admin", ct);
    }
    public async Task<bool> CanListAsync(CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanCreateAsync(CancellationToken ct)
    {
        if (authenticationService.State.User == null) return false;
        return await db.UserRoles.AnyAsync(a => a.UserId == authenticationService.State.User.Id && a.Role!.Name == "Admin", ct);
    }
    public async Task<bool> CanCreateAsync(TinderWithStats.Shared.Dtos.Location dto, CancellationToken ct)
    {
        if (authenticationService.State.User == null) return false;
        return await db.UserRoles.AnyAsync(a => a.UserId == authenticationService.State.User.Id && a.Role!.Name == "Admin", ct);
    }
    public async Task<bool> CanReadAsync(TinderWithStats.Shared.Dtos.Location dto, CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanUpdateAsync(TinderWithStats.Shared.Dtos.Location dto, CancellationToken ct)
    {
        if (authenticationService.State.User == null) return false;
        return await db.UserRoles.AnyAsync(a => a.UserId == authenticationService.State.User.Id && a.Role!.Name == "Admin", ct);
    }
    public async Task<bool> CanDeleteAsync(TinderWithStats.Shared.Dtos.Location dto, CancellationToken ct)
    {
        if (authenticationService.State.User == null) return false;
        return await db.UserRoles.AnyAsync(a => a.UserId == authenticationService.State.User.Id && a.Role!.Name == "Admin", ct);
    }

    public async Task<Location?> FindByMatchAsync(TinderWithStats.Shared.Dtos.Location dto, CancellationToken ct) 
        => null; // If you implement this, also use includes
    public async Task<Location?> FindByIdAsync(int id, CancellationToken ct) 
        => await db.Locations // Add your filter query
            .FirstOrDefaultAsync(a => a.Id == id, ct);
    public IQueryable<Location> ListAll()
        => db.Locations; // Add your filter query, no need for includes here

    public async Task<bool> AddAsync(Location entityToAdd, CancellationToken ct) 
    {
        await db.Locations.AddAsync(entityToAdd, ct);
        await db.SaveChangesAsync(ct);
        return true;
    }
    public async Task<bool> UpdateAsync(Location updatedEntity, TinderWithStats.Shared.Dtos.Location dto, CancellationToken ct)
    {
        await db.SaveChangesAsync();
        return true;
    }
    public async Task<bool> RemoveAsync(Location entity, CancellationToken ct)
    {
        db.Locations.Remove(entity);
        await db.SaveChangesAsync();
        return true;
    }
}
