using UwvLlm.Core.Interfaces;
using UwvLlm.Shared.Dtos;

namespace UwvLlm.Core.Services;

public class MailService(
    gAPI.Interfaces.IUseCase<UwvLlm.Core.Infrastructure.Data.User, User, Guid> userUseCase,
    gAPI.Interfaces.IUseCase<UwvLlm.Core.Infrastructure.Data.MailMessage, MailMessage, Guid> useCase,
    gAPI.Interfaces.Mapping<UwvLlm.Core.Infrastructure.Data.MailMessage, MailMessage> mapping)
    : IMailService
{
    public async Task<MailMessage> ReceivedEmail(NewMailMessage newMail, CancellationToken ct)
    {
        // Find emails
        string[] addresses = [newMail.From.ToLower(), .. newMail.To.Select(a => a.ToLower())];
        addresses = [.. addresses.Distinct()];

        var entities = userUseCase
            .ListAll()
            .Where(a => addresses.Contains(a.Email.ToLower()))
            .ToArray();

        if (entities.Length != addresses.Length)
            throw new Exception("Some email adresses are not found");

        // Create mail
        var dbMail = new UwvLlm.Core.Infrastructure.Data.MailMessage
        {
            Subject = newMail.Subject,
            Body = newMail.Body,
            FromUser = entities.First(a => a.Email.ToLower() == newMail.From.ToLower())
        };
        foreach (var user in entities.Where(a => newMail.To.Contains(a.Email.ToLower())))
        {
            dbMail.ToUsers.Add(new UwvLlm.Core.Infrastructure.Data.MailMessageToUser()
            {
                User = user
            });
        }

        // Add mail
        await useCase.AddAsync(dbMail, ct);
        var dto = await mapping.ToDtoAsync(dbMail, new MailMessage(), ct);
        return dto;
    }
}
