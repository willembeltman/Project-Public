using gAPI.Core.Server.Interfaces;
using Microsoft.EntityFrameworkCore;
using TinderWithStats.Backend.Entities;

namespace TinderWithStats.Backend.UseCases;

public class ProfilesUseCase(
    ApplicationDbContext db,
    IAuthenticationService<TinderWithStats.Backend.Entities.User, TinderWithStats.Shared.Dtos.State> auth)
    : gAPI.Core.Interfaces.IUseCase<TinderWithStats.Backend.Entities.Profile, TinderWithStats.Shared.Dtos.Profile, Guid>
{
    public async Task<bool> IsAllowedAsync(CancellationToken ct) => auth.State.User != null;
    public async Task<bool> CanListAsync(CancellationToken ct) => auth.State.User != null;
    public async Task<bool> CanCreateAsync(CancellationToken ct) => auth.State.User != null;
    public async Task<bool> CanCreateAsync(TinderWithStats.Shared.Dtos.Profile dto, CancellationToken ct)
    {
        if (auth.AuthenticationState.User == null) return false;
        var isAdmin = await db.UserRoles.AnyAsync(a => a.UserId == auth.AuthenticationState.User.Id && a.Role!.Name == "Admin", ct);
        if (isAdmin) return true;
        if (dto.UserId == auth.AuthenticationState.User.Id) return true;
        return false;
    }
    public async Task<bool> CanReadAsync(TinderWithStats.Shared.Dtos.Profile dto, CancellationToken ct)
    {
        if (auth.State.User == null) return false;
        var currentUserId = auth.State.User.Id;

        var isAdmin = await db.UserRoles.AnyAsync(a => a.UserId == currentUserId && a.Role!.Name == "Admin", ct);
        if (isAdmin) return true;

        if (dto.UserId == currentUserId) return true;

        var currentProfileId = await db.Profiles
            .Where(p => p.UserId == currentUserId)
            .Select(p => p.Id)
            .FirstOrDefaultAsync(ct);

        if (currentProfileId == Guid.Empty) return false;

        return await db.Matches
            .AnyAsync(a =>
                a.MatchAccepted != null &&
                a.MatchRemoved == null &&
                (
                    (a.ProfileSenderId == currentProfileId && a.ProfileReceiverId == dto.Id) ||
                    (a.ProfileReceiverId == currentProfileId && a.ProfileSenderId == dto.Id)
                ), ct);
    }

    public async Task<bool> CanUpdateAsync(TinderWithStats.Shared.Dtos.Profile dto, CancellationToken ct)
    {
        if (auth.State.User == null) return false;

        var isAdmin = await db.UserRoles.AnyAsync(a => a.UserId == auth.State.User.Id && a.Role!.Name == "Admin", ct);
        if (isAdmin) return true;

        if (dto.UserId == auth.State.User.Id) return true;
        return false;
    }
    public async Task<bool> CanDeleteAsync(TinderWithStats.Shared.Dtos.Profile dto, CancellationToken ct)
    {
        if (auth.State.User == null) return false;

        var isAdmin = await db.UserRoles.AnyAsync(a => a.UserId == auth.State.User.Id && a.Role!.Name == "Admin", ct);
        if (isAdmin) return true;

        if (dto.UserId == auth.State.User.Id) return true;
        return false;
    }

    public async Task<Profile?> FindByMatchAsync(TinderWithStats.Shared.Dtos.Profile dto, CancellationToken ct)
        => null;
    public async Task<Profile?> FindByIdAsync(Guid id, CancellationToken ct)
    {
        if (auth.State.User == null) return null;
        var currentUserId = auth.State.User.Id;

        var isAdmin = await db.UserRoles.AnyAsync(a => a.UserId == currentUserId && a.Role!.Name == "Admin", ct);

        return await db.Profiles
            .Include(p => p.User)
            .Include(p => p.Location)
            .Where(p =>
                isAdmin ||
                p.UserId == currentUserId ||
                p.MatchesReceived!.Any(m => m.ProfileSender!.UserId == currentUserId && m.MatchAccepted != null && m.MatchRemoved == null) ||
                p.MatchesSend!.Any(m => m.ProfileReceiver!.UserId == currentUserId && m.MatchAccepted != null && m.MatchRemoved == null)
            )
            .FirstOrDefaultAsync(a => a.Id == id, ct);
    }


    public IQueryable<Profile> ListAll()
    {
        if (auth.State.User == null)
            return Enumerable.Empty<Profile>().AsQueryable();
        var currentUserId = auth.State.User.Id;

        var isAdmin = db.UserRoles.Any(a => a.UserId == currentUserId && a.Role!.Name == "Admin");

        return db.Profiles
            .Include(p => p.User)
            .Include(p => p.Location)
            .Where(p =>
                isAdmin ||
                p.UserId == currentUserId || 
                p.MatchesReceived!.Any(m => m.ProfileSender!.UserId == currentUserId && m.MatchAccepted != null && m.MatchRemoved == null) ||
                p.MatchesSend!.Any(m => m.ProfileReceiver!.UserId == currentUserId && m.MatchAccepted != null && m.MatchRemoved == null)
            );
    }

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
