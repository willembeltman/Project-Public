using Microsoft.AspNetCore.Mvc;
using BeltmanSoftwareDesign.Business.Interfaces;
using BeltmanSoftwareDesign.Shared.RequestJsons;
using BeltmanSoftwareDesign.Shared.ResponseJsons;
namespace BeltmanSoftwareDesign.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CountriesController : BaseControllerBase
    {
        public CountriesController(ICountriesService countriesService) 
        {
            CountriesService = countriesService;
        }

        public ICountriesService CountriesService { get; }

        [HttpPost]
        public CountryListResponse List(CountryListRequest request) 
            => CountriesService.List(request, IpAddress, Headers);
    }
}
