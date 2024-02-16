using Microsoft.AspNetCore.Mvc;
using BeltmanSoftwareDesign.Business.Interfaces;
using BeltmanSoftwareDesign.Shared.RequestJsons;
using BeltmanSoftwareDesign.Shared.ResponseJsons;
namespace BeltmanSoftwareDesign.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CountryController : BaseControllerBase
    {
        public CountryController(ICountryService countryService) 
        {
            CountryService = countryService;
        }

        public ICountryService CountryService { get; }

        [HttpPost]
        public CountryListResponse List(CountryListRequest request) 
            => CountryService.List(request, IpAddress, Headers);
    }
}
