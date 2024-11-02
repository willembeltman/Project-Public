using LanCloud.Shared.Log;
using LanCloud.Models;
using LanCloud.Servers.Wjp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LanCloud.Configs;
using LanCloud.Communication.Dtos;

namespace LanCloud.Handlers
{
    public class LocalShareHandler : IWjpHandler
    {
        public LocalShareHandler(LocalShareConfig shareConfig, ILogger logger)
        {
            ShareConfig = shareConfig;
            Logger = logger;

            Root = new DirectoryInfo(shareConfig.FullName);
            _FileBits = Root
                .GetFiles("*.json")
                .Select(file => new FileBit(file))
                .ToList();
        }

        public LocalShareConfig ShareConfig { get; }
        public ILogger Logger { get; }
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

        public WjpResponse ProcessRequest(WjpRequest request)
        {
            var requestDto = JsonConvert.DeserializeObject<ShareRequestDto>(request.Json);

            throw new System.NotImplementedException();
        }
    }
}