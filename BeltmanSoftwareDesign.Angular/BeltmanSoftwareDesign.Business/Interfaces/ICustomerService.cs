using BeltmanSoftwareDesign.Shared.RequestJsons;
using BeltmanSoftwareDesign.Shared.ResponseJsons;

namespace BeltmanSoftwareDesign.Business.Interfaces
{
    public interface ICustomerService
    {
        CustomerCreateResponse Create(CustomerCreateRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
        CustomerDeleteResponse Delete(CustomerDeleteRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
        CustomerListResponse List(CustomerListRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
        CustomerReadResponse Read(CustomerReadRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
        CustomerUpdateResponse Update(CustomerUpdateRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
    }
}
