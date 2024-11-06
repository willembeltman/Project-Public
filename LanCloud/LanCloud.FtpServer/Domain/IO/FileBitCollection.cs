using LanCloud.Domain.Share;
using LanCloud.Services;
using LanCloud.Shared.Log;
using System;
using System.IO;
using System.Linq;

namespace LanCloud.Domain.IO
{
    public class FileBitCollection
    {
        public FileBitCollection(LocalSharePart localSharePart, ILogger logger)
        {
            LocalSharePart = localSharePart;
            Logger = logger;

            if (!Root.Exists) Root.Create();

            FileBits = Root
                .GetFiles($"*.{string.Join("_", Indexes)}.filebit")
                .Select(fileRefInfo => new FileBit(fileRefInfo))
                .ToArray();

            Logger.Info($"Loaded");
        }

        public LocalSharePart LocalSharePart { get; }
        private ILogger Logger { get; }
        private FileBit[] FileBits { get; set; }

        public byte[] Indexes => LocalSharePart.Indexes;
        public DirectoryInfo Root => LocalSharePart.LocalShare.Root;

        public FileBit CreateTempFileBit(string extention)
        {
            return new FileBit(Root, extention, Indexes);
        }

        public void AddFileBit(FileBit fileBit)
        {
            if (!fileBit.Indexes.Matches(Indexes)) throw new Exception("Indexes do not match");

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
            if (!fileRefBit.Indexes.Matches(Indexes)) return null;

            lock (FileBits)
            {
                var fileBit = FileBits.FirstOrDefault(a =>
                    a.Extention == extention &&
                    a.Length == fileRef.Length.Value &&
                    a.Hash == fileRef.Hash);
                return fileBit;
            }
        }
    }
}