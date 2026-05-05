using gAPI.Core.Server;
using UwvLlm.Shared.Private.Dtos;
using UwvLlm.Shared.Private.Enums;
using UwvLlm.Shared.Private.Services;
using UwvLlm.Shared.Public.Dtos;
using UwvLlm.Shared.Public.Interfaces;

namespace UwvLlm.Api.Core.Services;

public class MailApi(
    IAuthenticationService<Infrastructure.Data.User, State> authenticationService,
    IMailMessageCrudService mailService,
    ServiceBusSender serviceBusSender)
    : IMailApi
{
    public async Task SendMail(MailMessage newMail, CancellationToken ct)
    {
        if (authenticationService.State.User == null)
            return;

        newMail.FromUserId = authenticationService.State.User.Id;
        var mailResponse = await mailService.Create(newMail, ct);
        if (mailResponse.Success == false || mailResponse.Response == null) 
            return;

        var autoReplyMessage = new GenerateAutoReplyRequest(
            mailResponse.Response);

        await serviceBusSender.SendAsync(Bus.LlmProxy, autoReplyMessage, ct);
    }
}
