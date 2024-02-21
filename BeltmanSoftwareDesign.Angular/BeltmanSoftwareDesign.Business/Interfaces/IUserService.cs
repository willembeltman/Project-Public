using BeltmanSoftwareDesign.Business.Models;
using BeltmanSoftwareDesign.Shared.Jsons;
using BeltmanSoftwareDesign.Shared.RequestJsons;
using BeltmanSoftwareDesign.Shared.ResponseJsons;

namespace BeltmanSoftwareDesign.Business.Interfaces
{
    public interface IUserService
    {
        //UserCreateResponse Create(UserCreateRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
        UserDeleteResponse DeleteMyself(UserDeleteRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
        UserListResponse ListKnownUsers(UserListRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
        UserReadResponse ReadKnownUser(UserReadRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
        SetCurrentCompanyResponse SetCurrentCompany(SetCurrentCompanyRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
        UserUpdateResponse UpdateMyself(UserUpdateRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
    }
}