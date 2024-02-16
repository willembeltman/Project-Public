using BeltmanSoftwareDesign.Shared.RequestJsons;
using BeltmanSoftwareDesign.Shared.ResponseJsons;

namespace BeltmanSoftwareDesign.Business.Interfaces
{
    public interface ICountryService
    {
        CountryListResponse List(CountryListRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
    }
}
