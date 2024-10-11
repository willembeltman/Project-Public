using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpFtpServer
{
    public class CommandHandler : ICommandHandler
    {
        public CommandHandler(string basePath)
        {
            BasePath = basePath;
        }

        public string BasePath { get; }
    }
}
