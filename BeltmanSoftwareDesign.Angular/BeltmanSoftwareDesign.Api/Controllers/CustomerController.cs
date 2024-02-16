using Microsoft.AspNetCore.Mvc;
using BeltmanSoftwareDesign.Business.Interfaces;
using BeltmanSoftwareDesign.Shared.RequestJsons;
using BeltmanSoftwareDesign.Shared.ResponseJsons;
namespace BeltmanSoftwareDesign.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CustomerController : BaseControllerBase
    {
        public CustomerController(ICustomerService customerService) 
        {
            CustomerService = customerService;
        }

        public ICustomerService CustomerService { get; }

        [HttpPost]
        public CustomerCreateResponse Create(CustomerCreateRequest request) 
            => CustomerService.Create(request, IpAddress, Headers);

        [HttpPost]
        public CustomerReadResponse Read(CustomerReadRequest request) 
            => CustomerService.Read(request, IpAddress, Headers);

        [HttpPost]
        public CustomerUpdateResponse Update(CustomerUpdateRequest request) 
            => CustomerService.Update(request, IpAddress, Headers);

        [HttpPost]
        public CustomerDeleteResponse Delete(CustomerDeleteRequest request) 
            => CustomerService.Delete(request, IpAddress, Headers);

        [HttpPost]
        public CustomerListResponse List(CustomerListRequest request) 
            => CustomerService.List(request, IpAddress, Headers);
    }
}
