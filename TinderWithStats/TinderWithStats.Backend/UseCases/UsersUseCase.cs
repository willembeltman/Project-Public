using gAPI.Core.Server.Interfaces;
using Microsoft.EntityFrameworkCore;
using TinderWithStats.Backend.Entities;

namespace TinderWithStats.Backend.UseCases;

public class UsersUseCase(
    ApplicationDbContext db,
    IAuthenticationService<TinderWithStats.Backend.Entities.User, TinderWithStats.Shared.Dtos.State> auth)
    : gAPI.Core.Interfaces.IUseCase<TinderWithStats.Backend.Entities.User, TinderWithStats.Shared.Dtos.User, Guid>
{
    // Alleen ingelogd
    public async Task<bool> IsAllowedAsync(CancellationToken ct) => auth.State.User != null;
    // Alleen admin
    public async Task<bool> CanListAsync(CancellationToken ct)
    {
        if (auth.State.User == null) return false;
        return await db.UserRoles.AnyAsync(a => a.UserId == auth.State.User.Id && a.Role!.Name == "Admin", ct);
    }
    // Alleen admin
    public async Task<bool> CanCreateAsync(CancellationToken ct)
    {
        if (auth.State.User == null) return false;
        return await db.UserRoles.AnyAsync(a => a.UserId == auth.State.User.Id && a.Role!.Name == "Admin", ct);
    }
    // Alleen admin
    public async Task<bool> CanCreateAsync(TinderWithStats.Shared.Dtos.User dto, CancellationToken ct)
    {
        if (auth.State.User == null) return false;
        return await db.UserRoles.AnyAsync(a => a.UserId == auth.State.User.Id && a.Role!.Name == "Admin", ct);
    }
    // Alleen admin of user zelf
    public async Task<bool> CanReadAsync(TinderWithStats.Shared.Dtos.User dto, CancellationToken ct)
    {
        if (auth.State.User == null) return false;
        if (auth.State.User.Id == dto.Id) return true;
        return await db.UserRoles.AnyAsync(a => a.UserId == auth.State.User.Id && a.Role!.Name == "Admin", ct);
    }
    // Alleen admin of user zelf
    public async Task<bool> CanUpdateAsync(TinderWithStats.Shared.Dtos.User dto, CancellationToken ct)
    {
        if (auth.State.User == null) return false;
        if (auth.State.User.Id == dto.Id) return true;
        return await db.UserRoles.AnyAsync(a => a.UserId == auth.State.User.Id && a.Role!.Name == "Admin", ct);
    }
    // Alleen admin of user zelf
    public async Task<bool> CanDeleteAsync(TinderWithStats.Shared.Dtos.User dto, CancellationToken ct)
    {
        if (auth.State.User == null) return false;
        if (auth.State.User.Id == dto.Id) return true;
        return await db.UserRoles.AnyAsync(a => a.UserId == auth.State.User.Id && a.Role!.Name == "Admin", ct);
    }

    public async Task<User?> FindByMatchAsync(TinderWithStats.Shared.Dtos.User dto, CancellationToken ct)
        => await db.Users 
            .FirstOrDefaultAsync(a =>
                a.Email == dto.Email, ct);
    public async Task<User?> FindByIdAsync(Guid id, CancellationToken ct)
    {
        if (auth.State.User == null) return null;
        var isAdmin = await db.UserRoles.AnyAsync(a => a.UserId == auth.State.User.Id && a.Role!.Name == "Admin", ct);
        return await db.Users
            .FirstOrDefaultAsync(a => 
                a.Id == id &&
                (isAdmin || a.Id == auth.State.User.Id)
            , ct);
    }
    public IQueryable<User> ListAll()
    {
        if (auth.State.User == null) return null;
        var isAdmin = db.UserRoles.Any(a => a.UserId == auth.State.User.Id && a.Role!.Name == "Admin");
        return db.Users
            .Where(a => (isAdmin || a.Id == auth.State.User.Id));
    }

    public async Task<bool> AddAsync(User entityToAdd, CancellationToken ct)
    {
        await db.Users.AddAsync(entityToAdd, ct);
        await db.SaveChangesAsync(ct);
        return true;
    }
    public async Task<bool> UpdateAsync(User updatedEntity, TinderWithStats.Shared.Dtos.User dto, CancellationToken ct)
    {
        await db.SaveChangesAsync();
        return true;
    }
    public async Task<bool> RemoveAsync(User entity, CancellationToken ct)
    {
        db.Users.Remove(entity);
        await db.SaveChangesAsync();
        return true;
    }
}
