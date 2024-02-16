using BeltmanSoftwareDesign.Shared.Jsons;

namespace BeltmanSoftwareDesign.Shared.ResponseJsons
{
    public class ProjectListResponse : Response
    {
        public Project[]? Projects {  get; set; }
    }
}
