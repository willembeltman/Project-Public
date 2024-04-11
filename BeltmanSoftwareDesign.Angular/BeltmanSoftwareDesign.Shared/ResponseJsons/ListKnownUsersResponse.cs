using BeltmanSoftwareDesign.Shared.Jsons;

namespace BeltmanSoftwareDesign.Shared.ResponseJsons
{
    public class ListKnownUsersResponse : Response
    {
        public User[]? Users {  get; set; }
    }
}
