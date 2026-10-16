using gAPI.Core.Server.Interfaces;
using Microsoft.EntityFrameworkCore;
using Rekenkamer.Infrastructure.Data.Entities;

namespace Rekenkamer.Infrastructure.Data.UseCases;

public class UsersUseCase(
    ApplicationDbContext db,
    IAuthenticationService<Rekenkamer.Infrastructure.Data.Entities.User, Rekenkamer.Shared.Dtos.State> auth)
    : gAPI.Core.Interfaces.IUseCase<Rekenkamer.Infrastructure.Data.Entities.User, Rekenkamer.Shared.Dtos.User, Guid>
{
    public async Task<bool> IsAllowedAsync(CancellationToken ct) => auth.State.User != null;
    public async Task<bool> CanListAsync(CancellationToken ct) => auth.State.User != null;
    public async Task<bool> CanCreateAsync(CancellationToken ct) => false;
    public async Task<bool> CanCreateAsync(Rekenkamer.Shared.Dtos.User dto, CancellationToken ct) => auth.State.User != null;
    public async Task<bool> CanReadAsync(Rekenkamer.Shared.Dtos.User dto, CancellationToken ct)
        => auth.State.User != null && auth.State.User.Id == dto.Id;
    public async Task<bool> CanUpdateAsync(Rekenkamer.Shared.Dtos.User dto, CancellationToken ct)
        => auth.State.User != null && auth.State.User.Id == dto.Id;
    public async Task<bool> CanDeleteAsync(Rekenkamer.Shared.Dtos.User dto, CancellationToken ct)
        => auth.State.User != null && auth.State.User.Id == dto.Id;

    public async Task<User?> FindByMatchAsync(Rekenkamer.Shared.Dtos.User dto, CancellationToken ct)
        => await db.Users
            .FirstOrDefaultAsync(a =>
                a.UserName == dto.UserName &&
                a.Email == dto.Email, ct);

    public async Task<User?> FindByIdAsync(Guid id, CancellationToken ct)
        => await db.Users
            .FirstOrDefaultAsync(a => a.Id == id, ct);

    public IQueryable<User> ListAll()
        => db.Users;

    public async Task<bool> AddAsync(User entityToAdd, CancellationToken ct)
    {
        await db.Users.AddAsync(entityToAdd, ct);
        await db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> UpdateAsync(User updatedEntity, Rekenkamer.Shared.Dtos.User dto, CancellationToken ct)
    {
        await db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> RemoveAsync(User entity, CancellationToken ct)
    {
        db.Users.Remove(entity);
        await db.SaveChangesAsync(ct);
        return true;
    }
}
