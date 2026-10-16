using gAPI.Generated;
using Rekenkamer.Shared.Dtos;
using Rekenkamer.Shared.Interfaces;

namespace Rekenkamer.Backend.Services;

public class UploadApi(
    UploadService uploadService,
    IClientContext clientContext,
    IAuthenticationService auth)
    : IUploadApi
{
    public void GetStatus()
    {
        if (auth.State.User == null)
            return;

        clientContext.UploadHub
            .ToSession(auth.SessionId)
            .UpdateStatus(0, $"{DateTime.Now} {uploadService.Status}");
    }
    public async Task UploadPdf(UploadFile uploadFile, CancellationToken ct)
    {
        await uploadService.AddFile(uploadFile, ct);
    }
}
