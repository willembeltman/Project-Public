using BeltmanSoftwareDesign.Business.Interfaces;
using BeltmanSoftwareDesign.Business.Models;
using BeltmanSoftwareDesign.Data;
using BeltmanSoftwareDesign.Data.Factories;
using BeltmanSoftwareDesign.Shared.Attributes;
using BeltmanSoftwareDesign.Shared.RequestJsons;
using BeltmanSoftwareDesign.Shared.ResponseJsons;
using BeltmanSoftwareDesign.StorageBlob.Business.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BeltmanSoftwareDesign.Business.Services
{
    public class InvoiceService : IInvoiceService
    {
        ApplicationDbContext db { get; }
        IStorageFileService StorageFileService { get; }
        IAuthenticationService AuthenticationService { get; }
        InvoiceFactory InvoiceFactory { get; }

        public InvoiceService(
            ApplicationDbContext db,
            IStorageFileService storageFileService,
            IAuthenticationService authenticationService)
        {
            this.db = db;
            StorageFileService = storageFileService;
            AuthenticationService = authenticationService;
            InvoiceFactory = new InvoiceFactory(storageFileService);
        }

        [TsServiceMethod("Invoice", "Create")]
        public InvoiceCreateResponse Create(InvoiceCreateRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        {
            var authentication = AuthenticationService.GetState(
                request.BearerId, request.CurrentCompanyId, ipAddress, headers);
            if (!authentication.Success)
                return new InvoiceCreateResponse()
                {
                    ErrorAuthentication = true
                };

            if (authentication.DbCurrentCompany == null)
                throw new Exception("Current company not chosen or doesn't exist, please create a company or select one.");

            var dbinvoice = InvoiceFactory.Convert(request.Invoice);

            dbinvoice.CompanyId = authentication.DbCurrentCompany.id;
            dbinvoice.Company = authentication.DbCurrentCompany;

            //dbinvoice.Customer = null;
            //if (!string.IsNullOrEmpty(request.Invoice.ProjectName))
            //{
            //    dbinvoice.Customer = db.Customers.FirstOrDefault(a =>
            //        a.CompanyId == authentication.DbCurrentCompany.id &&
            //        a.Name.ToLower() == request.Invoice.CustomerName.ToLower());
            //    if (dbinvoice.Customer == null)
            //    {
            //        dbinvoice.Customer = new Data.Entities.Customer()
            //        {
            //            Name = request.Invoice.CustomerName,
            //            CompanyId = authentication.DbCurrentCompany.id
            //        };
            //        db.Customers.Add(dbinvoice.Customer);
            //        db.SaveChanges();
            //    }
            //}
            //dbinvoice.CustomerId = dbinvoice.Customer?.id;

            //dbinvoice.Project = null;
            //if (!string.IsNullOrEmpty(request.Invoice.ProjectName))
            //{
            //    dbinvoice.Project = db.Projects.FirstOrDefault(a => 
            //        a.CompanyId == authentication.DbCurrentCompany.id &&
            //        a.Name.ToLower() == request.Invoice.ProjectName.ToLower());
            //    if (dbinvoice.Project == null)
            //    {
            //        dbinvoice.Project = new Data.Entities.Project()
            //        {
            //            Name = request.Invoice.ProjectName,
            //            CompanyId = authentication.DbCurrentCompany.id,
            //            Customer = dbinvoice.Customer,
            //        };
            //        db.Projects.Add(dbinvoice.Project);
            //        db.SaveChanges();
            //    }
            //}
            //dbinvoice.ProjectId = dbinvoice.Project?.id;

            db.Invoices.Add(dbinvoice);
            db.SaveChanges();

            return new InvoiceCreateResponse()
            {
                Success = true,
                State = authentication,
                Invoice = InvoiceFactory.Convert(dbinvoice)
            };
        }

        [TsServiceMethod("Invoice", "Read")]
        public InvoiceReadResponse Read(InvoiceReadRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        {
            var authentication = AuthenticationService.GetState(
                request.BearerId, request.CurrentCompanyId, ipAddress, headers);
            if (!authentication.Success)
                return new InvoiceReadResponse()
                {
                    ErrorAuthentication = true
                };

            if (authentication.DbCurrentCompany == null)
                throw new Exception("Current company not chosen or doesn't exist, please create a company or select one.");

            var dbinvoice = db.Invoices
                .Include(a => a.Company)
                .Include(a => a.Customer)
                .Include(a => a.Project)
                .Include(a => a.InvoiceWorkorders)
                .Include(a => a.InvoiceAttachments)
                .FirstOrDefault(a =>
                    a.CompanyId == authentication.DbCurrentCompany.id && 
                    a.id == request.InvoiceId);
            if (dbinvoice == null)
                return new InvoiceReadResponse()
                {
                    ErrorItemNotFound = true,
                    State = authentication
                };

            return new InvoiceReadResponse()
            {
                Success = true,
                State = authentication,
                Invoice = InvoiceFactory.Convert(dbinvoice)
            };
        }

        [TsServiceMethod("Invoice", "Update")]
        public InvoiceUpdateResponse Update(InvoiceUpdateRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        {
            var authentication = AuthenticationService.GetState(
                request.BearerId, request.CurrentCompanyId, ipAddress, headers);
            if (!authentication.Success)
                return new InvoiceUpdateResponse()
                {
                    ErrorAuthentication = true
                };

            if (authentication.DbCurrentCompany == null)
                throw new Exception("Current company not chosen or doesn't exist, please create a company or select one.");

            var dbinvoice = db.Invoices
                .Include(a => a.Company)
                .Include(a => a.Customer)
                .Include(a => a.Project)
                .Include(a => a.InvoiceWorkorders)
                .Include(a => a.InvoiceAttachments)
                .FirstOrDefault(a =>
                    a.CompanyId == authentication.DbCurrentCompany.id && 
                    a.id == request.Invoice.id);
            if (dbinvoice == null)
                return new InvoiceUpdateResponse()
                {
                    ErrorItemNotFound = true,
                    State = authentication
                };

            if (InvoiceFactory.Copy(request.Invoice, dbinvoice))
                db.SaveChanges();

            return new InvoiceUpdateResponse()
            {
                Success = true,
                State = authentication,
                Invoice = InvoiceFactory.Convert(dbinvoice)
            };
        }

        [TsServiceMethod("Invoice", "Delete")]
        public InvoiceDeleteResponse Delete(InvoiceDeleteRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        {
            var authentication = AuthenticationService.GetState(
                request.BearerId, request.CurrentCompanyId, ipAddress, headers);
            if (!authentication.Success)
                return new InvoiceDeleteResponse()
                {
                    ErrorAuthentication = true
                };

            if (authentication.DbCurrentCompany == null)
                throw new Exception("Current company not chosen or doesn't exist, please create a company or select one.");

            var dbinvoice = db.Invoices
                .Include(a => a.Company)
                .Include(a => a.Customer)
                .Include(a => a.Project)
                .Include(a => a.InvoiceWorkorders)
                .Include(a => a.InvoiceAttachments)
                .FirstOrDefault(a =>
                    a.CompanyId == authentication.DbCurrentCompany.id && 
                    a.id == request.InvoiceId);

            if (dbinvoice == null)
                return new InvoiceDeleteResponse()
                {
                    ErrorItemNotFound = true,
                    State = authentication
                };

            db.Invoices.Remove(dbinvoice);
            db.SaveChanges();

            return new InvoiceDeleteResponse()
            {
                Success = true,
                State = authentication
            };
        }

        [TsServiceMethod("Invoice", "List")]
        public InvoiceListResponse List(InvoiceListRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        {
            var authentication = AuthenticationService.GetState(
                request.BearerId, request.CurrentCompanyId, ipAddress, headers);
            if (!authentication.Success)
                return new InvoiceListResponse()
                {
                    ErrorAuthentication = true
                };

            if (authentication.DbCurrentCompany == null)
                throw new Exception("Current company not chosen or doesn't exist, please create a company or select one.");

            var list = db.Invoices
                .Include(a => a.Company)
                .Include(a => a.Customer)
                .Include(a => a.Project)
                .Include(a => a.InvoiceWorkorders)
                .Include(a => a.InvoiceAttachments)
                .Where(a => a.CompanyId == authentication.DbCurrentCompany.id)
                .Select(a => InvoiceFactory.Convert(a))
                .ToArray();

            return new InvoiceListResponse()
            {
                Success = true,
                State = authentication,
                Invoices = list
            };
        }
    }
}
