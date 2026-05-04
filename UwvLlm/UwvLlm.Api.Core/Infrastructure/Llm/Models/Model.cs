namespace UwvLlm.Core.Infrastructure.Llm.Models;

public record Model(
    string Name, 
    long? MemorySize, 
    int? MaxTokenSize,
    DateTime? LastModified);