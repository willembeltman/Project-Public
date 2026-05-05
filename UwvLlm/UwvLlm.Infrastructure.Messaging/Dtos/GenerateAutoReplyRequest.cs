using UwvLlm.Shared.Dtos;

namespace UwvLlm.Api.Core.Dtos;

public record GenerateAutoReplyRequest(MailMessage Email);
