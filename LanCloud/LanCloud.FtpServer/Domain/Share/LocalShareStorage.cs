using LanCloud.Domain.IO;
using LanCloud.Models.Configs;
using LanCloud.Shared.Log;
using System;
using System.IO;
using System.IO.Ports;
using System.Linq;

namespace LanCloud.Domain.Share
{
    public class LocalShareStorage
    {
        public LocalShareStorage(LocalShareConfig config, ILogger logger)
        {
            Config = config;
            Logger = logger;

            Root = new DirectoryInfo(Config.DirectoryName);
            if (!Root.Exists) Root.Create();
            
            FileBits = Root
                .GetFiles("*.filebit")
                .Select(fileRefInfo => new FileBit(fileRefInfo))
                .ToArray();

            //Logger.Info($"Loaded");
        }

        private LocalShareConfig Config { get; }
        private ILogger Logger { get; }
        public DirectoryInfo Root { get; }
        private FileBit[] FileBits { get; set; }

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

        public FileBit FindFileBit(FileRef fileRef, FileRefBit fileRefBit)
        {
            lock (FileBits)
            {
                var fileBit = FileBits.FirstOrDefault(a =>
                    a.Extention == fileRef.Extention &&
                    a.Indexes.Length == fileRefBit.Indexes.Length &&
                    a.Indexes.All(b => fileRefBit.Indexes.Any(c => b == c)) &&
                    a.Length == fileRef.Length.Value &&
                    a.Hash == fileRef.Hash);
                return fileBit;

                //return new FileBit(Root, fileRef.Extention, fileRefBit.Indexes, fileRef.Length.Value, fileRef.Hash);
            }
        }
    }
}