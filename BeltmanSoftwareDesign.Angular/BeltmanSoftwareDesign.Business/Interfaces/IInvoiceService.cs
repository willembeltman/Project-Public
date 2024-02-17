using BeltmanSoftwareDesign.Shared.RequestJsons;
using BeltmanSoftwareDesign.Shared.ResponseJsons;

namespace BeltmanSoftwareDesign.Business.Interfaces
{
    public interface IInvoiceService
    {
        InvoiceCreateResponse Create(InvoiceCreateRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
        InvoiceDeleteResponse Delete(InvoiceDeleteRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
        InvoiceListResponse List(InvoiceListRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
        InvoiceReadResponse Read(InvoiceReadRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
        InvoiceUpdateResponse Update(InvoiceUpdateRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers);
    }
}
