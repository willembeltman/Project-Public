using BeltmanSoftwareDesign.Business.Interfaces;
using BeltmanSoftwareDesign.Data;
using BeltmanSoftwareDesign.Data.Converters;
using BeltmanSoftwareDesign.Shared.Attributes;
using BeltmanSoftwareDesign.Shared.RequestJsons;
using BeltmanSoftwareDesign.Shared.ResponseJsons;

namespace BeltmanSoftwareDesign.Business.Services
{
    public class CountryService : ICountryService
    {
        ApplicationDbContext db { get; }
        IAuthenticationService AuthenticationService { get; }
        CountryConverter CountryConverter { get; }

        public CountryService(
            ApplicationDbContext db,
            IAuthenticationService authenticationService)
        {
            this.db = db;
            AuthenticationService = authenticationService;
            CountryConverter = new CountryConverter();
        }

        [TsServiceMethod("Country", "List")]
        public CountryListResponse List(CountryListRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        {
            if (ipAddress == null)
                return new CountryListResponse()
                {
                    ErrorAuthentication = true
                };

            var authentication = AuthenticationService.GetState(
                request, ipAddress, headers);
            if (!authentication.Success)
                return new CountryListResponse()
                {
                    ErrorAuthentication = true
                };

            var list = db.Countries
                .Select(a => CountryConverter.Create(a))
                .ToArray();

            return new CountryListResponse()
            {
                Success = true,
                State = authentication,
                Countries = list
            };
        }
    }
}
