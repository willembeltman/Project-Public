using BeltmanSoftwareDesign.Shared.Jsons;

namespace BeltmanSoftwareDesign.Shared.ResponseJsons
{
    public class UserReadResponse : Response
    {
        public User? User {  get; set; }
    }
}
