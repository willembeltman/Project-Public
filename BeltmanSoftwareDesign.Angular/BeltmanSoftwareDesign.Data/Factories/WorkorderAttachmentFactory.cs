using BeltmanSoftwareDesign.Data.Entities;
using BeltmanSoftwareDesign.Shared.Jsons;
using BeltmanSoftwareDesign.StorageBlob.Business.Interfaces;

namespace BeltmanSoftwareDesign.Data.Factories
{
    public class WorkorderAttachmentFactory
    {
        public WorkorderAttachmentFactory(IStorageFileService storageFileService)
        {
            StorageFileService = storageFileService;
        }

        IStorageFileService StorageFileService { get; }

        public Shared.Jsons.WorkorderAttachment Convert(Entities.WorkorderAttachment b)
        {
            return new Shared.Jsons.WorkorderAttachment()
            {
                id = b.id,
                FileMD5 = b.FileMD5,
                FileMimeType = b.FileMimeType,
                FileName = b.FileName,
                FileSize = b.FileSize,
                FileUrl = StorageFileService.GetUrl(b)
            };
        }
    }
}