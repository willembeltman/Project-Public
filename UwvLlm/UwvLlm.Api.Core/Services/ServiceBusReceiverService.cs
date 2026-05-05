using Microsoft.EntityFrameworkCore;
using UwvLlm.Api.Core.Handlers;
using UwvLlm.Api.Core.Infrastructure.Data;
using UwvLlm.Shared.Private.Interfaces;
using UwvLlm.Shared.Private.Services;

namespace UwvLlm.Api.Core.Services;

public class ServiceBusReceiverService(
    IDbContextFactory<ApplicationDbContext> dbFactory) : BaseServiceBusReceiverService
{
    protected override IHandler[] Handlers { get; } =
    [
        new GenerateAutoReplyResponseHandler(dbFactory)
    ];
}
