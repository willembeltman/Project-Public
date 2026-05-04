using gAPI.Core.Server;
using Microsoft.EntityFrameworkCore;
using UwvLlm.Core.Infrastructure.Data;

namespace UwvLlm.Core.CrudUseCases;

public class MailMessagesUseCase(
    ApplicationDbContext db,
    IAuthenticationService<UwvLlm.Core.Infrastructure.Data.User, UwvLlm.Shared.Dtos.State> authenticationService)
    : gAPI.Interfaces.IUseCase<UwvLlm.Core.Infrastructure.Data.MailMessage, UwvLlm.Shared.Dtos.MailMessage, Guid>
{
    public async Task<bool> IsAllowedAsync(CancellationToken ct) => true;
    public async Task<bool> CanListAsync(CancellationToken ct) => true;
    public async Task<bool> CanCreateAsync(CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanCreateAsync(UwvLlm.Shared.Dtos.MailMessage dto, CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanReadAsync(UwvLlm.Shared.Dtos.MailMessage dto, CancellationToken ct) => true;
    public async Task<bool> CanUpdateAsync(UwvLlm.Shared.Dtos.MailMessage dto, CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanDeleteAsync(UwvLlm.Shared.Dtos.MailMessage dto, CancellationToken ct) => authenticationService.State.User != null;

    public async Task<MailMessage?> FindByMatchAsync(UwvLlm.Shared.Dtos.MailMessage dto, CancellationToken ct) 
        => null; // If you implement this, also use includes
    public async Task<MailMessage?> FindByIdAsync(Guid id, CancellationToken ct) 
        => await db.EmailMessages // Add your filter query
            .FirstOrDefaultAsync(a => a.Id == id, ct);
    public IQueryable<MailMessage> ListAll()
        => db.EmailMessages; // Add your filter query, no need for includes here

    public async Task<bool> AddAsync(MailMessage entityToAdd, CancellationToken ct) 
    {
        await db.EmailMessages.AddAsync(entityToAdd, ct);
        await db.SaveChangesAsync(ct);
        return true;
    }
    public async Task<bool> UpdateAsync(MailMessage updatedEntity, UwvLlm.Shared.Dtos.MailMessage dto, CancellationToken ct)
    {
        await db.SaveChangesAsync();
        return true;
    }
    public async Task<bool> RemoveAsync(MailMessage entity, CancellationToken ct)
    {
        db.EmailMessages.Remove(entity);
        await db.SaveChangesAsync();
        return true;
    }
}
