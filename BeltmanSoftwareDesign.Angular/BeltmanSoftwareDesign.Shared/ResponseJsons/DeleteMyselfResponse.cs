using BeltmanSoftwareDesign.Shared.Jsons;

namespace BeltmanSoftwareDesign.Shared.ResponseJsons
{
    public class DeleteMyselfResponse : Response
    {
        public bool ErrorOnlyDeletesToYourselfAreAllowed { get; set; }
    }
}
