using BeltmanSoftwareDesign.Business.Models;
using BeltmanSoftwareDesign.Shared.Jsons;
using BeltmanSoftwareDesign.Shared.RequestJsons;
using BeltmanSoftwareDesign.Shared.ResponseJsons;

namespace BeltmanSoftwareDesign.Business.Interfaces
{
    public interface IUserService
    {
        //UserCreateResponse Create(UserCreateRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
        DeleteMyselfResponse DeleteMyself(DeleteMyselfRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
        ListKnownUsersResponse ListKnownUsers(ListKnownUsersRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
        ReadKnownUserResponse ReadKnownUser(ReadKnownUserRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
        SetCurrentCompanyResponse SetCurrentCompany(SetCurrentCompanyRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
        UpdateMyselfResponse UpdateMyself(UpdateMyselfRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
    }
}