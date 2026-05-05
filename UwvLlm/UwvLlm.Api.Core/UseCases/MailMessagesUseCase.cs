using gAPI.Core.Server;
using Microsoft.EntityFrameworkCore;
using UwvLlm.Api.Core.Infrastructure.Data;

namespace UwvLlm.Api.Core.UseCases;

public class MailMessagesUseCase(
    ApplicationDbContext db,
    IAuthenticationService<User, Shared.Public.Dtos.State> authenticationService)
    : gAPI.Core.Interfaces.IUseCase<MailMessage, Shared.Public.Dtos.MailMessage, Guid>
{
    public async Task<bool> IsAllowedAsync(CancellationToken ct) => true;
    public async Task<bool> CanListAsync(CancellationToken ct) => true;
    public async Task<bool> CanCreateAsync(CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanCreateAsync(Shared.Public.Dtos.MailMessage dto, CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanReadAsync(Shared.Public.Dtos.MailMessage dto, CancellationToken ct) => true;
    public async Task<bool> CanUpdateAsync(Shared.Public.Dtos.MailMessage dto, CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanDeleteAsync(Shared.Public.Dtos.MailMessage dto, CancellationToken ct) => authenticationService.State.User != null;

    public async Task<MailMessage?> FindByMatchAsync(Shared.Public.Dtos.MailMessage dto, CancellationToken ct) 
        => null; // If you implement this, also use includes
    public async Task<MailMessage?> FindByIdAsync(Guid id, CancellationToken ct) 
        => await db.MailMessages // Add your filter query
            .FirstOrDefaultAsync(a => a.Id == id, ct);
    public IQueryable<MailMessage> ListAll()
        => db.MailMessages; // Add your filter query, no need for includes here

    public async Task<bool> AddAsync(MailMessage entityToAdd, CancellationToken ct) 
    {
        await db.MailMessages.AddAsync(entityToAdd, ct);
        await db.SaveChangesAsync(ct);
        return true;
    }
    public async Task<bool> UpdateAsync(MailMessage updatedEntity, Shared.Public.Dtos.MailMessage dto, CancellationToken ct)
    {
        await db.SaveChangesAsync();
        return true;
    }
    public async Task<bool> RemoveAsync(MailMessage entity, CancellationToken ct)
    {
        db.MailMessages.Remove(entity);
        await db.SaveChangesAsync();
        return true;
    }
}
