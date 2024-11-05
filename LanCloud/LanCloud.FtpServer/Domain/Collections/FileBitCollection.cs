using LanCloud.Domain.IO;
using LanCloud.Domain.Share;
using LanCloud.Models.Configs;
using LanCloud.Shared.Log;
using System;
using System.IO;
using System.Linq;

namespace LanCloud.Domain.Collections
{
    public class FileBitCollection
    {
        public FileBitCollection(LocalShare localShare, ILogger logger)
        {
            LocalShare = localShare;
            Logger = logger;

            Root = new DirectoryInfo(Config.DirectoryName);
            if (!Root.Exists) Root.Create();
            
            FileBits = Root
                .GetFiles("*.filebit")
                .Select(fileRefInfo => new FileBit(fileRefInfo))
                .ToArray();

            //Logger.Info($"Loaded");
        }

        public LocalShare LocalShare { get; }
        private ILogger Logger { get; }
        public DirectoryInfo Root { get; }
        private FileBit[] FileBits { get; set; }

        private LocalShareConfig Config => LocalShare.Config;

        public FileBit CreateTempFileBit(string extention, int[] indexes)
        {
            return new FileBit(Root, extention, indexes);
        }

        public void AddFileBit(FileBit fileBit)
        {
            lock (FileBits)
            {
                var newArray = new FileBit[FileBits.Length + 1];
                Array.Copy(FileBits, newArray, FileBits.Length);
                newArray[newArray.Length - 1] = fileBit;
                FileBits = newArray;
            }
        }

        public FileBit FindFileBit(string extention, FileRef fileRef, FileRefBit fileRefBit)
        {
            lock (FileBits)
            {
                var fileBit = FileBits.FirstOrDefault(a =>
                    a.Extention == extention &&
                    a.Indexes.Length == fileRefBit.Indexes.Length &&
                    a.Indexes.All(b => fileRefBit.Indexes.Any(c => b == c)) &&
                    a.Length == fileRef.Length.Value &&
                    a.Hash == fileRef.Hash);
                return fileBit;
            }
        }
    }
}