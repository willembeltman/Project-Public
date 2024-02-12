using Microsoft.AspNetCore.Mvc;
using BeltmanSoftwareDesign.Business.Interfaces;
using BeltmanSoftwareDesign.Shared.RequestJsons;
using BeltmanSoftwareDesign.Shared.ResponseJsons;
namespace BeltmanSoftwareDesign.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WorkordersController : BaseControllerBase
    {
        public WorkordersController(IWorkordersService workordersService) 
        {
            WorkordersService = workordersService;
        }

        public IWorkordersService WorkordersService { get; }

        [HttpPost]
        public WorkorderCreateResponse Create(WorkorderCreateRequest request) 
            => WorkordersService.Create(request, IpAddress, Headers);

        [HttpPost]
        public WorkorderReadResponse Read(WorkorderReadRequest request) 
            => WorkordersService.Read(request, IpAddress, Headers);

        [HttpPost]
        public WorkorderUpdateResponse Update(WorkorderUpdateRequest request) 
            => WorkordersService.Update(request, IpAddress, Headers);

        [HttpPost]
        public WorkorderDeleteResponse Delete(WorkorderDeleteRequest request) 
            => WorkordersService.Delete(request, IpAddress, Headers);

        [HttpPost]
        public WorkorderListResponse List(WorkorderListRequest request) 
            => WorkordersService.List(request, IpAddress, Headers);
    }
}
