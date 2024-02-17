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
    public class WorkorderService : IWorkorderService
    {
        ApplicationDbContext db { get; }
        IStorageFileService StorageFileService { get; }
        IAuthenticationService AuthenticationService { get; }
        WorkorderFactory WorkorderFactory { get; }

        public WorkorderService(
            ApplicationDbContext db,
            IStorageFileService storageFileService,
            IAuthenticationService authenticationService)
        {
            this.db = db;
            StorageFileService = storageFileService;
            AuthenticationService = authenticationService;
            WorkorderFactory = new WorkorderFactory(storageFileService);
        }

        [TsServiceMethod("Workorder", "Create")]
        public WorkorderCreateResponse Create(WorkorderCreateRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        {
            var authentication = AuthenticationService.GetState(
                request.BearerId, request.CurrentCompanyId, ipAddress, headers);
            if (!authentication.Success)
                return new WorkorderCreateResponse()
                {
                    ErrorAuthentication = true
                };

            if (authentication.DbCurrentCompany == null)
                throw new Exception("Current company not chosen or doesn't exist, please create a company or select one.");

            var dbworkorder = WorkorderFactory.Convert(request.Workorder);

            dbworkorder.CompanyId = authentication.DbCurrentCompany.id;
            dbworkorder.Company = authentication.DbCurrentCompany;

            dbworkorder.Customer = null;
            if (!string.IsNullOrEmpty(request.Workorder.ProjectName))
            {
                dbworkorder.Customer = db.Customers.FirstOrDefault(a =>
                    a.CompanyId == authentication.DbCurrentCompany.id &&
                    a.Name.ToLower() == request.Workorder.CustomerName.ToLower());
                if (dbworkorder.Customer == null)
                {
                    dbworkorder.Customer = new Data.Entities.Customer()
                    {
                        Name = request.Workorder.CustomerName,
                        CompanyId = authentication.DbCurrentCompany.id
                    };
                    db.Customers.Add(dbworkorder.Customer);
                    db.SaveChanges();
                }
            }
            dbworkorder.CustomerId = dbworkorder.Customer?.id;

            dbworkorder.Project = null;
            if (!string.IsNullOrEmpty(request.Workorder.ProjectName))
            {
                dbworkorder.Project = db.Projects.FirstOrDefault(a =>
                    a.CompanyId == authentication.DbCurrentCompany.id &&
                    a.Name.ToLower() == request.Workorder.ProjectName.ToLower());
                if (dbworkorder.Project == null)
                {
                    dbworkorder.Project = new Data.Entities.Project()
                    {
                        Name = request.Workorder.ProjectName,
                        CompanyId = authentication.DbCurrentCompany.id,
                        Customer = dbworkorder.Customer,
                    };
                    db.Projects.Add(dbworkorder.Project);
                    db.SaveChanges();
                }
            }
            dbworkorder.ProjectId = dbworkorder.Project?.id;

            db.Workorders.Add(dbworkorder);
            db.SaveChanges();

            return new WorkorderCreateResponse()
            {
                Success = true,
                State = authentication,
                Workorder = WorkorderFactory.Convert(dbworkorder)
            };
        }

        [TsServiceMethod("Workorder", "Read")]
        public WorkorderReadResponse Read(WorkorderReadRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        {
            var authentication = AuthenticationService.GetState(
                request.BearerId, request.CurrentCompanyId, ipAddress, headers);
            if (!authentication.Success)
                return new WorkorderReadResponse()
                {
                    ErrorAuthentication = true
                };

            if (authentication.DbCurrentCompany == null)
                throw new Exception("Current company not chosen or doesn't exist, please create a company or select one.");

            var dbworkorder = db.Workorders
                .Include(a => a.Company)
                .Include(a => a.Customer)
                .Include(a => a.Project)
                .Include(a => a.InvoiceWorkorders)
                .Include(a => a.WorkorderAttachments)
                .FirstOrDefault(a => a.id == request.WorkorderId);
            if (dbworkorder == null)
                return new WorkorderReadResponse()
                {
                    ErrorItemNotFound = true,
                    State = authentication
                };

            if (dbworkorder.CompanyId != authentication.DbCurrentCompany.id)
                return new WorkorderReadResponse()
                {
                    ErrorWrongCompany = true,
                    State = authentication
                };

            return new WorkorderReadResponse()
            {
                Success = true,
                State = authentication,
                Workorder = WorkorderFactory.Convert(dbworkorder)
            };
        }

        [TsServiceMethod("Workorder", "Update")]
        public WorkorderUpdateResponse Update(WorkorderUpdateRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        {
            var authentication = AuthenticationService.GetState(
                request.BearerId, request.CurrentCompanyId, ipAddress, headers);
            if (!authentication.Success)
                return new WorkorderUpdateResponse()
                {
                    ErrorAuthentication = true
                };

            if (authentication.DbCurrentCompany == null)
                throw new Exception("Current company not chosen or doesn't exist, please create a company or select one.");

            var dbworkorder = db.Workorders
                .Include(a => a.Company)
                .Include(a => a.Customer)
                .Include(a => a.Project)
                .Include(a => a.InvoiceWorkorders)
                .Include(a => a.WorkorderAttachments)
                .FirstOrDefault(a => a.id == request.Workorder.id);
            if (dbworkorder == null)
                return new WorkorderUpdateResponse()
                {
                    ErrorItemNotFound = true,
                    State = authentication
                };

            if (dbworkorder.CompanyId != authentication.DbCurrentCompany.id)
                return new WorkorderUpdateResponse()
                {
                    ErrorWrongCompany = true,
                    State = authentication
                };

            if (WorkorderFactory.Copy(request.Workorder, dbworkorder))
                db.SaveChanges();

            return new WorkorderUpdateResponse()
            {
                Success = true,
                State = authentication,
                Workorder = WorkorderFactory.Convert(dbworkorder)
            };
        }

        [TsServiceMethod("Workorder", "Delete")]
        public WorkorderDeleteResponse Delete(WorkorderDeleteRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        {
            var authentication = AuthenticationService.GetState(
                request.BearerId, request.CurrentCompanyId, ipAddress, headers);
            if (!authentication.Success)
                return new WorkorderDeleteResponse()
                {
                    ErrorAuthentication = true
                };

            if (authentication.DbCurrentCompany == null)
                throw new Exception("Current company not chosen or doesn't exist, please create a company or select one.");

            var dbworkorder = db.Workorders
                .Include(a => a.Company)
                .Include(a => a.Customer)
                .Include(a => a.Project)
                .Include(a => a.InvoiceWorkorders)
                .Include(a => a.WorkorderAttachments)
                .FirstOrDefault(a => a.id == request.WorkorderId);
            if (dbworkorder == null)
                return new WorkorderDeleteResponse()
                {
                    ErrorItemNotFound = true,
                    State = authentication
                };

            if (dbworkorder.CompanyId != authentication.DbCurrentCompany.id)
                return new WorkorderDeleteResponse()
                {
                    ErrorWrongCompany = true,
                    State = authentication
                };

            db.Workorders.Remove(dbworkorder);
            db.SaveChanges();

            return new WorkorderDeleteResponse()
            {
                Success = true,
                State = authentication
            };
        }

        [TsServiceMethod("Workorder", "List")]
        public WorkorderListResponse List(WorkorderListRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        {
            var authentication = AuthenticationService.GetState(
                request.BearerId, request.CurrentCompanyId, ipAddress, headers);
            if (!authentication.Success)
                return new WorkorderListResponse()
                {
                    ErrorAuthentication = true
                };

            if (authentication.DbCurrentCompany == null)
                throw new Exception("Current company not chosen or doesn't exist, please create a company or select one.");

            var list = db.Workorders
                .Include(a => a.Company)
                .Include(a => a.Customer)
                .Include(a => a.Project)
                .Include(a => a.InvoiceWorkorders)
                .Include(a => a.WorkorderAttachments)
                .Where(a => a.CompanyId == authentication.DbCurrentCompany.id)
                .Select(a => WorkorderFactory.Convert(a))
                .ToArray();

            return new WorkorderListResponse()
            {
                Success = true,
                State = authentication,
                Workorders = list
            };
        }
    }
}
