using BeltmanSoftwareDesign.Shared.Jsons;

namespace BeltmanSoftwareDesign.Shared.RequestJsons
{
    public class UpdateMyselfRequest : Request
    {
        public User? User { get; set; }
    }
}
