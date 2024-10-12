using System;
using System.Net;

namespace LanCloud
{
    internal class Folder : IDisposable
    {

        public Folder(IPAddress any, int port, string dataFolder)
        {
            DataFolder = dataFolder;
        }

        public string DataFolder { get; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}