using gAPI.Attributes;
using gAPI.Dtos;

namespace UwvLlm.Shared.Dtos;

[GenerateSerializer]
[IsStateDto]
public class State : AuthStateDto
{
}
