using MyCodingAgent.Models;

namespace MyCodingAgent.BaseAgents;

public class YesNoBaseAgent
{
    public async Task<bool?> ProcessResponse(AgentResponse response)
    {
        if (response.responseText.Contains("[NO]", StringComparison.InvariantCultureIgnoreCase))
        {
            return false;
        }
        else if (response.responseText.Contains("[YES]", StringComparison.InvariantCultureIgnoreCase))
        {
            return true;
        }
        return null;
    }
}
