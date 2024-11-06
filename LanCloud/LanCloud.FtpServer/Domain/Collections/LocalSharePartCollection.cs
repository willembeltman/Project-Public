using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LanCloud.Shared.Log;
using LanCloud.Domain.Share;
using LanCloud.Domain.Application;
using LanCloud.Domain.IO;

namespace LanCloud.Domain.Collections
{
    public class LocalSharePartCollection : IEnumerable<LocalSharePart>, IDisposable
    {
        public LocalSharePartCollection(LocalApplication application, LocalShareCollection localShares, string hostName, ILogger logger)
        {
            Application = application;
            LocalShares = localShares;
            Logger = logger;

            //Logger.Info("Loaded");
        }

        public LocalApplication Application { get; }
        public LocalShareCollection LocalShares { get; }
        public ILogger Logger { get; }
        public LocalSharePart[] LocalShareParts =>
            LocalShares
                .SelectMany(localShare => localShare.LocalShareParts)
                .ToArray();

        public FileBit[] FindFileBits(string extention, FileRef fileRef, FileRefBit fileRefBit)
        {
            var fileBits = LocalShares
                .Select(a => a.FileBits.FindFileBit(extention, fileRef, fileRefBit))
                .Where(a => a != null)
                .ToArray();
            return fileBits;
        }

        public IEnumerator<LocalSharePart> GetEnumerator()
        {
            return ((IEnumerable<LocalSharePart>)LocalShareParts).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return LocalShareParts.GetEnumerator();
        }

        public void Dispose()
        {
            foreach (var item in LocalShareParts)
                item.Dispose();
        }
    }
}