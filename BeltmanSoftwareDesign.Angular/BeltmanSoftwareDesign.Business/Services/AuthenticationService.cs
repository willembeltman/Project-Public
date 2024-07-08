using BeltmanSoftwareDesign.Business.Helpers;
using BeltmanSoftwareDesign.Business.Interfaces;
using BeltmanSoftwareDesign.Business.Models;
using BeltmanSoftwareDesign.Data;
using BeltmanSoftwareDesign.Data.Entities;
using BeltmanSoftwareDesign.Data.Converters;
using BeltmanSoftwareDesign.Shared.Attributes;
using BeltmanSoftwareDesign.Shared.RequestJsons;
using BeltmanSoftwareDesign.Shared.ResponseJsons;
using StorageBlob.Proxy.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BeltmanSoftwareDesign.Business.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        static int shorthoursago = -1;
        static int longhoursago = -72;

        ApplicationDbContext db { get; }
        IDateTimeService DateTime { get; }
        UserConverter UserConverter { get; }
        CompanyConverter CompanyConverter { get; }

        public AuthenticationService(
            ApplicationDbContext db, 
            IStorageFileService storageFileService, 
            IDateTimeService dateTimeService)
        {
            this.db = db;
            DateTime = dateTimeService;
            UserConverter = new UserConverter(storageFileService);
            CompanyConverter = new CompanyConverter(storageFileService);
        }

        [TsServiceMethod("Auth", "Login")]
        public LoginResponse Login(LoginRequest request, string? requestIpAddress, KeyValuePair<string, string?>[]? requestHeaders)
        {
            var shortago = DateTime.Now.AddHours(shorthoursago);
            var longago = DateTime.Now.AddHours(longhoursago);

            if (string.IsNullOrEmpty(requestIpAddress))
                return new LoginResponse()
                {
                    ErrorAuthentication = true
                };

            var email = request.Email;
            var password = request.Password;

            if (!EmailAddressHelper.IsEmailAddress(email))
                return new LoginResponse()
                {
                    ErrorEmailNotValid = true
                };
            if (string.IsNullOrEmpty(password))
                return new LoginResponse()
                {
                    AuthenticationError = true
                };

            var dbuser = db.Users.FirstOrDefault(a => a.Email == email);
            if (dbuser == null)
                return new LoginResponse()
                {
                    AuthenticationError = true // Security?
                };

            // Password correct?
            var passwordHash = StringHelper.HashString(password);
            if (dbuser.PasswordHash != passwordHash)
                return new LoginResponse()
                {
                    AuthenticationError = true // Security?
                };

            var clientDevice = GetClientDevice(requestHeaders);
            if (clientDevice == null)
                return new LoginResponse()
                {
                    AuthenticationError = true
                };

            var clientIpAddress = GetIpAddress(requestIpAddress);
            if (clientIpAddress == null)
                return new LoginResponse()
                {
                    AuthenticationError = true
                };

            var clientBearer = db.ClientBearers
                .OrderByDescending(a => a.Date)
                .FirstOrDefault(a =>
                    a.UserId == dbuser.Id &&
                    a.ClientIpAddressId == clientIpAddress.id &&
                    a.ClientDeviceId == clientDevice.id &&
                    a.Date > shortago) ;
            if (clientBearer == null)
            {
                // Automatisch vernieuwen
                clientBearer = CreateBearer(dbuser, clientDevice, clientIpAddress);
            }

            var user = UserConverter.Create(dbuser);
            if (user == null)
                return new LoginResponse()
                {
                    AuthenticationError = true
                };

            var dbcurrentcompany = db.Companies
                .Include(a => a.Country)
                .FirstOrDefault(a => 
                    a.id == user.currentCompanyId &&
                    a.CompanyUsers.Any(a => a.UserId == user.id));

            var currentcompany = CompanyConverter.Create(dbcurrentcompany);

            return new LoginResponse()
            {
                Success = true,
                State = new Shared.Jsons.State()
                {
                    User = user,
                    CurrentCompany = currentcompany,
                    BearerId = clientBearer.id,
                }
            };
        }

        [TsServiceMethod("Auth", "Register")]
        public RegisterResponse Register(RegisterRequest request, string? requestIpAddress, KeyValuePair<string, string?>[]? requestHeaders)
        {
            if (string.IsNullOrEmpty(requestIpAddress))
                return new RegisterResponse()
                {
                    ErrorAuthentication = true
                };

            var username = request.Username;
            var email = request.Email;
            var phoneNumber = request.PhoneNumber;
            var password = request.Password;

            // Add validation
            if (!EmailAddressHelper.IsEmailAddress(email))
                return new RegisterResponse()
                {
                    ErrorEmailNotValid = true
                };
            if (string.IsNullOrEmpty(username))
                return new RegisterResponse()
                {
                    ErrorUsernameEmpty = true
                };
            if (string.IsNullOrEmpty(password))
                return new RegisterResponse()
                {
                    ErrorPasswordEmpty = true
                };
            if (string.IsNullOrEmpty(phoneNumber))
                return new RegisterResponse()
                {
                    ErrorPhoneNumberEmpty = true
                };

            var usernameUser = db.Users.FirstOrDefault(a => a.UserName.ToLower() == username.ToLower());
            if (usernameUser != null)
                return new RegisterResponse()
                {
                    ErrorUsernameInUse = true
                };

            var emailUser = db.Users.FirstOrDefault(a => a.Email.ToLower() == email.ToLower());
            if (emailUser != null)
                return new RegisterResponse()
                {
                    ErrorEmailInUse = true
                };

            var dbuser = CreateUser(username, email, phoneNumber, password);
            if (dbuser == null)
                return new RegisterResponse()
                {
                    ErrorCouldNotCreateUser = true
                };

            var device = GetClientDevice(requestHeaders);
            if (device == null)
                return new RegisterResponse()
                {
                    ErrorCouldNotGetDevice = true
                };

            var ipAddress = GetIpAddress(requestIpAddress);
            if (ipAddress == null)
                return new RegisterResponse()
                {
                    ErrorEmailInUse = true
                };

            var bearer = CreateBearer(dbuser, device, ipAddress);
            if (bearer == null)
                return new RegisterResponse()
                {
                    ErrorCouldNotCreateBearer = true
                };

            var user = UserConverter.Create(dbuser);
            return new RegisterResponse()
            {
                Success = true,
                State = new Shared.Jsons.State()
                {
                    User = user,
                    BearerId = bearer.id,
                }
            };
        }

        public AuthenticationState GetState(Request request, string? requestIpAddress, KeyValuePair<string, string?>[]? requestHeaders)
        {
            if (requestIpAddress == null)
                return new AuthenticationState()
                {
                };

            if (requestHeaders == null)
                return new AuthenticationState()
                {
                };

            var shortago = DateTime.Now.AddHours(shorthoursago);
            var longago = DateTime.Now.AddHours(longhoursago);

            var clientDevice = GetClientDevice(requestHeaders);
            if (clientDevice == null)
                return new AuthenticationState()
                {
                };

            var clientIpAddress = GetIpAddress(requestIpAddress);
            if (clientIpAddress == null)
                return new AuthenticationState()
                {
                };

            var clientBearer = db.ClientBearers
                .OrderByDescending(a => a.Date)
                .FirstOrDefault(a => 
                    a.id == request.BearerId &&
                    a.Date > longago);
            if (clientBearer == null || clientBearer.UserId == null) 
                return new AuthenticationState()
                {
                };

            if (clientBearer.ClientDeviceId != clientDevice.id)
                return new AuthenticationState()
                {
                }; 
            
            if (clientBearer.Date < longago)
                return new AuthenticationState()
                {
                };

            // Get user from database
            var user = db.Users
                .FirstOrDefault(a => a.Id == clientBearer.UserId);
            if (user == null)
                return new AuthenticationState()
                {
                };

            if (clientBearer.ClientIpAddressId != clientIpAddress.id)
            {
                // Ip veranderd, toch automatisch vernieuwen
                clientBearer = CreateBearer(user, clientDevice, clientIpAddress);
            }

            if (clientBearer.Date < shortago && clientBearer.Date > longago)
            {
                // Automatisch vernieuwen
                clientBearer = CreateBearer(user, clientDevice, clientIpAddress);
            }

            // Get current company from database
            var currentcompany = db.Companies
                .Include(a => a.Country)
                .FirstOrDefault(a => 
                    a.CompanyUsers.Any(a => a.UserId == user.Id) &&
                    a.id == request.CurrentCompanyId);
       
            var userJson = UserConverter.Create(user);
            var companyJson = CompanyConverter.Create(currentcompany);

            return new AuthenticationState()
            {
                Success = true,

                User = userJson,
                CurrentCompany = companyJson,
                BearerId = clientBearer.id,

                DbUser = user,
                DbCurrentCompany = currentcompany,
                DbClientBearer = clientBearer,
                DbClientDevice = clientDevice,
                DbClientLocation = clientIpAddress,
            };
        }


        private ClientBearer CreateBearer(User user, ClientDevice clientDevice, ClientIpAddress clientIpAddress)
        {
            // Te oud, vernieuwen
            var bearerid = HashGeneratorHelper.GenerateCode(64);
            var bearer = new ClientBearer()
            {
                id = bearerid,
                ClientDevice = clientDevice,
                ClientDeviceId = clientDevice?.id,
                ClientIpAddress = clientIpAddress,
                ClientIpAddressId = clientIpAddress?.id,    
                User = user,
                UserId = user.Id,
            };
            db.ClientBearers.Add(bearer);
            db.SaveChanges();
            return bearer;
        }
        private User CreateUser(string username, string email, string phoneNumber, string password)
        {
            // Create user
            var passwordHash = StringHelper.HashString(password);
            var userid = HashGeneratorHelper.GenerateCode(64);
            var dbuser = new User()
            {
                Id = userid,
                UserName = username,
                Email = email,
                PhoneNumber = phoneNumber,
                PasswordHash = passwordHash,
            };
            db.Users.Add(dbuser);
            db.SaveChanges();
            return dbuser;
        }
        private ClientIpAddress GetIpAddress(string ipAddress)
        {
            var location = db.ClientLocations.FirstOrDefault(a => a.IpAddress == ipAddress);
            if (location == null)
            {
                location = new ClientIpAddress()
                {
                    IpAddress = ipAddress,
                };
                db.ClientLocations.Add(location);
                db.SaveChanges();
            }

            return location;
        }
        private ClientDevice? GetClientDevice(KeyValuePair<string, string?>[]? headers)
        {
            if (headers == null)
                return null;

            if (!headers.Any(a => a.Key.ToLower() == "useragent" || a.Key.ToLower() == "user-agent"))
                return null;

            var useragentheader = headers.First(a => a.Key.ToLower() == "useragent" || a.Key.ToLower() == "user-agent");
            var useragent = (useragentheader.Value ?? string.Empty).ToLower();

            if (useragent == null)
                return null;

            var deviceHash = StringHelper.HashString(useragent);
            var device = db.ClientDevices.FirstOrDefault(a => a.DeviceHash == deviceHash);
            if (device == null)
            {
                device = new ClientDevice()
                {
                    DeviceHash = deviceHash,
                    ClientDeviceProperties = new List<ClientDeviceProperty>()
                    {
                        new ClientDeviceProperty()
                        {
                            Name = "UserAgent",
                            Value = useragent
                        },
                    }
                };
                db.ClientDevices.Add(device);
                db.SaveChanges();
            }

            return device;
        }
    }
}
