using BeltmanSoftwareDesign.Business.Models;
using BeltmanSoftwareDesign.Shared.RequestJsons;
using BeltmanSoftwareDesign.Shared.ResponseJsons;

namespace BeltmanSoftwareDesign.Business.Interfaces
{
    public interface IAuthenticationService
    {
        AuthenticationState GetState(Request request, string? requestIpAddress, KeyValuePair<string, string?>[]? requestHeaders);
        LoginResponse Login(LoginRequest request, string? requestIpAddress, KeyValuePair<string, string?>[]? requestHeaders);
        RegisterResponse Register(RegisterRequest request, string? requestIpAddress, KeyValuePair<string, string?>[]? requestHeaders);
    }
}
