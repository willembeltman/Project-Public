using BeltmanSoftwareDesign.Business.Models;
using BeltmanSoftwareDesign.Shared.Jsons;
using BeltmanSoftwareDesign.Shared.RequestJsons;
using BeltmanSoftwareDesign.Shared.ResponseJsons;

namespace BeltmanSoftwareDesign.Business.Interfaces
{
    public interface IUsersService
    {
        //UserCreateResponse Create(UserCreateRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
        UserDeleteResponse Delete(UserDeleteRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
        UserListResponse List(UserListRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
        UserReadResponse Read(UserReadRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
        SetCurrentCompanyResponse SetCurrentCompany(SetCurrentCompanyRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
        UserUpdateResponse Update(UserUpdateRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
    }
}