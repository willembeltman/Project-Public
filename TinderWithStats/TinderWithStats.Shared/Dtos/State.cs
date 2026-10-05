using gAPI.Core.Dtos;

namespace TinderWithStats.Shared.Dtos;

public class State : AuthStateDto
{
    public bool IsAdmin { get; set; }
    public Guid? ProfileId { get; set; }
}