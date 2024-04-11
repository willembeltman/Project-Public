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
        public ReadKnownUserResponse ReadKnownUser(ReadKnownUserRequest request) 
            => UserService.ReadKnownUser(request, IpAddress, Headers);

        [HttpPost]
        public UpdateMyselfResponse UpdateMyself(UpdateMyselfRequest request) 
            => UserService.UpdateMyself(request, IpAddress, Headers);

        [HttpPost]
        public DeleteMyselfResponse DeleteMyself(DeleteMyselfRequest request) 
            => UserService.DeleteMyself(request, IpAddress, Headers);

        [HttpPost]
        public ListKnownUsersResponse ListKnownUsers(ListKnownUsersRequest request) 
            => UserService.ListKnownUsers(request, IpAddress, Headers);
    }
}
