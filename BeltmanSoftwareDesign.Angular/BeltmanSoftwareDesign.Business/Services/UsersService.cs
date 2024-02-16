using BeltmanSoftwareDesign.Business.Interfaces;
using BeltmanSoftwareDesign.Business.Models;
using BeltmanSoftwareDesign.Data;
using BeltmanSoftwareDesign.Data.Factories;
using BeltmanSoftwareDesign.Shared.Attributes;
using BeltmanSoftwareDesign.Shared.Jsons;
using BeltmanSoftwareDesign.Shared.RequestJsons;
using BeltmanSoftwareDesign.Shared.ResponseJsons;
using BeltmanSoftwareDesign.StorageBlob.Business.Interfaces;

namespace BeltmanSoftwareDesign.Business.Services
{
    public class UsersService : IUsersService
    {
        ApplicationDbContext db { get; }
        IStorageFileService StorageFileService { get; }
        IAuthenticationService AuthenticationService { get; }
        UserFactory UserFactory { get; }
        CompanyFactory CompanyFactory { get; }

        public UsersService(
            ApplicationDbContext db,
            IStorageFileService storageFileService,
            IAuthenticationService authenticationService)
        {
            this.db = db;
            StorageFileService = storageFileService;
            AuthenticationService = authenticationService;

            UserFactory = new UserFactory(storageFileService);
            CompanyFactory = new CompanyFactory(storageFileService);
        }


        [TsServiceMethod("Users", "SetCurrentCompany")]
        public SetCurrentCompanyResponse SetCurrentCompany(SetCurrentCompanyRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        {
            if (request == null)
                return new SetCurrentCompanyResponse()
                {
                    ErrorAuthentication = true
                };
            var state = AuthenticationService.GetState(
                request.BearerId, request.CurrentCompanyId, ipAddress, headers);
            if (!state.Success)
                return new SetCurrentCompanyResponse()
                {
                    ErrorAuthentication = true
                };

            // ===========================

            var dbcompany = db.Companies.FirstOrDefault(a =>
                a.id == request.CurrentCompanyId &&
                a.CompanyUsers.Any(a => a.UserId == state.User.id));
            if (dbcompany == null)
                return new SetCurrentCompanyResponse()
                {
                    CompanyNotFound = true
                };


            // Convert it back
            var company = CompanyFactory.Convert(dbcompany);

            // Set current company to 
            state.User.currentCompanyId = company.id;
            state.DbUser.CurrentCompanyId = dbcompany.id;
            state.DbUser.CurrentCompany = dbcompany;
            state.CurrentCompany = company;
            state.DbCurrentCompany = dbcompany;
            db.SaveChanges();

            return new SetCurrentCompanyResponse()
            {
                Success = true,
                State = state
            };
        }

        [TsServiceMethod("Users", "Read")]
        public UserReadResponse Read(UserReadRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        {
            if (request == null)
                return new UserReadResponse()
                {
                    ErrorAuthentication = true
                };
            if (ipAddress == null)
                return new UserReadResponse()
                {
                    ErrorAuthentication = true
                };
            if (headers == null)
                return new UserReadResponse()
                {
                    ErrorAuthentication = true
                };
            var state = AuthenticationService.GetState(
                request.BearerId, request.CurrentCompanyId, ipAddress, headers);
            if (!state.Success)
                return new UserReadResponse()
                {
                    ErrorAuthentication = true
                };

            // ===========================

            var knownusers = GetKnownUsers(state);

            var dbuser = knownusers
                .FirstOrDefault(a =>
                    a.Id == request.UserId);
            if (dbuser == null)
                return new UserReadResponse()
                {
                    ErrorItemNotFound = true
                };

            var user = UserFactory.Convert(dbuser);

            return new UserReadResponse()
            {
                Success = true,
                User = user,
                State = state
            };
        }

        [TsServiceMethod("Users", "Update")]
        public UserUpdateResponse Update(UserUpdateRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        {
            if (request == null)
                return new UserUpdateResponse()
                {
                    ErrorAuthentication = true
                };
            var state = AuthenticationService.GetState(
                request.BearerId, request.CurrentCompanyId, ipAddress, headers);
            if (!state.Success)
                return new UserUpdateResponse()
                {
                    ErrorAuthentication = true
                };

            // ===========================

            if (request.User.id != state.User.id)
                return new UserUpdateResponse()
                {
                    ErrorOnlyUpdatesToYourselfAreAllowed = true
                };

            // Convert it to db item
            if (UserFactory.Copy(request.User, state.DbUser) == true)
                db.SaveChanges();

            // Convert it back
            var user = UserFactory.Convert(state.DbUser);

            // Zichzelf ook update in de json
            state.User = user;
            return new UserUpdateResponse()
            {
                Success = true,
                User = user,
                State = state
            };
        }

        [TsServiceMethod("Users", "Delete")]
        public UserDeleteResponse Delete(UserDeleteRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        {
            if (request == null)
                return new UserDeleteResponse()
                {
                    ErrorAuthentication = true
                };
            var state = AuthenticationService.GetState(
                request.BearerId, request.CurrentCompanyId, ipAddress, headers);
            if (!state.Success)
                return new UserDeleteResponse()
                {
                    ErrorAuthentication = true
                };

            // ===========================

            if (request.UserId != state.User.id)
                return new UserDeleteResponse()
                {
                    ErrorOnlyDeletesToYourselfAreAllowed = true
                };

            db.Users.Remove(state.DbUser);
            db.SaveChanges();
            
            return new UserDeleteResponse()
            {
                Success = true,
                State = new State()
                {
                    
                }
            };
        }

        [TsServiceMethod("Users", "List")]
        public UserListResponse List(UserListRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        {
            if (request == null)
                return new UserListResponse()
                {
                    ErrorAuthentication = true
                };

            var state = AuthenticationService.GetState(
                request.BearerId, request.CurrentCompanyId, ipAddress, headers);
            if (!state.Success) 
                return new UserListResponse()
                {
                    ErrorAuthentication = true
                };

            var knownusers = GetKnownUsers(state);

            var list =
                knownusers
                    .Select(u => UserFactory.Convert(u))
                    .ToArray();

            return new UserListResponse()
            {
                Success = true,
                State = state,
                Users = list
            };
        }

        private List<Data.Entities.User?> GetKnownUsers(AuthenticationState state)
        {
            var loggedinuser = state.DbUser;
            var knownusers =
                db.CompanyUsers
                    .Where(a => a.UserId == loggedinuser.Id)
                    .Select(a => a.Company)
                    .SelectMany(c => c.CompanyUsers)
                    .Select(cu => cu.User)
                    .ToList();
            knownusers.Add(loggedinuser);
            knownusers = knownusers
                .GroupBy(a => a.Id)
                .Select(a => a.First())
                .ToList();
            return knownusers;
        }

        //[TsServiceMethod("Users", "Create")]
        //public UserCreateResponse Create(UserCreateRequest request, string? ipAddress, KeyValuePair<string, string?>[]? headers)
        //{
        //    if (request == null)
        //        return new UserCreateResponse()
        //        {
        //            ErrorAuthentication = true
        //        };

        //    var state = AuthenticationService.GetState(
        //        request.BearerId, request.CurrentCompanyId, ipAddress, headers);
        //    if (!state.Success)
        //        return new UserCreateResponse()
        //        {
        //            ErrorAuthentication = true
        //        };

        //    // ===========================

        //    var dbuser = db.Users.FirstOrDefault(a =>
        //        a.UserName == request.User.userName);
        //    if (dbuser != null)
        //        return new UserCreateResponse()
        //        {
        //            ErrorUserNameAlreadyUsed = true
        //        };

        //    // Convert it to db item
        //    dbuser = UserFactory.Convert(request.User);
        //    db.Users.Add(dbuser);
        //    db.SaveChanges();

        //    // Convert it back
        //    var user = UserFactory.Convert(dbuser);

        //    state.DbUser = dbuser;
        //    state.User = user;

        //    return new UserCreateResponse()
        //    {
        //        Success = true,
        //        User = user,
        //        State = state
        //    };
        //}
    }
}
