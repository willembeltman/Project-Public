using gAPI.Core.Server;
using Microsoft.EntityFrameworkCore;
using UwvLlm.Api.Core.Infrastructure.Data;

namespace UwvLlm.Api.Core.UseCases;

public class UserNotificationsUseCase(
    ApplicationDbContext db,
    IAuthenticationService<User, Shared.Dtos.State> authenticationService)
    : gAPI.Core.Interfaces.IUseCase<UserNotification, Shared.Dtos.UserNotification, long>
{
    public async Task<bool> IsAllowedAsync(CancellationToken ct) => true;
    public async Task<bool> CanListAsync(CancellationToken ct) => true;
    public async Task<bool> CanCreateAsync(CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanCreateAsync(Shared.Dtos.UserNotification dto, CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanReadAsync(Shared.Dtos.UserNotification dto, CancellationToken ct) => true;
    public async Task<bool> CanUpdateAsync(Shared.Dtos.UserNotification dto, CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanDeleteAsync(Shared.Dtos.UserNotification dto, CancellationToken ct) => authenticationService.State.User != null;

    public async Task<UserNotification?> FindByMatchAsync(Shared.Dtos.UserNotification dto, CancellationToken ct) 
        => null; // If you implement this, also use includes
    public async Task<UserNotification?> FindByIdAsync(long id, CancellationToken ct) 
        => await db.UserNotifications
            .Include("User") // Add your filter query
            .FirstOrDefaultAsync(a => a.Id == id, ct);
    public IQueryable<UserNotification> ListAll()
        => db.UserNotifications; // Add your filter query, no need for includes here

    public async Task<bool> AddAsync(UserNotification entityToAdd, CancellationToken ct) 
    {
        await db.UserNotifications.AddAsync(entityToAdd, ct);
        await db.SaveChangesAsync(ct);
        return true;
    }
    public async Task<bool> UpdateAsync(UserNotification updatedEntity, Shared.Dtos.UserNotification dto, CancellationToken ct)
    {
        await db.SaveChangesAsync();
        return true;
    }
    public async Task<bool> RemoveAsync(UserNotification entity, CancellationToken ct)
    {
        db.UserNotifications.Remove(entity);
        await db.SaveChangesAsync();
        return true;
    }
}
