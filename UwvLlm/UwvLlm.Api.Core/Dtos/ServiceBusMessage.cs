namespace UwvLlm.Api.Core.Dtos;

public record ServiceBusMessage(
    string MessageType,
    string Payload);
