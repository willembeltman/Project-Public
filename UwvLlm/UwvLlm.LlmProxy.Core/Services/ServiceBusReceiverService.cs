using Microsoft.EntityFrameworkCore;
using UwvLlm.Api.Core.Infrastructure.Data;
using UwvLlm.Llm.Core.Handlers;
using UwvLlm.Shared.Private.Interfaces;
using UwvLlm.Shared.Private.Services;

namespace UwvLlm.Llm.Core.Services;

public class ServiceBusReceiverService(
    IDbContextFactory<ApplicationDbContext> dbFactory) : BaseServiceBusReceiverService
{
    protected override IHandler[] Handlers { get; } =
    [
        new GenerateAutoReplyRequestHandler(dbFactory)
    ];
}
