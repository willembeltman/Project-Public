using System.Net;

namespace CPUCalculator2.Helpers;

public static class HttpClientHelper
{
    public static string GetWebpage(string url)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        using (var req = new HttpClient())
        {
            return req.GetStringAsync(url).Result;
        }
    }
}
