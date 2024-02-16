using BeltmanSoftwareDesign.Business.Models;
using BeltmanSoftwareDesign.Shared.Jsons;
using BeltmanSoftwareDesign.Shared.RequestJsons;
using BeltmanSoftwareDesign.Shared.ResponseJsons;

namespace BeltmanSoftwareDesign.Business.Interfaces
{
    public interface ICompanyService
    {
        CompanyCreateResponse Create(CompanyCreateRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
        CompanyDeleteResponse Delete(CompanyDeleteRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
        CompanyListResponse List(CompanyListRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
        CompanyReadResponse Read(CompanyReadRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
        CompanyUpdateResponse Update(CompanyUpdateRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
    }
}