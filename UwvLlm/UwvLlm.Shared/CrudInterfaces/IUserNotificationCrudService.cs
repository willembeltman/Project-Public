using gAPI.Core.Attributes;
using gAPI.Core.Dtos;
using UwvLlm.Shared.Dtos;

namespace UwvLlm.Shared.CrudInterfaces;

[GenerateApi]
[IsAuthorized]
public interface IUserNotificationCrudService
{
    [IsCreate]
    Task<BaseResponseT<UserNotification>> Create(UserNotification usernotification, CancellationToken ct);

    [IsRead]
    Task<BaseResponseT<UserNotification>> Read(long usernotificationId, CancellationToken ct);

    [IsUpdate]
    Task<BaseResponseT<UserNotification>> Update(UserNotification usernotification, CancellationToken ct);

    [IsDelete(typeof(UserNotification))]
    Task<BaseResponseT<bool>> Delete(long usernotificationId, CancellationToken ct);

    [IsList]
    Task<BaseListResponseT<UserNotification>> List(int? skip, int? take, string[]? orderby, CancellationToken ct);

    [IsListBy(nameof(UserNotification.UserId), typeof(User))]
    Task<BaseListResponseT<UserNotification>> ListByUserId(Guid UserId, int? skip, int? take, string[]? orderby, CancellationToken ct);

    [IsListNotBy(nameof(UserNotification.UserId), typeof(User))]
    Task<BaseListResponseT<UserNotification>> ListNotByUserId(Guid UserId, int? skip, int? take, string[]? orderby, CancellationToken ct);
}