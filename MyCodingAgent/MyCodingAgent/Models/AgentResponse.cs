namespace MyCodingAgent.Models;

public record AgentResponse(
    string model,
    DateTime created_at,
    AgentResponseMessage message);

public record AgentResponseMessage(
    string content,
    AgentResponseMessageToolCall[] tool_calls);

public record AgentResponseMessageToolCall(
    AgentAction function);

public record AgentAction(
    string name,
    AgentActionArguments arguments);