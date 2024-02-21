namespace BeltmanSoftwareDesign.Shared.ResponseJsons
{
    public class LoginResponse : Response
    {
        public bool ErrorEmailNotValid { get; set; }
        public bool AuthenticationError { get; set; }
    }
}
