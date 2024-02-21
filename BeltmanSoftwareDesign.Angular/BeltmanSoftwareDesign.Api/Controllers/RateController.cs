using Microsoft.AspNetCore.Mvc;
using BeltmanSoftwareDesign.Business.Interfaces;
using BeltmanSoftwareDesign.Shared.RequestJsons;
using BeltmanSoftwareDesign.Shared.ResponseJsons;
namespace BeltmanSoftwareDesign.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class RateController : BaseControllerBase
    {
        public RateController(IRateService rateService) 
        {
            RateService = rateService;
        }

        public IRateService RateService { get; }

        [HttpPost]
        public RateCreateResponse Create(RateCreateRequest request) 
            => RateService.Create(request, IpAddress, Headers);

        [HttpPost]
        public RateReadResponse Read(RateReadRequest request) 
            => RateService.Read(request, IpAddress, Headers);

        [HttpPost]
        public RateUpdateResponse Update(RateUpdateRequest request) 
            => RateService.Update(request, IpAddress, Headers);

        [HttpPost]
        public RateDeleteResponse Delete(RateDeleteRequest request) 
            => RateService.Delete(request, IpAddress, Headers);

        [HttpPost]
        public RateListResponse List(RateListRequest request) 
            => RateService.List(request, IpAddress, Headers);
    }
}
