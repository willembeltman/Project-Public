namespace StorageBlob.Proxy.Interfaces
{
    public interface IStorageFile
    {
        long id { get; set; }
        string StorageFolder { get; }
        string FileMimeType { get; set; }
        string FileName { get; set; }
        long FileSize { get; set; }
        string FileMD5 { get; set; }
    }
}
