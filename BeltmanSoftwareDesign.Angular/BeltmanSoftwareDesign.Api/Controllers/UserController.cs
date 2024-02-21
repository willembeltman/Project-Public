using Microsoft.AspNetCore.Mvc;
using BeltmanSoftwareDesign.Business.Interfaces;
using BeltmanSoftwareDesign.Shared.RequestJsons;
using BeltmanSoftwareDesign.Shared.ResponseJsons;
namespace BeltmanSoftwareDesign.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : BaseControllerBase
    {
        public UserController(IUserService userService) 
        {
            UserService = userService;
        }

        public IUserService UserService { get; }

        [HttpPost]
        public SetCurrentCompanyResponse SetCurrentCompany(SetCurrentCompanyRequest request) 
            => UserService.SetCurrentCompany(request, IpAddress, Headers);

        [HttpPost]
        public UserReadResponse ReadKnownUser(UserReadRequest request) 
            => UserService.ReadKnownUser(request, IpAddress, Headers);

        [HttpPost]
        public UserUpdateResponse UpdateMyself(UserUpdateRequest request) 
            => UserService.UpdateMyself(request, IpAddress, Headers);

        [HttpPost]
        public UserDeleteResponse DeleteMyself(UserDeleteRequest request) 
            => UserService.DeleteMyself(request, IpAddress, Headers);

        [HttpPost]
        public UserListResponse ListKnownUsers(UserListRequest request) 
            => UserService.ListKnownUsers(request, IpAddress, Headers);
    }
}
