using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LanCloud.Dtos
{
    public class ExternalShareDto
    {
        public IPAddress IpAdress { get; internal set; }
        public int Port { get; internal set; }
        public int FreeSpace { get; internal set; }
    }
}
