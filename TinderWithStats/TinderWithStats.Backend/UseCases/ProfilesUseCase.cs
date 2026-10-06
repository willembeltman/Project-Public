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
        // Is user logged in?
        if (auth.State.User == null) return false;

        // Is admin?
        var isAdmin = await db.UserRoles.AnyAsync(a => a.UserId == auth.State.User.Id && a.Role!.Name == "Admin", ct);
        if (isAdmin) return true;

        // User already has a profile?
        var userProfile = db.Profiles.FirstOrDefault(p => p.UserId == auth.State.User.Id);
        if (userProfile != null) return false;

        // Ok, fine by me
        return true;
    }
    public async Task<bool> CanReadAsync(TinderWithStats.Shared.Dtos.Profile dto, CancellationToken ct)
    {
        // Is user logged in?
        if (auth.State.User == null) return false;

        // Is admin?
        var isAdmin = await db.UserRoles.AnyAsync(a => a.UserId == auth.State.User.Id && a.Role!.Name == "Admin", ct);
        if (isAdmin) return true;

        // User already has a profile?
        var userProfile = db.Profiles.FirstOrDefault(p => p.UserId == auth.State.User.Id);
        if (userProfile != null) return false;

        var currentProfileId = await db.Profiles
            .Where(p => p.UserId == auth.State.User.Id)
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
        // Logged in?
        if (auth.State.User == null) return false;

        // Is admin?
        var isAdmin = await db.UserRoles.AnyAsync(a => a.UserId == auth.State.User.Id && a.Role!.Name == "Admin", ct);
        if (isAdmin) return true;

        // Get db profile (yes I know, this needs to be better)
        var profile = db.Profiles.FirstOrDefault(p => p.Id == dto.Id);
        if (profile == null) return false;

        // Check if it our profile?
        if (profile.UserId == auth.State.User.Id) return true;

        // Else: no.
        return false;
    }
    public async Task<bool> CanDeleteAsync(TinderWithStats.Shared.Dtos.Profile dto, CancellationToken ct)
    {
        // Logged in?
        if (auth.State.User == null) return false;

        // Is admin?
        var isAdmin = await db.UserRoles.AnyAsync(a => a.UserId == auth.State.User.Id && a.Role!.Name == "Admin", ct);
        if (isAdmin) return true;

        // Get db profile (yes I know, this needs to be better)
        var profile = db.Profiles.FirstOrDefault(p => p.Id == dto.Id);
        if (profile == null) return false;

        // Check if it our profile?
        if (profile.UserId == auth.State.User.Id) return true;

        // Else: no.
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
        if (auth.State.User == null)
            return false;

        entityToAdd.UserId = auth.State.User.Id;

        await db.Profiles.AddAsync(entityToAdd, ct);
        try
        {
            await db.SaveChangesAsync(ct);
        }
        catch(Exception ex)
        {

        }
        return true;
    }
    public async Task<bool> UpdateAsync(Profile updatedEntity, TinderWithStats.Shared.Dtos.Profile dto, CancellationToken ct)
    {
        if (auth.State.User == null || updatedEntity.UserId != auth.State.User.Id)
            return false;

        await db.SaveChangesAsync();
        return true;
    }
    public async Task<bool> RemoveAsync(Profile entity, CancellationToken ct)
    {
        if (auth.State.User == null || entity.UserId != auth.State.User.Id)
            return false;

        db.Profiles.Remove(entity);
        await db.SaveChangesAsync();
        return true;
    }
}
