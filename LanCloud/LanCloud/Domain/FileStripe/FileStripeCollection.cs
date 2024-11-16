using LanCloud.Models;
using LanCloud.Services;
using LanCloud.Shared.Log;
using System;
using System.IO;
using System.Linq;

namespace LanCloud.Domain.Share
{
    public class FileStripeCollection
    {
        public FileStripeCollection(LocalShare localShare, ILogger logger)
        {
            LocalShare = localShare;
            Logger = logger;

            if (!Root.Exists) Root.Create();

            FileBits = Root
                .GetFiles($"*.filebit")
                .Select(fileRefInfo => new FileStripe(fileRefInfo))
                .ToArray();

            Logger.Info($"Loaded");
        }

        public LocalShare LocalShare { get; }
        private ILogger Logger { get; }
        private FileStripe[] FileBits { get; set; }
        public DirectoryInfo Root => LocalShare.Root;

        public FileStripe CreateTempFileBit(string extention, byte[] indexes)
        {
            return new FileStripe(Root, extention, indexes);
        }

        public void AddFileBit(FileStripe fileBit)
        {
            lock (FileBits)
            {
                var newArray = new FileStripe[FileBits.Length + 1];
                Array.Copy(FileBits, newArray, FileBits.Length);
                newArray[newArray.Length - 1] = fileBit;
                FileBits = newArray;
            }
        }
        public void RemoveFileBit(FileStripe fileBit)
        {
            lock (FileBits)
            {
                // todo
            }
            throw new NotImplementedException();
        }

        public FileStripe FindFileBit(string extention, FileRef fileRef, FileRefStripe fileRefBit)
        {
            lock (FileBits)
            {
                var fileBit = FileBits.FirstOrDefault(a =>
                    a.Indexes.Matches(fileRefBit.Indexes) &&
                    a.Extention == extention &&
                    a.Length == fileRef.Length.Value &&
                    a.Hash == fileRef.Hash);
                return fileBit;
            }
        }

        public FileStripe[] List()
        {
            lock (FileBits)
            {
                return FileBits.ToArray();
            }
        }
    }
}