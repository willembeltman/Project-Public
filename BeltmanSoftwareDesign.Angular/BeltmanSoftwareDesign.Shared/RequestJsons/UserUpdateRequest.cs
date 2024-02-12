using BeltmanSoftwareDesign.Shared.Jsons;

namespace BeltmanSoftwareDesign.Shared.RequestJsons
{
    public class UserUpdateRequest : Request
    {
        public User? User { get; set; }
    }
}
