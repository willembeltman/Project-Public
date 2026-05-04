using gAPI.Core.Server;
using Microsoft.EntityFrameworkCore;
using UwvLlm.Core.Infrastructure.Data;

namespace UwvLlm.Core.CrudUseCases;

public class UsersUseCase(
    ApplicationDbContext db,
    IAuthenticationService<UwvLlm.Core.Infrastructure.Data.User, UwvLlm.Shared.Dtos.State> authenticationService)
    : gAPI.Interfaces.IUseCase<UwvLlm.Core.Infrastructure.Data.User, UwvLlm.Shared.Dtos.User, Guid>
{
    public async Task<bool> IsAllowedAsync(CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanListAsync(CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanCreateAsync(CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanCreateAsync(UwvLlm.Shared.Dtos.User dto, CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanReadAsync(UwvLlm.Shared.Dtos.User dto, CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanUpdateAsync(UwvLlm.Shared.Dtos.User dto, CancellationToken ct) => authenticationService.State.User != null;
    public async Task<bool> CanDeleteAsync(UwvLlm.Shared.Dtos.User dto, CancellationToken ct) => authenticationService.State.User != null;

    public async Task<User?> FindByMatchAsync(UwvLlm.Shared.Dtos.User dto, CancellationToken ct) 
        => await db.Users // Add your filter query
            .FirstOrDefaultAsync(a => 
                a.UserName == dto.UserName, ct);
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
    public async Task<bool> UpdateAsync(User updatedEntity, UwvLlm.Shared.Dtos.User dto, CancellationToken ct)
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
