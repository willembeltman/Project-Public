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
        public UserReadResponse Read(UserReadRequest request) 
            => UserService.Read(request, IpAddress, Headers);

        [HttpPost]
        public UserUpdateResponse Update(UserUpdateRequest request) 
            => UserService.Update(request, IpAddress, Headers);

        [HttpPost]
        public UserDeleteResponse Delete(UserDeleteRequest request) 
            => UserService.Delete(request, IpAddress, Headers);

        [HttpPost]
        public UserListResponse List(UserListRequest request) 
            => UserService.List(request, IpAddress, Headers);
    }
}
