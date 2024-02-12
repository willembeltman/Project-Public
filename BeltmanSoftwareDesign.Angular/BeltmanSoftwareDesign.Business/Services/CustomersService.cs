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
    public class CustomersService : ICustomersService
    {
        ApplicationDbContext db { get; }
        IStorageFileService StorageFileService { get; }
        IAuthenticationService AuthenticationService { get; }
        CustomerFactory CustomerFactory { get; }

        public CustomersService(
            ApplicationDbContext db,
            IStorageFileService storageFileService,
            IAuthenticationService authenticationService)
        {
            this.db = db;
            StorageFileService = storageFileService;
            AuthenticationService = authenticationService;
            CustomerFactory = new CustomerFactory();
        }

        [TsServiceMethod("Customers", "Create")]
        public CustomerCreateResponse Create(CustomerCreateRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        {
            if (ipAddress == null)
                return new CustomerCreateResponse()
                {
                    ErrorAuthentication = true
                };

            var authentication = AuthenticationService.GetState(
                request.BearerId, request.CurrentCompanyId, ipAddress, headers);
            if (!authentication.Success)
                return new CustomerCreateResponse()
                {
                    ErrorAuthentication = true
                };

            if (authentication.DbCurrentCompany == null)
                throw new Exception("Current company not chosen or doesn't exist, please create a company or select one.");

            if (request.Customer.CompanyId == 0)
                request.Customer.CompanyId = authentication.DbCurrentCompany.Id;

            if (request.Customer.CompanyId != authentication.DbCurrentCompany.Id)
                return new CustomerCreateResponse()
                {
                    ErrorWrongCompany = true
                };

            var dbcustomer = CustomerFactory.Convert(request.Customer);
            db.Customers.Add(dbcustomer);
            db.SaveChanges();

            return new CustomerCreateResponse()
            {
                Success = true,
                State = authentication,
                Customer = CustomerFactory.Convert(dbcustomer)
            };
        }

        [TsServiceMethod("Customers", "Read")]
        public CustomerReadResponse Read(CustomerReadRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        {
            if (ipAddress == null)
                return new CustomerReadResponse()
                {
                    ErrorAuthentication = true
                };

            var authentication = AuthenticationService.GetState(
                request.BearerId, request.CurrentCompanyId, ipAddress, headers);
            if (!authentication.Success)
                return new CustomerReadResponse()
                {
                    ErrorAuthentication = true
                };

            if (authentication.DbCurrentCompany == null)
                throw new Exception("Current company not chosen or doesn't exist, please create a company or select one.");

            var dbcustomer = db.Customers.FirstOrDefault(a => a.id == request.CustomerId);
            if (dbcustomer == null)
                return new CustomerReadResponse()
                {
                    ErrorItemNotFound = true
                };

            if (dbcustomer.CompanyId != authentication.DbCurrentCompany.Id)
                return new CustomerReadResponse()
                {
                    ErrorWrongCompany = true
                };

            return new CustomerReadResponse()
            {
                Success = true,
                State = authentication,
                Customer = CustomerFactory.Convert(dbcustomer)
            };
        }

        [TsServiceMethod("Customers", "Update")]
        public CustomerUpdateResponse Update(CustomerUpdateRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        {
            if (ipAddress == null)
                return new CustomerUpdateResponse()
                {
                    ErrorAuthentication = true
                };

            var authentication = AuthenticationService.GetState(
                request.BearerId, request.CurrentCompanyId, ipAddress, headers);
            if (!authentication.Success)
                return new CustomerUpdateResponse()
                {
                    ErrorAuthentication = true
                };

            if (authentication.DbCurrentCompany == null)
                throw new Exception("Current company not chosen or doesn't exist, please create a company or select one.");

            var dbcustomer = db.Customers.FirstOrDefault(a => a.id == request.Customer.id);
            if (dbcustomer == null)
                return new CustomerUpdateResponse()
                {
                    ErrorItemNotFound = true,
                };

            if (dbcustomer.CompanyId != authentication.DbCurrentCompany.Id)
                return new CustomerUpdateResponse()
                {
                    ErrorWrongCompany = true,
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

        [TsServiceMethod("Customers", "Delete")]
        public CustomerDeleteResponse Delete(CustomerDeleteRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        {
            if (ipAddress == null)
                return new CustomerDeleteResponse()
                {
                    ErrorAuthentication = true
                };

            var authentication = AuthenticationService.GetState(
                request.BearerId, request.CurrentCompanyId, ipAddress, headers);
            if (!authentication.Success)
                return new CustomerDeleteResponse()
                {
                    ErrorAuthentication = true
                };

            if (authentication.DbCurrentCompany == null)
                throw new Exception("Current company not chosen or doesn't exist, please create a company or select one.");

            var dbcustomer = db.Customers.FirstOrDefault(a => a.id == request.CustomerId);
            if (dbcustomer == null)
                return new CustomerDeleteResponse()
                {
                    ErrorItemNotFound = true,
                    State = authentication
                };

            if (dbcustomer.CompanyId != authentication.DbCurrentCompany.Id)
                return new CustomerDeleteResponse()
                {
                    ErrorWrongCompany = true,
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

        [TsServiceMethod("Customers", "List")]
        public CustomerListResponse List(CustomerListRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        {
            if (ipAddress == null)
                return new CustomerListResponse()
                {
                    ErrorAuthentication = true
                };

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
                .Where(a => a.CompanyId == authentication.DbCurrentCompany.Id)
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
