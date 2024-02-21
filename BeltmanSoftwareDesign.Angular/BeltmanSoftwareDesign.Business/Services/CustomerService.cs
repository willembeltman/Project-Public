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
    public class CustomerService : ICustomerService
    {
        ApplicationDbContext db { get; }
        IStorageFileService StorageFileService { get; }
        IAuthenticationService AuthenticationService { get; }
        CustomerFactory CustomerFactory { get; }

        public CustomerService(
            ApplicationDbContext db,
            IStorageFileService storageFileService,
            IAuthenticationService authenticationService)
        {
            this.db = db;
            StorageFileService = storageFileService;
            AuthenticationService = authenticationService;
            CustomerFactory = new CustomerFactory();
        }

        [TsServiceMethod("Customer", "Create")]
        public CustomerCreateResponse Create(CustomerCreateRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        {
            var authentication = AuthenticationService.GetState(
                request.BearerId, request.CurrentCompanyId, ipAddress, headers);
            if (!authentication.Success)
                return new CustomerCreateResponse()
                {
                    ErrorAuthentication = true
                };

            if (authentication.DbCurrentCompany == null)
                throw new Exception("Current company not chosen or doesn't exist, please create a company or select one.");

            var dbcustomer = CustomerFactory.Convert(request.Customer);

            dbcustomer.Company = authentication.DbCurrentCompany;
            dbcustomer.CompanyId = authentication.DbCurrentCompany.id;

            db.Customers.Add(dbcustomer);
            db.SaveChanges();

            return new CustomerCreateResponse()
            {
                Success = true,
                State = authentication,
                Customer = CustomerFactory.Convert(dbcustomer)
            };
        }

        [TsServiceMethod("Customer", "Read")]
        public CustomerReadResponse Read(CustomerReadRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        {
            var authentication = AuthenticationService.GetState(
                request.BearerId, request.CurrentCompanyId, ipAddress, headers);
            if (!authentication.Success)
                return new CustomerReadResponse()
                {
                    ErrorAuthentication = true
                };

            if (authentication.DbCurrentCompany == null)
                throw new Exception("Current company not chosen or doesn't exist, please create a company or select one.");

            var dbcustomer = db.Customers
                .Include(a => a.Country)
                .Include(a => a.Workorders)
                .Include(a => a.Projects)
                .Include(a => a.Invoices)
                .Include(a => a.Expenses)
                .Include(a => a.Documents)
                .FirstOrDefault(a => 
                    a.CompanyId == authentication.DbCurrentCompany.id &&
                    a.id == request.CustomerId);

            if (dbcustomer == null)
                return new CustomerReadResponse()
                {
                    ErrorItemNotFound = true
                };

            return new CustomerReadResponse()
            {
                Success = true,
                State = authentication,
                Customer = CustomerFactory.Convert(dbcustomer)
            };
        }

        [TsServiceMethod("Customer", "Update")]
        public CustomerUpdateResponse Update(CustomerUpdateRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        {
            var authentication = AuthenticationService.GetState(
                request.BearerId, request.CurrentCompanyId, ipAddress, headers);
            if (!authentication.Success)
                return new CustomerUpdateResponse()
                {
                    ErrorAuthentication = true
                };

            if (authentication.DbCurrentCompany == null)
                throw new Exception("Current company not chosen or doesn't exist, please create a company or select one.");

            var dbcustomer = db.Customers
                .Include(a => a.Country)
                .Include(a => a.Workorders)
                .Include(a => a.Projects)
                .Include(a => a.Invoices)
                .Include(a => a.Expenses)
                .Include(a => a.Documents)
                .FirstOrDefault(a =>
                    a.CompanyId == authentication.DbCurrentCompany.id &&
                    a.id == request.Customer.id);
            if (dbcustomer == null)
                return new CustomerUpdateResponse()
                {
                    ErrorItemNotFound = true,
                };

            if (CustomerFactory.Copy(request.Customer, dbcustomer))
                db.SaveChanges();

            return new CustomerUpdateResponse()
            {
                Success = true,
                State = authentication,
                Customer = CustomerFactory.Convert(dbcustomer)
            };
        }

        [TsServiceMethod("Customer", "Delete")]
        public CustomerDeleteResponse Delete(CustomerDeleteRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        {
            var authentication = AuthenticationService.GetState(
                request.BearerId, request.CurrentCompanyId, ipAddress, headers);
            if (!authentication.Success)
                return new CustomerDeleteResponse()
                {
                    ErrorAuthentication = true
                };

            if (authentication.DbCurrentCompany == null)
                throw new Exception("Current company not chosen or doesn't exist, please create a company or select one.");

            var dbcustomer = db.Customers
                .Include(a => a.Country)
                .Include(a => a.Workorders)
                .Include(a => a.Projects)
                .Include(a => a.Invoices)
                .Include(a => a.Expenses)
                .Include(a => a.Documents)
                .FirstOrDefault(a =>
                    a.CompanyId == authentication.DbCurrentCompany.id &&
                    a.id == request.CustomerId);

            if (dbcustomer == null)
                return new CustomerDeleteResponse()
                {
                    ErrorItemNotFound = true,
                    State = authentication
                };

            db.Customers.Remove(dbcustomer);
            db.SaveChanges();

            return new CustomerDeleteResponse()
            {
                Success = true,
                State = authentication
            };
        }

        [TsServiceMethod("Customer", "List")]
        public CustomerListResponse List(CustomerListRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        {
            var authentication = AuthenticationService.GetState(
                request.BearerId, request.CurrentCompanyId, ipAddress, headers);
            if (!authentication.Success)
                return new CustomerListResponse()
                {
                    ErrorAuthentication = true
                };

            if (authentication.DbCurrentCompany == null)
                throw new Exception("Current company not chosen or doesn't exist, please create a company or select one.");

            var list = db.Customers
                .Include(a => a.Country)
                .Include(a => a.Workorders)
                .Include(a => a.Projects)
                .Include(a => a.Invoices)
                .Include(a => a.Expenses)
                .Include(a => a.Documents)
                .Where(a => 
                    a.CompanyId == authentication.DbCurrentCompany.id)
                .Select(a => CustomerFactory.Convert(a))
                .ToArray();

            return new CustomerListResponse()
            {
                Success = true,
                State = authentication,
                Customers = list
            };
        }
    }
}
