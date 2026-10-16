using gAPI.Core.Attributes;
using Rekenkamer.Shared.Dtos;

namespace Rekenkamer.Shared.Interfaces;

[GenerateApi]
public interface IUploadApi
{
    void GetStatus();
    Task UploadPdf(UploadFile file, CancellationToken ct);
}