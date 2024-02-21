using BeltmanSoftwareDesign.Shared.RequestJsons;
using BeltmanSoftwareDesign.Shared.ResponseJsons;

namespace BeltmanSoftwareDesign.Business.Interfaces
{
    public interface IRateService
    {
        RateCreateResponse Create(RateCreateRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
        RateDeleteResponse Delete(RateDeleteRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
        RateListResponse List(RateListRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
        RateReadResponse Read(RateReadRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
        RateUpdateResponse Update(RateUpdateRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
    }
}