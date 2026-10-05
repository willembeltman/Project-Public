using gAPI.Core.Server.Interfaces;
using Microsoft.EntityFrameworkCore;
using TinderWithStats.Backend.Entities;

namespace TinderWithStats.Backend.UseCases;

public class ProfilePicturesUseCase(
    ApplicationDbContext db,
    IAuthenticationService<TinderWithStats.Backend.Entities.User, TinderWithStats.Shared.Dtos.State> authenticationService)
    : gAPI.Core.Interfaces.IUseCase<TinderWithStats.Backend.Entities.ProfilePicture, TinderWithStats.Shared.Dtos.ProfilePicture, Guid>
{
    public async Task<bool> IsAllowedAsync(CancellationToken ct)
    {
        if (authenticationService.State.User == null) return false;
        return await db.UserRoles.AnyAsync(a => a.UserId == authenticationService.State.User.Id && a.Role!.Name == "Admin", ct);
    }
    public async Task<bool> CanListAsync(CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanCreateAsync(CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanCreateAsync(TinderWithStats.Shared.Dtos.ProfilePicture dto, CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanReadAsync(TinderWithStats.Shared.Dtos.ProfilePicture dto, CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanUpdateAsync(TinderWithStats.Shared.Dtos.ProfilePicture dto, CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanDeleteAsync(TinderWithStats.Shared.Dtos.ProfilePicture dto, CancellationToken ct) => authenticationService.State.User != null;

    public async Task<ProfilePicture?> FindByMatchAsync(TinderWithStats.Shared.Dtos.ProfilePicture dto, CancellationToken ct) 
        => null; // If you implement this, also use includes
    public async Task<ProfilePicture?> FindByIdAsync(Guid id, CancellationToken ct) 
        => await db.ProfilePictures
            .Include("Profile") // Add your filter query
            .FirstOrDefaultAsync(a => a.Id == id, ct);
    public IQueryable<ProfilePicture> ListAll()
        => db.ProfilePictures; // Add your filter query, no need for includes here

    public async Task<bool> AddAsync(ProfilePicture entityToAdd, CancellationToken ct) 
    {
        await db.ProfilePictures.AddAsync(entityToAdd, ct);
        await db.SaveChangesAsync(ct);
        return true;
    }
    public async Task<bool> UpdateAsync(ProfilePicture updatedEntity, TinderWithStats.Shared.Dtos.ProfilePicture dto, CancellationToken ct)
    {
        await db.SaveChangesAsync();
        return true;
    }
    public async Task<bool> RemoveAsync(ProfilePicture entity, CancellationToken ct)
    {
        db.ProfilePictures.Remove(entity);
        await db.SaveChangesAsync();
        return true;
    }
}
