using UwvLlm.Shared.Dtos;

namespace UwvLlm.Api.Core.Dtos;

public record GenerateAutoReplyResponse(
    MailMessage Email);