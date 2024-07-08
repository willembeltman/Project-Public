using BeltmanSoftwareDesign.Business.Interfaces;
using BeltmanSoftwareDesign.Business.Models;
using BeltmanSoftwareDesign.Data;
using BeltmanSoftwareDesign.Data.Converters;
using BeltmanSoftwareDesign.Shared.Attributes;
using BeltmanSoftwareDesign.Shared.RequestJsons;
using BeltmanSoftwareDesign.Shared.ResponseJsons;
using StorageBlob.Proxy.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BeltmanSoftwareDesign.Business.Services
{
    public class WorkorderService : IWorkorderService
    {
        ApplicationDbContext db { get; }
        IAuthenticationService AuthenticationService { get; }
        WorkorderConverter WorkorderConverter { get; }

        public WorkorderService(
            ApplicationDbContext db,
            IStorageFileService storageFileService,
            IAuthenticationService authenticationService)
        {
            this.db = db;
            AuthenticationService = authenticationService;
            WorkorderConverter = new WorkorderConverter(storageFileService);
        }

        [TsServiceMethod("Workorder", "Create")]
        public WorkorderCreateResponse Create(WorkorderCreateRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        {
            var authentication = AuthenticationService.GetState(
                request, ipAddress, headers);
            if (!authentication.Success)
                return new WorkorderCreateResponse()
                {
                    ErrorAuthentication = true
                };

            if (authentication.DbCurrentCompany == null)
                throw new Exception("Current company not chosen or doesn't exist, please create a company or select one.");

            var dbworkorder = WorkorderConverter.Create(request.Workorder, authentication.DbCurrentCompany, db);
            db.Workorders.Add(dbworkorder);
            db.SaveChanges();

            return new WorkorderCreateResponse()
            {
                Success = true,
                State = authentication,
                Workorder = WorkorderConverter.Create(dbworkorder)
            };
        }

        [TsServiceMethod("Workorder", "Read")]
        public WorkorderReadResponse Read(WorkorderReadRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        {
            var authentication = AuthenticationService.GetState(
                request, ipAddress, headers);
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
                .FirstOrDefault(a =>
                    a.CompanyId == authentication.DbCurrentCompany.id && 
                    a.id == request.WorkorderId);
            if (dbworkorder == null)
                return new WorkorderReadResponse()
                {
                    ErrorItemNotFound = true,
                    State = authentication
                };

            return new WorkorderReadResponse()
            {
                Success = true,
                State = authentication,
                Workorder = WorkorderConverter.Create(dbworkorder)
            };
        }

        [TsServiceMethod("Workorder", "Update")]
        public WorkorderUpdateResponse Update(WorkorderUpdateRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        {
            var authentication = AuthenticationService.GetState(
                request, ipAddress, headers);
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
                .FirstOrDefault(a =>
                    a.CompanyId == authentication.DbCurrentCompany.id &&
                    a.id == request.Workorder.id);
            if (dbworkorder == null)
                return new WorkorderUpdateResponse()
                {
                    ErrorItemNotFound = true,
                    State = authentication
                };

            if (WorkorderConverter.Copy(request.Workorder, dbworkorder, authentication.DbCurrentCompany, db))
                db.SaveChanges();

            return new WorkorderUpdateResponse()
            {
                Success = true,
                State = authentication,
                Workorder = WorkorderConverter.Create(dbworkorder)
            };
        }

        [TsServiceMethod("Workorder", "Delete")]
        public WorkorderDeleteResponse Delete(WorkorderDeleteRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        {
            var authentication = AuthenticationService.GetState(
                request, ipAddress, headers);
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
                .FirstOrDefault(a =>
                    a.CompanyId == authentication.DbCurrentCompany.id && 
                    a.id == request.WorkorderId);
            if (dbworkorder == null)
                return new WorkorderDeleteResponse()
                {
                    ErrorItemNotFound = true,
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
                request, ipAddress, headers);
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
                .Select(a => WorkorderConverter.Create(a))
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
