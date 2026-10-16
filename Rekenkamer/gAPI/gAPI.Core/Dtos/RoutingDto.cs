using gAPI.Core.Attributes;
using gAPI.Core.Ids;

namespace gAPI.Core.Dtos;

[GenerateSerializer]
public record RoutingDto(
    FabricManagerId From_FabricManagerId,
    FabricConnectionId From_FabricConnectionId,

    RequestId RequestId,
    ServiceId ServiceId,
    ServiceMethodId MethodId,
    
    UserId? UserId,
    SessionId? SessionId
    );
