using BeltmanSoftwareDesign.Shared.Jsons;

namespace BeltmanSoftwareDesign.Shared.ResponseJsons
{
    public class UserListResponse : Response
    {
        public User[]? Users {  get; set; }
    }
}
