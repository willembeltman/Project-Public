using gAPI.Attributes;
using gAPI.Dtos;
using UwvLlm.Shared.Dtos;

namespace UwvLlm.Shared.Interfaces;

[GenerateApi]
public interface IUserNotificationQuickOptionsService
{
    [IsCreate]
    Task<BaseResponseT<UserNotificationQuickOption>> Create(UserNotificationQuickOption usernotificationquickoption, CancellationToken ct);

    [IsRead]
    Task<BaseResponseT<UserNotificationQuickOption>> Read(long usernotificationquickoptionId, CancellationToken ct);

    [IsUpdate]
    Task<BaseResponseT<UserNotificationQuickOption>> Update(UserNotificationQuickOption usernotificationquickoption, CancellationToken ct);

    [IsDelete(typeof(UserNotificationQuickOption))]
    Task<BaseResponseT<bool>> Delete(long usernotificationquickoptionId, CancellationToken ct);

    [IsList]
    Task<BaseListResponseT<UserNotificationQuickOption>> List(int? skip, int? take, string[]? orderby, CancellationToken ct);

    [IsListBy(nameof(UserNotificationQuickOption.UserNotificationId), typeof(UserNotification))]
    Task<BaseListResponseT<UserNotificationQuickOption>> ListByUserNotificationId(long UserNotificationId, int? skip, int? take, string[]? orderby, CancellationToken ct);

    [IsListNotBy(nameof(UserNotificationQuickOption.UserNotificationId), typeof(UserNotification))]
    Task<BaseListResponseT<UserNotificationQuickOption>> ListNotByUserNotificationId(long UserNotificationId, int? skip, int? take, string[]? orderby, CancellationToken ct);
}