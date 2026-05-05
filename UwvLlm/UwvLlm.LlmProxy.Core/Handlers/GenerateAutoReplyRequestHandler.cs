using Microsoft.EntityFrameworkCore;
using UwvLlm.Api.Core.Infrastructure.Llm.Interfaces;
using UwvLlm.Api.Core.Infrastructure.Llm.Models;
using UwvLlm.Infrastructure.Llm.Enums;
using UwvLlm.Api.Core.Dtos;
using UwvLlm.Api.Core.Enums;
using UwvLlm.Infrastructure.Data.Entities;
using UwvLlm.Infrastructure.Messaging.Interfaces;

namespace UwvLlm.LlmProxy.Core.Handlers;

public class GenerateAutoReplyRequestHandler(
    IDbContextFactory<ApplicationDbContext> dbFactory,
    IServiceBusSender sender,
    ILlmClient llmClient)
    : IHandler<GenerateAutoReplyRequest>
{
    public async Task Handle(GenerateAutoReplyRequest message, CancellationToken ct)
    {
        using var db = dbFactory.CreateDbContext();
        var dbMailMessage = db.MailMessages.FirstOrDefault(a => a.Id == message.Email.Id);
        if (dbMailMessage == null || dbMailMessage.AutoResponse != null)
            return;

        // Hardcoded for now
        var model = new Model("gpt-oss:20b");
        if (llmClient.Initialized == false)
        {
            await llmClient.InitializeModelAsync(model, ct);
        }

        var systemPrompt = "Create a reply to this email conversation, use the same language as the user uses.";
        var mailMessageText = $@"Date: {dbMailMessage.Date}
From: {dbMailMessage.FromUser.UserName} ({dbMailMessage.FromUser.Email})
To: {dbMailMessage.ToUser.UserName} ({dbMailMessage.ToUser.Email})
Subject: {dbMailMessage.Subject}

{dbMailMessage.Content}";
        var messages = new List<Message>()
        {
            new Message(Role.System, null, systemPrompt, null, null),
            new Message(Role.User, null, mailMessageText, null, null)
        };

        var toolName = "reply-email";
        var tool = new Tool(toolName, "reply to the email", [new ToolParameter("Content", "string", "text of the reply")]);

        while (dbMailMessage.AutoResponse == null)
        {
            var request = new LlmRequest([.. messages], [tool]);
            var response = await llmClient.ChatAsync(model, request, ct);
            messages.Add(response.Message);

            dbMailMessage.AutoResponse = response.Message.ToolCalls?
                .FirstOrDefault(a => a.Function.Name == toolName)?
                .Function.Arguments.Content;
            message.Email.AutoResponse = dbMailMessage.AutoResponse;
        }

        await db.SaveChangesAsync(ct);

        await sender.SendAsync(Bus.Api, new GenerateAutoReplyResponse(message.Email), ct);
    }
}
