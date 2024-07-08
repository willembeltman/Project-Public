using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBlob.Proxy.Interfaces
{
    public interface IStorageFileService
    {
        string GetUrl(IStorageFile storageFile);
    }
}
