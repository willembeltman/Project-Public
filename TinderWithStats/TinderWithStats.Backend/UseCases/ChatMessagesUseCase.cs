using gAPI.Core.Server.Interfaces;
using Microsoft.EntityFrameworkCore;
using TinderWithStats.Backend.Entities;

namespace TinderWithStats.Backend.UseCases;

public class ChatMessagesUseCase(
    ApplicationDbContext db,
    IAuthenticationService<TinderWithStats.Backend.Entities.User, TinderWithStats.Shared.Dtos.State> authenticationService)
    : gAPI.Core.Interfaces.IUseCase<TinderWithStats.Backend.Entities.ChatMessage, TinderWithStats.Shared.Dtos.ChatMessage, Guid>
{
    public async Task<bool> IsAllowedAsync(CancellationToken ct) => true;
    public async Task<bool> CanListAsync(CancellationToken ct) => true;
    public async Task<bool> CanCreateAsync(CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanCreateAsync(TinderWithStats.Shared.Dtos.ChatMessage dto, CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanReadAsync(TinderWithStats.Shared.Dtos.ChatMessage dto, CancellationToken ct) => true;
    public async Task<bool> CanUpdateAsync(TinderWithStats.Shared.Dtos.ChatMessage dto, CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanDeleteAsync(TinderWithStats.Shared.Dtos.ChatMessage dto, CancellationToken ct) => authenticationService.State.User != null;

    public async Task<ChatMessage?> FindByMatchAsync(TinderWithStats.Shared.Dtos.ChatMessage dto, CancellationToken ct) 
        => await db.ChatMessages
            .Include("Match")
            .Include("ProfileSender")
            .Include("ProfileReceiver") // Add your filter query
            .FirstOrDefaultAsync(a => 
                a.Date == dto.Date &&
                a.Message == dto.Message, ct);
    public async Task<ChatMessage?> FindByIdAsync(Guid id, CancellationToken ct) 
        => await db.ChatMessages
            .Include("Match")
            .Include("ProfileSender")
            .Include("ProfileReceiver") // Add your filter query
            .FirstOrDefaultAsync(a => a.Id == id, ct);
    public IQueryable<ChatMessage> ListAll()
        => db.ChatMessages; // Add your filter query, no need for includes here

    public async Task<bool> AddAsync(ChatMessage entityToAdd, CancellationToken ct) 
    {
        await db.ChatMessages.AddAsync(entityToAdd, ct);
        await db.SaveChangesAsync(ct);
        return true;
    }
    public async Task<bool> UpdateAsync(ChatMessage updatedEntity, TinderWithStats.Shared.Dtos.ChatMessage dto, CancellationToken ct)
    {
        await db.SaveChangesAsync();
        return true;
    }
    public async Task<bool> RemoveAsync(ChatMessage entity, CancellationToken ct)
    {
        db.ChatMessages.Remove(entity);
        await db.SaveChangesAsync();
        return true;
    }
}
