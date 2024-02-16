using BeltmanSoftwareDesign.Shared.RequestJsons;
using BeltmanSoftwareDesign.Shared.ResponseJsons;

namespace BeltmanSoftwareDesign.Business.Interfaces
{
    public interface IWorkorderService
    {
        WorkorderCreateResponse Create(WorkorderCreateRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
        WorkorderDeleteResponse Delete(WorkorderDeleteRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
        WorkorderListResponse List(WorkorderListRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
        WorkorderReadResponse Read(WorkorderReadRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
        WorkorderUpdateResponse Update(WorkorderUpdateRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
    }
}
