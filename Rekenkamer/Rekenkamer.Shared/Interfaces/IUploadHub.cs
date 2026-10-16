using gAPI.Core.Attributes;

namespace Rekenkamer.Shared.Interfaces;

[GenerateHub]
public interface IUploadHub
{
    void UpdateStatus(long index, string line);
}
