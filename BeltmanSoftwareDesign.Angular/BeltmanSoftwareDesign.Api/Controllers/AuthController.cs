using Microsoft.AspNetCore.Mvc;
using BeltmanSoftwareDesign.Business.Interfaces;
using BeltmanSoftwareDesign.Shared.RequestJsons;
using BeltmanSoftwareDesign.Shared.ResponseJsons;
namespace BeltmanSoftwareDesign.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthController : BaseControllerBase
    {
        public AuthController(IAuthenticationService authenticationService) 
        {
            AuthenticationService = authenticationService;
        }

        public IAuthenticationService AuthenticationService { get; }

        [HttpPost]
        public LoginResponse Login(LoginRequest request) 
            => AuthenticationService.Login(request, IpAddress, Headers);

        [HttpPost]
        public RegisterResponse Register(RegisterRequest request) 
            => AuthenticationService.Register(request, IpAddress, Headers);
    }
}
