using Microsoft.AspNetCore.Mvc;
using BeltmanSoftwareDesign.Business.Interfaces;
using BeltmanSoftwareDesign.Shared.RequestJsons;
using BeltmanSoftwareDesign.Shared.ResponseJsons;
namespace BeltmanSoftwareDesign.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WorkorderController : BaseControllerBase
    {
        public WorkorderController(IWorkorderService workorderService) 
        {
            WorkorderService = workorderService;
        }

        public IWorkorderService WorkorderService { get; }

        [HttpPost]
        public WorkorderCreateResponse Create(WorkorderCreateRequest request) 
            => WorkorderService.Create(request, IpAddress, Headers);

        [HttpPost]
        public WorkorderReadResponse Read(WorkorderReadRequest request) 
            => WorkorderService.Read(request, IpAddress, Headers);

        [HttpPost]
        public WorkorderUpdateResponse Update(WorkorderUpdateRequest request) 
            => WorkorderService.Update(request, IpAddress, Headers);

        [HttpPost]
        public WorkorderDeleteResponse Delete(WorkorderDeleteRequest request) 
            => WorkorderService.Delete(request, IpAddress, Headers);

        [HttpPost]
        public WorkorderListResponse List(WorkorderListRequest request) 
            => WorkorderService.List(request, IpAddress, Headers);
    }
}
