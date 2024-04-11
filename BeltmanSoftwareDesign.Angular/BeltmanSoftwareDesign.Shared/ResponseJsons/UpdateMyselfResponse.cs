using BeltmanSoftwareDesign.Shared.Jsons;

namespace BeltmanSoftwareDesign.Shared.ResponseJsons
{
    public class UpdateMyselfResponse : Response
    {
        public User? User {  get; set; }
        public bool ErrorOnlyUpdatesToYourselfAreAllowed { get; set; }
    }
}
