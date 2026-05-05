using gAPI.Core.Server;
using UwvLlm.Api.Core.Dtos;
using UwvLlm.Api.Core.Enums;
using UwvLlm.Shared.CrudInterfaces;
using UwvLlm.Shared.Dtos;
using UwvLlm.Shared.Interfaces;

namespace UwvLlm.Api.Core.Services;

public class MailApi(
    IAuthenticationService<Infrastructure.Data.Entities.User, State> authenticationService,
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
