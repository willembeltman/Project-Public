using Microsoft.AspNetCore.Mvc;
using BeltmanSoftwareDesign.Business.Interfaces;
using BeltmanSoftwareDesign.Shared.RequestJsons;
using BeltmanSoftwareDesign.Shared.ResponseJsons;
namespace BeltmanSoftwareDesign.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class InvoiceController : BaseControllerBase
    {
        public InvoiceController(IInvoiceService invoiceService) 
        {
            InvoiceService = invoiceService;
        }

        public IInvoiceService InvoiceService { get; }

        [HttpPost]
        public InvoiceCreateResponse Create(InvoiceCreateRequest request) 
            => InvoiceService.Create(request, IpAddress, Headers);

        [HttpPost]
        public InvoiceReadResponse Read(InvoiceReadRequest request) 
            => InvoiceService.Read(request, IpAddress, Headers);

        [HttpPost]
        public InvoiceUpdateResponse Update(InvoiceUpdateRequest request) 
            => InvoiceService.Update(request, IpAddress, Headers);

        [HttpPost]
        public InvoiceDeleteResponse Delete(InvoiceDeleteRequest request) 
            => InvoiceService.Delete(request, IpAddress, Headers);

        [HttpPost]
        public InvoiceListResponse List(InvoiceListRequest request) 
            => InvoiceService.List(request, IpAddress, Headers);
    }
}
