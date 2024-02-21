using Microsoft.AspNetCore.Mvc;

namespace BeltmanSoftwareDesign.Api.Controllers
{
    public class BaseControllerBase : ControllerBase
    {
        public string? IpAddress => HttpContext.Connection.RemoteIpAddress?.ToString();
        public KeyValuePair<string, string?>[]? Headers => Request.Headers
            .Select(a => new KeyValuePair<string, string?>(a.Key, a.Value))
            .ToArray();
    }
}
