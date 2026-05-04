using gAPI.Core.Server;
using Microsoft.EntityFrameworkCore;
using UwvLlm.Core.Infrastructure.Data;

namespace UwvLlm.Core.CrudUseCases;

public class MailMessageToUsersUseCase(
    ApplicationDbContext db,
    IAuthenticationService<UwvLlm.Core.Infrastructure.Data.User, UwvLlm.Shared.Dtos.State> authenticationService)
    : gAPI.Interfaces.IUseCase<UwvLlm.Core.Infrastructure.Data.MailMessageToUser, UwvLlm.Shared.Dtos.MailMessageToUser, long>
{
    public async Task<bool> IsAllowedAsync(CancellationToken ct) => true;
    public async Task<bool> CanListAsync(CancellationToken ct) => true;
    public async Task<bool> CanCreateAsync(CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanCreateAsync(UwvLlm.Shared.Dtos.MailMessageToUser dto, CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanReadAsync(UwvLlm.Shared.Dtos.MailMessageToUser dto, CancellationToken ct) => true;
    public async Task<bool> CanUpdateAsync(UwvLlm.Shared.Dtos.MailMessageToUser dto, CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanDeleteAsync(UwvLlm.Shared.Dtos.MailMessageToUser dto, CancellationToken ct) => authenticationService.State.User != null;

    public async Task<MailMessageToUser?> FindByMatchAsync(UwvLlm.Shared.Dtos.MailMessageToUser dto, CancellationToken ct) 
        => null; // If you implement this, also use includes
    public async Task<MailMessageToUser?> FindByIdAsync(long id, CancellationToken ct) 
        => await db.MailMessageToUsers
            .Include("MailMessage")
            .Include("User") // Add your filter query
            .FirstOrDefaultAsync(a => a.Id == id, ct);
    public IQueryable<MailMessageToUser> ListAll()
        => db.MailMessageToUsers; // Add your filter query, no need for includes here

    public async Task<bool> AddAsync(MailMessageToUser entityToAdd, CancellationToken ct) 
    {
        await db.MailMessageToUsers.AddAsync(entityToAdd, ct);
        await db.SaveChangesAsync(ct);
        return true;
    }
    public async Task<bool> UpdateAsync(MailMessageToUser updatedEntity, UwvLlm.Shared.Dtos.MailMessageToUser dto, CancellationToken ct)
    {
        await db.SaveChangesAsync();
        return true;
    }
    public async Task<bool> RemoveAsync(MailMessageToUser entity, CancellationToken ct)
    {
        db.MailMessageToUsers.Remove(entity);
        await db.SaveChangesAsync();
        return true;
    }
}
