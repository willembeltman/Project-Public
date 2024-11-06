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
    public class LocalShareCollection : IEnumerable<LocalShare>, IDisposable
    {
        public LocalShareCollection(LocalApplication application, string hostName, ILogger logger)
        {
            Application = application;
            Logger = logger;

            var config = application.Config;
            var port = config.StartPort;
            Shares = config.Shares
                .Select(shareConfig => new LocalShare(this, hostName, ++port, config, shareConfig, logger))
                .ToArray();

            //Logger.Info("Loaded");
        }

        public LocalApplication Application { get; }
        public ILogger Logger { get; }
        public LocalShare[] Shares { get; }

        public FileBit[] FindFileBits(string extention, FileRef fileRef, FileRefBit fileRefBit)
        {
            var fileBits = Shares
                .Select(a => a.FileBits.FindFileBit(extention, fileRef, fileRefBit))
                .Where(a => a != null)
                .ToArray();
            return fileBits;
        }

        public IEnumerator<LocalShare> GetEnumerator()
        {
            return ((IEnumerable<LocalShare>)Shares).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Shares.GetEnumerator();
        }

        public void Dispose()
        {
            foreach (var item in Shares)
                item.Dispose();
        }
    }
}