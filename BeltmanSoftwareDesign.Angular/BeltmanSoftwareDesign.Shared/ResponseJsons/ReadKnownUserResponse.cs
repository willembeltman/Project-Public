using BeltmanSoftwareDesign.Shared.Jsons;

namespace BeltmanSoftwareDesign.Shared.ResponseJsons
{
    public class ReadKnownUserResponse : Response
    {
        public User? User {  get; set; }
    }
}
