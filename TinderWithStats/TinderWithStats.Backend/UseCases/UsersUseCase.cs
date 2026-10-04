using gAPI.Core.Server.Interfaces;
using Microsoft.EntityFrameworkCore;
using TinderWithStats.Backend.Entities;

namespace TinderWithStats.Backend.UseCases;

public class UsersUseCase(
    ApplicationDbContext db,
    IAuthenticationService<TinderWithStats.Backend.Entities.User, TinderWithStats.Shared.Dtos.State> authenticationService)
    : gAPI.Core.Interfaces.IUseCase<TinderWithStats.Backend.Entities.User, TinderWithStats.Shared.Dtos.User, Guid>
{
    public async Task<bool> IsAllowedAsync(CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanListAsync(CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanCreateAsync(CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanCreateAsync(TinderWithStats.Shared.Dtos.User dto, CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanReadAsync(TinderWithStats.Shared.Dtos.User dto, CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanUpdateAsync(TinderWithStats.Shared.Dtos.User dto, CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanDeleteAsync(TinderWithStats.Shared.Dtos.User dto, CancellationToken ct) => authenticationService.State.User != null;

    public async Task<User?> FindByMatchAsync(TinderWithStats.Shared.Dtos.User dto, CancellationToken ct) 
        => await db.Users // Add your filter query
            .FirstOrDefaultAsync(a => 
                a.UserName == dto.UserName &&
                a.Email == dto.Email, ct);
    public async Task<User?> FindByIdAsync(Guid id, CancellationToken ct) 
        => await db.Users // Add your filter query
            .FirstOrDefaultAsync(a => a.Id == id, ct);
    public IQueryable<User> ListAll()
        => db.Users; // Add your filter query, no need for includes here

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
