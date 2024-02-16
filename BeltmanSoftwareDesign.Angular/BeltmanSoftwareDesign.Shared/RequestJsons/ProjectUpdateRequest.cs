using BeltmanSoftwareDesign.Shared.Jsons;

namespace BeltmanSoftwareDesign.Shared.RequestJsons
{
    public class ProjectUpdateRequest : Request
    {
        public Project? Project { get; set; }
    }
}
