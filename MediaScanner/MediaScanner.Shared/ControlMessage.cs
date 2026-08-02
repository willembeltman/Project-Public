using System;
using System.Collections.Generic;

namespace MediaScanner.Shared;

public class ControlMessage
{
    public MessageType Type { get; set; }
    public string? Payload { get; set; } // JSON serialized payload
}
