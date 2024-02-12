using BeltmanSoftwareDesign.Shared.Jsons;

namespace BeltmanSoftwareDesign.Shared.ResponseJsons
{
    public class UserCreateResponse : Response
    {
        public User? User {  get; set; }
        public bool ErrorUserNameAlreadyUsed { get; set; }
    }
}
