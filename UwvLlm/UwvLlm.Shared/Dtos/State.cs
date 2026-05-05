using gAPI.Core.Attributes;
using gAPI.Core.Dtos;

namespace UwvLlm.Shared.Dtos;

[GenerateSerializer]
[IsStateDto]
public class State : AuthStateDto
{
}
