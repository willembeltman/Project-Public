using gAPI.Core.Attributes;
using gAPI.Core.Dtos;
using TinderWithStats.Shared.Dtos;

namespace TinderWithStats.Shared.Interfaces;

[GenerateApi]
public interface ISwipeApi
{
    public Task<BaseResponseT<Profile>> GetSwipeProfileAsync(CancellationToken ct);
}
