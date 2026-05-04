using gAPI.Interfaces;
using Microsoft.Extensions.Logging;
using System.Buffers;
using UwvLlm.Shared.Dtos;

namespace UwvLlm.Shared;

public class StateParser(ILoggerFactory loggerFactory) : IStateParser<StateDto>
{
    public ILogger Logger { get; } = loggerFactory.CreateLogger<StateParser>();

    public bool TryParse(string? value, out StateDto state)
    {
        state = default!;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        try
        {
            var data = Convert.FromBase64String(value);
            var offset = 0;
            state = data.ReadStateDto(ref offset);
            return true;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to parse state from string value.");
            return false;
        }
    }
    public string ToStringBase64(StateDto? value)
    {
        value ??= new StateDto();

        var writer = new ArrayBufferWriter<byte>();
        var span = writer.GetSpan();

        var offset = 0;
        span.Write(ref offset, value);

        writer.Advance(offset);

        return Convert.ToBase64String(writer.WrittenSpan);
    }
    public bool IsDifferent(StateDto? value1, StateDto? value2)
    {
        if (value1 == null && value2 == null) return false;
        if (value1 == null || value2 == null) return true;
        return value1.IsDifferent(value2);
    }
    public StateDto? CreateCopy(StateDto? value) => value?.CreateCopy();
}
