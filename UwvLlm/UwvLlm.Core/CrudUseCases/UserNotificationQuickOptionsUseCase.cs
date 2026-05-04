using gAPI.Core.Server;
using Microsoft.EntityFrameworkCore;
using UwvLlm.Core.Infrastructure.Data;

namespace UwvLlm.Core.CrudUseCases;

public class UserNotificationQuickOptionsUseCase(
    ApplicationDbContext db,
    IAuthenticationService<User, Shared.Dtos.State> authenticationService)
    : gAPI.Interfaces.IUseCase<UserNotificationQuickOption, Shared.Dtos.UserNotificationQuickOption, long>
{
    public async Task<bool> IsAllowedAsync(CancellationToken ct) => true;
    public async Task<bool> CanListAsync(CancellationToken ct) => true;
    public async Task<bool> CanCreateAsync(CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanCreateAsync(Shared.Dtos.UserNotificationQuickOption dto, CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanReadAsync(Shared.Dtos.UserNotificationQuickOption dto, CancellationToken ct) => true;
    public async Task<bool> CanUpdateAsync(Shared.Dtos.UserNotificationQuickOption dto, CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanDeleteAsync(Shared.Dtos.UserNotificationQuickOption dto, CancellationToken ct) => authenticationService.State.User != null;

    public async Task<UserNotificationQuickOption?> FindByMatchAsync(Shared.Dtos.UserNotificationQuickOption dto, CancellationToken ct) 
        => null; // If you implement this, also use includes
    public async Task<UserNotificationQuickOption?> FindByIdAsync(long id, CancellationToken ct) 
        => await db.UserNotificationQuickOptions
            .Include("UserNotification") // Add your filter query
            .FirstOrDefaultAsync(a => a.Id == id, ct);
    public IQueryable<UserNotificationQuickOption> ListAll()
        => db.UserNotificationQuickOptions; // Add your filter query, no need for includes here

    public async Task<bool> AddAsync(UserNotificationQuickOption entityToAdd, CancellationToken ct) 
    {
        await db.UserNotificationQuickOptions.AddAsync(entityToAdd, ct);
        await db.SaveChangesAsync(ct);
        return true;
    }
    public async Task<bool> UpdateAsync(UserNotificationQuickOption updatedEntity, Shared.Dtos.UserNotificationQuickOption dto, CancellationToken ct)
    {
        await db.SaveChangesAsync();
        return true;
    }
    public async Task<bool> RemoveAsync(UserNotificationQuickOption entity, CancellationToken ct)
    {
        db.UserNotificationQuickOptions.Remove(entity);
        await db.SaveChangesAsync();
        return true;
    }
}
