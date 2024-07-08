using BeltmanSoftwareDesign.Business.Interfaces;
using BeltmanSoftwareDesign.Data;
using BeltmanSoftwareDesign.Data.Converters;
using BeltmanSoftwareDesign.Shared.Attributes;
using BeltmanSoftwareDesign.Shared.RequestJsons;
using BeltmanSoftwareDesign.Shared.ResponseJsons;
using StorageBlob.Proxy.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BeltmanSoftwareDesign.Business.Services
{
    public class ProjectService : IProjectService
    {
        ApplicationDbContext db { get; }
        IAuthenticationService AuthenticationService { get; }
        ProjectConverter ProjectConverter { get; }

        public ProjectService(
            ApplicationDbContext db,
            IAuthenticationService authenticationService)
        {
            this.db = db;
            AuthenticationService = authenticationService;
            ProjectConverter = new ProjectConverter();
        }

        [TsServiceMethod("Project", "Create")]
        public ProjectCreateResponse Create(ProjectCreateRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        {
            var authentication = AuthenticationService.GetState(
                request, ipAddress, headers);
            if (!authentication.Success)
                return new ProjectCreateResponse()
                {
                    ErrorAuthentication = true
                };

            if (authentication.DbCurrentCompany == null)
                throw new Exception("Current company not chosen or doesn't exist, please create a company or select one.");

            var dbproject = ProjectConverter.Create(request.Project);
            dbproject.CompanyId = authentication.DbCurrentCompany.id;
            db.Projects.Add(dbproject);
            db.SaveChanges();

            return new ProjectCreateResponse()
            {
                Success = true,
                State = authentication,
                Project = ProjectConverter.Create(dbproject)
            };
        }

        [TsServiceMethod("Project", "Read")]
        public ProjectReadResponse Read(ProjectReadRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        {
            var authentication = AuthenticationService.GetState(
                request, ipAddress, headers);
            if (!authentication.Success)
                return new ProjectReadResponse()
                {
                    ErrorAuthentication = true
                };

            if (authentication.DbCurrentCompany == null)
                throw new Exception("Current company not chosen or doesn't exist, please create a company or select one.");

            var dbproject = db.Projects
                .Include(a => a.Company)
                .Include(a => a.Documents)
                .Include(a => a.Expenses)
                .Include(a => a.Invoices)
                .Include(a => a.Workorders)
                .FirstOrDefault(a =>
                    a.CompanyId == authentication.DbCurrentCompany.id && 
                    a.id == request.ProjectId);
            if (dbproject == null)
                return new ProjectReadResponse()
                {
                    ErrorItemNotFound = true
                };

            return new ProjectReadResponse()
            {
                Success = true,
                State = authentication,
                Project = ProjectConverter.Create(dbproject)
            };
        }

        [TsServiceMethod("Project", "Update")]
        public ProjectUpdateResponse Update(ProjectUpdateRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        {
            var authentication = AuthenticationService.GetState(
                request, ipAddress, headers);
            if (!authentication.Success)
                return new ProjectUpdateResponse()
                {
                    ErrorAuthentication = true
                };

            if (authentication.DbCurrentCompany == null)
                throw new Exception("Current company not chosen or doesn't exist, please create a company or select one.");

            var dbproject = db.Projects
                .Include(a => a.Company)
                .Include(a => a.Documents)
                .Include(a => a.Expenses)
                .Include(a => a.Invoices)
                .Include(a => a.Workorders)
                .FirstOrDefault(a =>
                    a.CompanyId == authentication.DbCurrentCompany.id && 
                    a.id == request.Project.id);
            if (dbproject == null)
                return new ProjectUpdateResponse()
                {
                    ErrorItemNotFound = true,
                };

            if (ProjectConverter.Copy(request.Project, dbproject))
                db.SaveChanges();

            return new ProjectUpdateResponse()
            {
                Success = true,
                State = authentication,
                Project = ProjectConverter.Create(dbproject)
            };
        }

        [TsServiceMethod("Project", "Delete")]
        public ProjectDeleteResponse Delete(ProjectDeleteRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        {
            var authentication = AuthenticationService.GetState(
                request, ipAddress, headers);
            if (!authentication.Success)
                return new ProjectDeleteResponse()
                {
                    ErrorAuthentication = true
                };

            if (authentication.DbCurrentCompany == null)
                throw new Exception("Current company not chosen or doesn't exist, please create a company or select one.");

            var dbproject = db.Projects
                .Include(a => a.Company)
                .Include(a => a.Documents)
                .Include(a => a.Expenses)
                .Include(a => a.Invoices)
                .Include(a => a.Workorders)
                .FirstOrDefault(a =>
                    a.CompanyId == authentication.DbCurrentCompany.id && 
                    a.id == request.ProjectId);
            if (dbproject == null)
                return new ProjectDeleteResponse()
                {
                    ErrorItemNotFound = true,
                    State = authentication
                };

            db.Projects.Remove(dbproject);
            db.SaveChanges();

            return new ProjectDeleteResponse()
            {
                Success = true,
                State = authentication
            };
        }

        [TsServiceMethod("Project", "List")]
        public ProjectListResponse List(ProjectListRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        {
            var authentication = AuthenticationService.GetState(
                request, ipAddress, headers);
            if (!authentication.Success)
                return new ProjectListResponse()
                {
                    ErrorAuthentication = true
                };

            if (authentication.DbCurrentCompany == null)
                throw new Exception("Current company not chosen or doesn't exist, please create a company or select one.");

            var list = db.Projects
                .Include(a => a.Company)
                .Include(a => a.Documents)
                .Include(a => a.Expenses)
                .Include(a => a.Invoices)
                .Include(a => a.Workorders)
                .Where(a => a.CompanyId == authentication.DbCurrentCompany.id)
                .Select(a => ProjectConverter.Create(a))
                .ToArray();

            return new ProjectListResponse()
            {
                Success = true,
                State = authentication,
                Projects = list
            };
        }
    }
}
