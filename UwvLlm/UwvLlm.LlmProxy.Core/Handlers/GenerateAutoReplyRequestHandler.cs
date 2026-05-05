using Microsoft.EntityFrameworkCore;
using UwvLlm.Api.Core.Infrastructure.Data;
using UwvLlm.Shared.Private.Dtos;
using UwvLlm.Shared.Private.Interfaces;
using UwvLlm.Shared.Private.Services;

namespace UwvLlm.Llm.Core.Handlers;

public class GenerateAutoReplyRequestHandler(
    IDbContextFactory<ApplicationDbContext> dbFactory,
    ServiceBusSenderService sender)
    : IHandler<GenerateAutoReplyRequest>
{
    public async Task Handle(GenerateAutoReplyRequest message, CancellationToken ct)
    {
        using var db = dbFactory.CreateDbContext();

        // geen JSON parsing meer 🎉
        // gewoon typed werken

        //Console.WriteLine(message.Email);

        await sender.SendAsync(new GenerateAutoReplyResponse(), ct);
    }
}
