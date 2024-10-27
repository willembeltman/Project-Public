using LanCloud.Shared.Dtos;
using LanCloud.Shared.Interfaces;
using LanCloud.Shared.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace LanCloud
{
    public class LocalShareHandler : ILocalShareHandler
    {
        public LocalShareHandler(LocalShareConfig shareConfig)
        {
            ShareConfig = shareConfig;
            Root = new DirectoryInfo(shareConfig.FullName);
            _FileBits = Root
                .GetFiles("*.json")
                .Select(file => new FileBit(file))
                .ToList();
        }

        public LocalShareConfig ShareConfig { get; }
        public DirectoryInfo Root { get; }
        private List<FileBit> _FileBits { get; }

        public FileBit[] FileBits
        {
            get
            {
                lock (_FileBits)
                {
                    return _FileBits.ToArray();
                }
            }
        }
        public FileBit AddFileBit(string path, long part, byte[] data, long originalSize)
        {
            var extention = path.Split('.').Last();
            string hash = HashHelper.CalculateHash(data);

            var duplicate = FileBits.FirstOrDefault(a =>
                a.Information.Extention == extention &&
                a.Information.Part == part &&
                a.Information.OriginalSize == originalSize &&
                a.Information.Hash == hash);
            if (duplicate != null)
            {
                var paths = duplicate.Information.Paths.ToList();
                paths.Add(path);
                duplicate.Information.Paths = paths.ToArray();
                duplicate.SaveInformation();
            }

            var information = new FileBitInformation()
            {
                Paths = new string[] { path },
                Part = part,
                Size = data.Length,
                Extention = extention,
                OriginalSize = originalSize,
                Hash = hash
            };

            var guid = Guid.NewGuid().ToString().Replace("-", "");
            var jsonFullname = Path.Combine(Root.FullName, $"{guid}.json");
            var jsonFileInfo = new FileInfo(jsonFullname);


            //var jsonFile

            var bit = new FileBit(jsonFileInfo, information, data);
            lock (_FileBits)
            {
                _FileBits.Add(bit);
            }
            return bit;
        }

        public RemoteShareResponse Receive(LocalShareRequest request)
        {
            var requestDto = JsonConvert.DeserializeObject<ShareRequestDto>(request.Json);

            throw new System.NotImplementedException();
        }
    }
}