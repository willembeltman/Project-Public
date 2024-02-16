using BeltmanSoftwareDesign.Business.Interfaces;
using BeltmanSoftwareDesign.Business.Models;
using BeltmanSoftwareDesign.Data;
using BeltmanSoftwareDesign.Data.Factories;
using BeltmanSoftwareDesign.Shared.Attributes;
using BeltmanSoftwareDesign.Shared.RequestJsons;
using BeltmanSoftwareDesign.Shared.ResponseJsons;
using BeltmanSoftwareDesign.StorageBlob.Business.Interfaces;

namespace BeltmanSoftwareDesign.Business.Services
{
    public class WorkordersService : IWorkordersService
    {
        ApplicationDbContext db { get; }
        IStorageFileService StorageFileService { get; }
        IAuthenticationService AuthenticationService { get; }
        WorkorderFactory WorkorderFactory { get; }

        public WorkordersService(
            ApplicationDbContext db,
            IStorageFileService storageFileService,
            IAuthenticationService authenticationService)
        {
            this.db = db;
            StorageFileService = storageFileService;
            AuthenticationService = authenticationService;
            WorkorderFactory = new WorkorderFactory(storageFileService);
        }

        [TsServiceMethod("Workorders", "Create")]
        public WorkorderCreateResponse Create(WorkorderCreateRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        {
            if (ipAddress == null)
                return new WorkorderCreateResponse()
                {
                    ErrorAuthentication = true
                };

            var authentication = AuthenticationService.GetState(
                request.BearerId, request.CurrentCompanyId, ipAddress, headers);
            if (!authentication.Success)
                return new WorkorderCreateResponse()
                {
                    ErrorAuthentication = true
                };

            if (authentication.DbCurrentCompany == null)
                throw new Exception("Current company not chosen or doesn't exist, please create a company or select one.");

            if (request.Workorder.CompanyId == 0)
                request.Workorder.CompanyId = authentication.DbCurrentCompany.id;

            if (request.Workorder.CompanyId != authentication.DbCurrentCompany.id)
                return new WorkorderCreateResponse()
                {
                    ErrorWrongCompany = true
                };

            var dbworkorder = WorkorderFactory.Convert(request.Workorder);
            db.Workorders.Add(dbworkorder);
            db.SaveChanges();

            return new WorkorderCreateResponse()
            {
                Success = true,
                State = authentication,
                Workorder = WorkorderFactory.Convert(dbworkorder)
            };
        }

        [TsServiceMethod("Workorders", "Read")]
        public WorkorderReadResponse Read(WorkorderReadRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        {
            if (ipAddress == null)
                return new WorkorderReadResponse()
                {
                    ErrorAuthentication = true
                };

            var authentication = AuthenticationService.GetState(
                request.BearerId, request.CurrentCompanyId, ipAddress, headers);
            if (!authentication.Success)
                return new WorkorderReadResponse()
                {
                    ErrorAuthentication = true
                };

            if (authentication.DbCurrentCompany == null)
                throw new Exception("Current company not chosen or doesn't exist, please create a company or select one.");

            var dbworkorder = db.Workorders.FirstOrDefault(a => a.id == request.WorkorderId);
            if (dbworkorder == null)
                return new WorkorderReadResponse()
                {
                    ErrorItemNotFound = true
                };

            if (dbworkorder.CompanyId != authentication.DbCurrentCompany.id)
                return new WorkorderReadResponse()
                {
                    ErrorWrongCompany = true
                };

            return new WorkorderReadResponse()
            {
                Success = true,
                State = authentication,
                Workorder = WorkorderFactory.Convert(dbworkorder)
            };
        }

        [TsServiceMethod("Workorders", "Update")]
        public WorkorderUpdateResponse Update(WorkorderUpdateRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        {
            if (ipAddress == null)
                return new WorkorderUpdateResponse()
                {
                    ErrorAuthentication = true
                };

            var authentication = AuthenticationService.GetState(
                request.BearerId, request.CurrentCompanyId, ipAddress, headers);
            if (!authentication.Success)
                return new WorkorderUpdateResponse()
                {
                    ErrorAuthentication = true
                };

            if (authentication.DbCurrentCompany == null)
                throw new Exception("Current company not chosen or doesn't exist, please create a company or select one.");

            var dbworkorder = db.Workorders.FirstOrDefault(a => a.id == request.Workorder.id);
            if (dbworkorder == null)
                return new WorkorderUpdateResponse()
                {
                    ErrorItemNotFound = true,
                };

            if (dbworkorder.CompanyId != authentication.DbCurrentCompany.id)
                return new WorkorderUpdateResponse()
                {
                    ErrorWrongCompany = true,
                };

            dbworkorder.AmountUur = request.Workorder.AmountUur;
            dbworkorder.CustomerId = request.Workorder.CustomerId;
            dbworkorder.Description = request.Workorder.Description;
            dbworkorder.ProjectId = request.Workorder.ProjectId;
            dbworkorder.Start = request.Workorder.Start;
            dbworkorder.Stop = request.Workorder.Stop;
            db.SaveChanges();

            return new WorkorderUpdateResponse()
            {
                Success = true,
                State = authentication,
                Workorder = WorkorderFactory.Convert(dbworkorder)
            };
        }

        [TsServiceMethod("Workorders", "Delete")]
        public WorkorderDeleteResponse Delete(WorkorderDeleteRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        {
            if (ipAddress == null)
                return new WorkorderDeleteResponse()
                {
                    ErrorAuthentication = true
                };

            var authentication = AuthenticationService.GetState(
                request.BearerId, request.CurrentCompanyId, ipAddress, headers);
            if (!authentication.Success)
                return new WorkorderDeleteResponse()
                {
                    ErrorAuthentication = true
                };

            if (authentication.DbCurrentCompany == null)
                throw new Exception("Current company not chosen or doesn't exist, please create a company or select one.");

            var dbworkorder = db.Workorders.FirstOrDefault(a => a.id == request.WorkorderId);
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

        [TsServiceMethod("Workorders", "List")]
        public WorkorderListResponse List(WorkorderListRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        {
            if (ipAddress == null)
                return new WorkorderListResponse()
                {
                    ErrorAuthentication = true
                };

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
