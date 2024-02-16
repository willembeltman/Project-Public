using BeltmanSoftwareDesign.Business.Interfaces;
using BeltmanSoftwareDesign.Data;
using BeltmanSoftwareDesign.Data.Factories;
using BeltmanSoftwareDesign.Shared.Attributes;
using BeltmanSoftwareDesign.Shared.RequestJsons;
using BeltmanSoftwareDesign.Shared.ResponseJsons;
using BeltmanSoftwareDesign.StorageBlob.Business.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BeltmanSoftwareDesign.Business.Services
{
    public class CompaniesService : ICompaniesService
    {
        ApplicationDbContext db { get; }
        IStorageFileService StorageFileService { get; }
        IAuthenticationService AuthenticationService { get; }
        CompanyFactory CompanyFactory { get; }

        public CompaniesService(
            ApplicationDbContext db,
            IStorageFileService storageFileService,
            IAuthenticationService authenticationService)
        {
            this.db = db;
            StorageFileService = storageFileService;
            AuthenticationService = authenticationService;

            CompanyFactory = new CompanyFactory(storageFileService);
        }

        [TsServiceMethod("Companies", "Create")]
        public CompanyCreateResponse Create(CompanyCreateRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        {
            if (request == null)
                return new CompanyCreateResponse()
                {
                    ErrorAuthentication = true
                };

            var state = AuthenticationService.GetState(
                request.BearerId, request.CurrentCompanyId, ipAddress, headers);
            if (!state.Success)
                return new CompanyCreateResponse()
                {
                    ErrorAuthentication = true
                };

            // ===========================

            var dbcompany = db.Companies.FirstOrDefault(a =>
                a.Name == request.Company.Name &&
                a.CompanyUsers.Any(a => a.UserId == state.User.id));
            if (dbcompany != null)
                return new CompanyCreateResponse()
                {
                    ErrorCompanyNameAlreadyUsed = true
                };

            // Convert it to db item
            dbcompany = CompanyFactory.Convert(request.Company);
            db.Companies.Add(dbcompany);
            db.SaveChanges();

            var dbcompanyuser = new Data.Entities.CompanyUser()
            {
                Company = dbcompany,
                CompanyId = dbcompany.id,
                User = state.DbUser,
                UserId = state.DbUser.Id,
                Eigenaar = true,
                Admin = true,
                Actief = true,
            };
            db.CompanyUsers.Add(dbcompanyuser);
            db.SaveChanges();

            // Convert it back
            var company = CompanyFactory.Convert(dbcompany);

            state.DbUser.CurrentCompany = dbcompany;
            state.DbUser.CurrentCompanyId = dbcompany.id;
            state.User.currentCompanyId = dbcompany.id;
            state.DbCurrentCompany = dbcompany;
            state.CurrentCompany = company; 
            db.SaveChanges();

            return new CompanyCreateResponse()
            {
                Success = true,
                Company = company,
                State = state
            };
        }

        [TsServiceMethod("Companies", "Read")]
        public CompanyReadResponse Read(CompanyReadRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        {
            if (request == null)
                return new CompanyReadResponse()
                {
                    ErrorAuthentication = true
                };
            if (ipAddress == null)
                return new CompanyReadResponse()
                {
                    ErrorAuthentication = true
                };
            if (headers == null)
                return new CompanyReadResponse()
                {
                    ErrorAuthentication = true
                };
            var state = AuthenticationService.GetState(
                request.BearerId, request.CurrentCompanyId, ipAddress, headers);
            if (!state.Success)
                return new CompanyReadResponse()
                {
                    ErrorAuthentication = true
                };

            // ===========================

            var dbcompany = db.Companies
                .Include(a => a.Country)
                .FirstOrDefault(a =>
                    a.id == request.CompanyId &&
                    a.CompanyUsers.Any(a => a.UserId == state.User.id));
            if (dbcompany == null)
                return new CompanyReadResponse()
                {
                    CompanyNotFound = true
                };

            // Convert it back
            var company = CompanyFactory.Convert(dbcompany);

            //// Set current company to 
            //state.User.currentCompanyId = company.Id;
            //state.DbUser.CurrentCompanyId = dbcompany.Id;
            //state.DbUser.CurrentCompany = dbcompany;
            //state.CurrentCompany = company;
            //state.DbCurrentCompany = dbcompany;
            //db.SaveChanges();

            return new CompanyReadResponse()
            {
                Success = true,
                Company = company,
                State = state
            };
        }

        [TsServiceMethod("Companies", "Update")]
        public CompanyUpdateResponse Update(CompanyUpdateRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        {
            if (request == null)
                return new CompanyUpdateResponse()
                {
                    ErrorAuthentication = true
                };
            var state = AuthenticationService.GetState(
                request.BearerId, request.CurrentCompanyId, ipAddress, headers);
            if (!state.Success)
                return new CompanyUpdateResponse()
                {
                    ErrorAuthentication = true
                };

            // ===========================

            var dbcompany = db.Companies.FirstOrDefault(a =>
                a.id == request.Company.id &&
                a.CompanyUsers.Any(a => a.UserId == state.User.id));
            if (dbcompany == null)
                return new CompanyUpdateResponse()
                {
                    ErrorItemNotFound = true
                };

            // Convert it to db item
            if (CompanyFactory.Copy(request.Company, dbcompany) == true)
                db.SaveChanges();

            // Convert it back
            var company = CompanyFactory.Convert(dbcompany);

            // Set current company to 
            state.User.currentCompanyId = company.id;
            state.DbUser.CurrentCompanyId = dbcompany.id;
            state.DbUser.CurrentCompany = dbcompany;
            state.CurrentCompany = company;
            state.DbCurrentCompany = dbcompany;
            db.SaveChanges();

            return new CompanyUpdateResponse()
            {
                Success = true,
                Company = company,
                State = state
            };
        }

        [TsServiceMethod("Companies", "Delete")]
        public CompanyDeleteResponse Delete(CompanyDeleteRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        {
            if (request == null)
                return new CompanyDeleteResponse()
                {
                    ErrorAuthentication = true
                };
            var state = AuthenticationService.GetState(
                request.BearerId, request.CurrentCompanyId, ipAddress, headers);
            if (!state.Success)
                return new CompanyDeleteResponse()
                {
                    ErrorAuthentication = true
                };

            // ===========================

            var dbcompany = db.Companies.FirstOrDefault(a =>
                a.id == request.CompanyId &&
                a.CompanyUsers.Any(b => b.UserId == state.User.id && (b.Admin || b.Eigenaar)));
            if (dbcompany == null)
                return new CompanyDeleteResponse()
                {
                    ErrorItemNotFound = true
                };


            var userscurrentcompanywillbedeleted = 
                state.DbUser.CurrentCompanyId == dbcompany.id;

            if (userscurrentcompanywillbedeleted)
            {
                state.DbUser.CurrentCompany = null;
                state.DbCurrentCompany = null;
                state.CurrentCompany = null;
                state.User.currentCompanyId = null;
                state.DbUser.CurrentCompanyId = null;
            }

            db.Companies.Remove(dbcompany);
            db.SaveChanges();

            return new CompanyDeleteResponse()
            {
                Success = true,
                State = state                
            };
        }

        [TsServiceMethod("Companies", "List")]
        public CompanyListResponse List(CompanyListRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        {
            if (request == null)
                return new CompanyListResponse()
                {
                    ErrorAuthentication = true
                };

            var state = AuthenticationService.GetState(
                request.BearerId, request.CurrentCompanyId, ipAddress, headers);
            if (!state.Success) 
                return new CompanyListResponse()
                {
                    ErrorAuthentication = true
                };

            var list = db.Companies
                .Where(a => a.CompanyUsers.Any(a => a.UserId == state.User.id))
                .Select(a => CompanyFactory.Convert(a))
                .ToArray();

            return new CompanyListResponse()
            {
                Success = true,
                State = state,
                Companies = list
            };
        }
    }
}
