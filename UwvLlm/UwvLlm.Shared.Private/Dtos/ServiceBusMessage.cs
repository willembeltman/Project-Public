namespace UwvLlm.Shared.Private.Dtos;

public record ServiceBusMessage(
    string MessageType,
    string Payload);
