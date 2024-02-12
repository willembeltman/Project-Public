using BeltmanSoftwareDesign.Shared.Jsons;

namespace BeltmanSoftwareDesign.Shared.ResponseJsons
{
    public class UserDeleteResponse : Response
    {
        public bool ErrorOnlyDeletesToYourselfAreAllowed { get; set; }
    }
}
