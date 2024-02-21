using BeltmanSoftwareDesign.Shared.Attributes;
using BeltmanSoftwareDesign.Shared.Jsons;

namespace BeltmanSoftwareDesign.Shared.ResponseJsons
{
    [TsHidden]
    public class Response
    {
        public bool Success { get; set; }
        public bool ErrorAuthentication { get; set; }
        public bool ErrorItemNotFound { get; set; }
        
        public State State { get; set; } = new State();
    }
}