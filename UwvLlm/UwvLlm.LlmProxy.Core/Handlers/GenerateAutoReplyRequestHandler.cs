using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using UwvLlm.Api.Core.Infrastructure.Data;
using UwvLlm.Shared.Private.Dtos;
using UwvLlm.Shared.Private.Interfaces;

namespace UwvLlm.Llm.Core.Handlers;

public class GenerateAutoReplyRequestHandler : IHandler
{
    private readonly IDbContextFactory<ApplicationDbContext> DbFactory;

    public GenerateAutoReplyRequestHandler(IDbContextFactory<ApplicationDbContext> dbFactory)
    {
        DbFactory = dbFactory;
    }

    public string MessageType => nameof(GenerateAutoReplyRequest);

    public async Task Handle(string messageJson)
    {
        var message = JsonSerializer.Deserialize<GenerateAutoReplyRequest>(messageJson)
            ?? throw new Exception("Cannot parse json");
        using var db = DbFactory.CreateDbContext();

    }
}
