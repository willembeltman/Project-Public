using BeltmanSoftwareDesign.Shared.Jsons;

namespace BeltmanSoftwareDesign.Shared.RequestJsons
{
    public class UserCreateRequest : Request
    {
        public User? User { get; set; }
    }
}
