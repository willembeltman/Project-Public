using System;
using System.IO;
using System.Xml.Linq;

namespace SharpFtpServer
{
    internal class LogManager
    {
        internal static ILogger GetLogger(Type type)
        {
            var filename = $"{type.Name}.txt";
            var fullname = Path.Combine(Environment.CurrentDirectory, filename);
            return new Logger(fullname);
        }
        internal static ILogger GetLogger(Type type, string name)
        {
            var filename = $"{type.Name} {name}.txt";
            var fullname = Path.Combine(Environment.CurrentDirectory, filename);
            return new Logger(fullname);
        }
    }
}