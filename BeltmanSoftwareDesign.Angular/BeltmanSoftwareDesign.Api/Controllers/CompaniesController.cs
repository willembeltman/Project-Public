using Microsoft.AspNetCore.Mvc;
using BeltmanSoftwareDesign.Business.Interfaces;
using BeltmanSoftwareDesign.Shared.RequestJsons;
using BeltmanSoftwareDesign.Shared.ResponseJsons;
namespace BeltmanSoftwareDesign.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CompaniesController : BaseControllerBase
    {
        public CompaniesController(ICompaniesService companiesService) 
        {
            CompaniesService = companiesService;
        }

        public ICompaniesService CompaniesService { get; }

        [HttpPost]
        public CompanyCreateResponse Create(CompanyCreateRequest request) 
            => CompaniesService.Create(request, IpAddress, Headers);

        [HttpPost]
        public CompanyReadResponse Read(CompanyReadRequest request) 
            => CompaniesService.Read(request, IpAddress, Headers);

        [HttpPost]
        public CompanyUpdateResponse Update(CompanyUpdateRequest request) 
            => CompaniesService.Update(request, IpAddress, Headers);

        [HttpPost]
        public CompanyDeleteResponse Delete(CompanyDeleteRequest request) 
            => CompaniesService.Delete(request, IpAddress, Headers);

        [HttpPost]
        public CompanyListResponse List(CompanyListRequest request) 
            => CompaniesService.List(request, IpAddress, Headers);
    }
}
