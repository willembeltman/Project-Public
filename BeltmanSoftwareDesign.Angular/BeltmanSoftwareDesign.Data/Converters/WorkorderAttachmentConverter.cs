using StorageBlob.Proxy.Interfaces;

namespace BeltmanSoftwareDesign.Data.Converters
{
    public class WorkorderAttachmentConverter
    {
        public WorkorderAttachmentConverter(IStorageFileService storageFileService)
        {
            StorageFileService = storageFileService;
        }

        IStorageFileService StorageFileService { get; }

        public Shared.Jsons.WorkorderAttachment Create(Entities.WorkorderAttachment a)
        {
            return new Shared.Jsons.WorkorderAttachment()
            {
                id = a.id,
                WorkorderId = a.WorkorderId,
                FileUrl = StorageFileService.GetUrl(a)
            };
        }
    }
}