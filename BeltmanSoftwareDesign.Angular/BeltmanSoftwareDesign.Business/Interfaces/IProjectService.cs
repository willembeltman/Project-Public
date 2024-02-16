using BeltmanSoftwareDesign.Shared.RequestJsons;
using BeltmanSoftwareDesign.Shared.ResponseJsons;

namespace BeltmanSoftwareDesign.Business.Interfaces
{
    public interface IProjectService
    {
        ProjectCreateResponse Create(ProjectCreateRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
        ProjectDeleteResponse Delete(ProjectDeleteRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
        ProjectListResponse List(ProjectListRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
        ProjectReadResponse Read(ProjectReadRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
        ProjectUpdateResponse Update(ProjectUpdateRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
    }
}
