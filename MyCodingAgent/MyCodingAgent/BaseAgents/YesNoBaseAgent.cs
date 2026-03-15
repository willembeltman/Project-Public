using MyCodingAgent.Models;

namespace MyCodingAgent.BaseAgents;

public class YesNoBaseAgent
{
    public async Task<bool?> ProcessResponse(AgentResponse response)
    {
        if (response.message.content.Contains("[NO]", StringComparison.InvariantCultureIgnoreCase))
        {
            return false;
        }
        else if (response.message.content.Contains("[YES]", StringComparison.InvariantCultureIgnoreCase))
        {
            return true;
        }
        return null;
    }
}
