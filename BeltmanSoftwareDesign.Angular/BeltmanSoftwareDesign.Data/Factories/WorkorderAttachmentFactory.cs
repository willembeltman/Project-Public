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
                WorkorderId = b.WorkorderId,
                FileUrl = StorageFileService.GetUrl(b)
            };
        }
    }
}