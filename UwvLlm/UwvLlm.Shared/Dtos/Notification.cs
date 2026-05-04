using UwvLlm.Shared.Enums;

namespace UwvLlm.Shared.Dtos;

public record Notification(
    NotificationType Type,
    string Id,
    string Title,
    string Message,
    string[]? QuickOptions = null);