using BeltmanSoftwareDesign.Shared.Attributes;

namespace BeltmanSoftwareDesign.Shared.RequestJsons
{
    [TsHidden]
    public class Request
    {
        public string? BearerId { get; set; }
        public long? CurrentCompanyId { get; set; }
    }
}
