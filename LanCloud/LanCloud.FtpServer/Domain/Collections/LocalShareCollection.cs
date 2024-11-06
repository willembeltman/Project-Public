using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LanCloud.Domain.Application;
using LanCloud.Domain.IO;
using LanCloud.Domain.Share;
using LanCloud.Shared.Log;

namespace LanCloud.Domain.Collections
{
    public class LocalShareCollection : IEnumerable<LocalShare>
    {
        public LocalShareCollection(LocalApplication application, ILogger logger)
        {
            Application = application;
            Logger = logger;
            int port = application.Port;
            LocalShares = Application.Config.Shares
                .Select(share => new LocalShare(this, share, ref port, Logger))
                .ToArray();
        }

        public LocalApplication Application { get; }
        public ILogger Logger { get; }

        public LocalShare[] LocalShares { get; }

        public FileBit[] FindFileBits(string extention, FileRef fileRef, FileRefBit fileRefBit)
        {
            var fileBits = LocalShares
                .Select(a => a.FileBits.FindFileBit(extention, fileRef, fileRefBit))
                .Where(a => a != null)
                .ToArray();
            return fileBits;
        }

        public IEnumerator<LocalShare> GetEnumerator()
        {
            return ((IEnumerable<LocalShare>)LocalShares).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}