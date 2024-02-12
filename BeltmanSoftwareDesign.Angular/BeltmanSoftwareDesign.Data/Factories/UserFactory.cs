
using BeltmanSoftwareDesign.StorageBlob.Business.Interfaces;

namespace BeltmanSoftwareDesign.Data.Factories
{
    public class UserFactory
    {
        //public Shared.Jsons.User Convert(Data.Entities.User a)
        //{
        //    return new Shared.Jsons.User()
        //    {
        //        id = a.Id,  
        //        email = a.Email,
        //        userName = a.UserName,
        //        phoneNumber = a.PhoneNumber,
        //        currentCompanyId = a.CurrentCompanyId,
        //    };
        //}

        public UserFactory(IStorageFileService storageFileService)
        {
        }

        public Shared.Jsons.User? Convert(Entities.User? a)
        {
            if (a == null) return null;
            return new Shared.Jsons.User
            {
                id = a.Id,
                userName = a.UserName,
                currentCompanyId = a.CurrentCompanyId,
                phoneNumber = a.PhoneNumber,
                email = a.Email
            };
        }
        public Entities.User? Convert(Shared.Jsons.User? a)
        {
            if (a == null) return null;
            return new Entities.User
            {
                Id = a.id,
                UserName = a.userName,
                CurrentCompanyId = a.currentCompanyId,
                PhoneNumber = a.phoneNumber,
                Email = a.email
            };
        }

        public bool? Copy(Shared.Jsons.User? source, Entities.User dest)
        {
            if (source == null || dest == null)
            {
                return null;
            }

            var dirty = false;
            //if (dest.Id != source.id) { dest.Id = source.id; dirty = true; }
            if (dest.UserName != source.userName) { dest.UserName = source.userName; dirty = true; }
            if (dest.CurrentCompanyId != source.currentCompanyId) { dest.CurrentCompanyId = source.currentCompanyId; dirty = true; }
            if (dest.PhoneNumber != source.phoneNumber) { dest.PhoneNumber = source.phoneNumber; dirty = true; }
            //if (dest.Email != source.email) { dest.Email = source.email; dirty = true; }
            return dirty;
        }
    }
}