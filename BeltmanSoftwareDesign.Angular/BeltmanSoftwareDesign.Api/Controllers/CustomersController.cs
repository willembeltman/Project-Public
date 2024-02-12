using Microsoft.AspNetCore.Mvc;
using BeltmanSoftwareDesign.Business.Interfaces;
using BeltmanSoftwareDesign.Shared.RequestJsons;
using BeltmanSoftwareDesign.Shared.ResponseJsons;
namespace BeltmanSoftwareDesign.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CustomersController : BaseControllerBase
    {
        public CustomersController(ICustomersService customersService) 
        {
            CustomersService = customersService;
        }

        public ICustomersService CustomersService { get; }

        [HttpPost]
        public CustomerCreateResponse Create(CustomerCreateRequest request) 
            => CustomersService.Create(request, IpAddress, Headers);

        [HttpPost]
        public CustomerReadResponse Read(CustomerReadRequest request) 
            => CustomersService.Read(request, IpAddress, Headers);

        [HttpPost]
        public CustomerUpdateResponse Update(CustomerUpdateRequest request) 
            => CustomersService.Update(request, IpAddress, Headers);

        [HttpPost]
        public CustomerDeleteResponse Delete(CustomerDeleteRequest request) 
            => CustomersService.Delete(request, IpAddress, Headers);

        [HttpPost]
        public CustomerListResponse List(CustomerListRequest request) 
            => CustomersService.List(request, IpAddress, Headers);
    }
}
