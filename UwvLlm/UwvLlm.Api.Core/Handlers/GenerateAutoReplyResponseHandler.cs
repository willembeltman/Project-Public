using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using UwvLlm.Api.Core.Infrastructure.Data;
using UwvLlm.Shared.Private.Dtos;
using UwvLlm.Shared.Private.Interfaces;

namespace UwvLlm.Api.Core.Handlers;

public class GenerateAutoReplyResponseHandler : IHandler
{
    private readonly IDbContextFactory<ApplicationDbContext> DbFactory;

    public GenerateAutoReplyResponseHandler(IDbContextFactory<ApplicationDbContext> dbFactory)
    {
        DbFactory = dbFactory;
    }

    public string MessageType => nameof(GenerateAutoReplyResponse);
    public async Task Handle(string messageJson)
    {
        var message = JsonSerializer.Deserialize<GenerateAutoReplyResponse>(messageJson)
            ?? throw new Exception("Cannot parse json");
        using var db = DbFactory.CreateDbContext();
    }
}
