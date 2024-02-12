using Microsoft.AspNetCore.Mvc;
using BeltmanSoftwareDesign.Business.Interfaces;
using BeltmanSoftwareDesign.Shared.RequestJsons;
using BeltmanSoftwareDesign.Shared.ResponseJsons;
namespace BeltmanSoftwareDesign.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UsersController : BaseControllerBase
    {
        public UsersController(IUsersService usersService) 
        {
            UsersService = usersService;
        }

        public IUsersService UsersService { get; }

        [HttpPost]
        public SetCurrentCompanyResponse SetCurrentCompany(SetCurrentCompanyRequest request) 
            => UsersService.SetCurrentCompany(request, IpAddress, Headers);

        [HttpPost]
        public UserReadResponse Read(UserReadRequest request) 
            => UsersService.Read(request, IpAddress, Headers);

        [HttpPost]
        public UserUpdateResponse Update(UserUpdateRequest request) 
            => UsersService.Update(request, IpAddress, Headers);

        [HttpPost]
        public UserDeleteResponse Delete(UserDeleteRequest request) 
            => UsersService.Delete(request, IpAddress, Headers);

        [HttpPost]
        public UserListResponse List(UserListRequest request) 
            => UsersService.List(request, IpAddress, Headers);
    }
}
