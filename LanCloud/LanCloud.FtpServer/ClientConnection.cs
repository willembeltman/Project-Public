using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SharpFtpServer
{

    public class ClientConnection : IDisposable
    {
        ILogger Logger { get; }
        string Name { get; }
        TcpClient ControlClient { get; }

        TcpClient DataClient { get; set; }
        TcpListener PassiveListener { get; set; }
        NetworkStream ControlStream { get; set; }
        StreamReader ControlReader { get; set; }
        StreamWriter ControlWriter { get; set; }
        TransferType ConnectionType { get; set; } = TransferType.Ascii;
        FormatControlType FormatControlType { get; set; } = FormatControlType.NonPrint;
        DataConnectionType DataConnectionType { get; set; } = DataConnectionType.Active;
        FileStructureType FileStructureType { get; set; } = FileStructureType.File;

        string UserName { get; set; }
        string Root { get; set; } = "D:\\Willem\\Videos";
        string CurrentDirectory { get; set; } = "D:\\Willem\\Videos";
        IPEndPoint DataEndpoint { get; set; }
        IPEndPoint RemoteEndPoint { get; set; }
        string CertificateFileName { get; set; }
        X509Certificate Cert { get; set; }
        SslStream SslStream { get; set; }
        string ClientIP { get; set; }
        bool Disposed { get; set; }

        //private User _currentUser;

        //private List<string> _validCommands;

        public ClientConnection(TcpClient client, ICommandHandler commandHandler, string certificateFilename = null)
        {
            Name = Guid.NewGuid().ToString().Substring(0, 4);
            ControlClient = client;
            CertificateFileName = certificateFilename;
            Logger = LogManager.GetLogger(typeof(ClientConnection), Name);

            //_validCommands = new List<string>();
        }

        //private string CheckUser()
        //{
        //    if (_currentUser == null)
        //    {
        //        return "530 Not logged in";
        //    }

        //    return null;
        //}

        public void HandleClient(object obj)
        {
            RemoteEndPoint = (IPEndPoint)ControlClient.Client.RemoteEndPoint;

            ClientIP = RemoteEndPoint.Address.ToString();

            ControlStream = ControlClient.GetStream();

            ControlReader = new StreamReader(ControlStream);
            ControlWriter = new StreamWriter(ControlStream);

            ControlWriter.WriteLine("220 Service Ready.");
            ControlWriter.Flush();

            //_validCommands.AddRange(new string[] { "AUTH", "USER", "PASS", "QUIT", "HELP", "NOOP" });

            string line;

            DataClient = new TcpClient();

            string renameFrom = null;

            try
            {
                while ((line = ControlReader.ReadLine()) != null)
                {
                    string response = null;

                    string[] command = line.Split(' ');

                    string cmd = command[0].ToUpperInvariant();
                    string arguments = command.Length > 1 ? line.Substring(command[0].Length + 1) : null;

                    if (arguments != null && arguments.Trim().Length == 0)
                    {
                        arguments = null;
                    }

                    //LogEntry logEntry = new LogEntry
                    //{
                    //    Date = DateTime.Now,
                    //    CIP = ClientIP,
                    //    CSUriStem = arguments
                    //};

                    //if (!_validCommands.Contains(cmd))
                    //{
                    //    response = CheckUser();
                    //}

                    if (cmd != "RNTO")
                    {
                        renameFrom = null;
                    }

                    if (response == null)
                    {
                        switch (cmd)
                        {
                            case "USER":
                                response = User(arguments);
                                break;
                            case "PASS":
                                response = Password(arguments);
                                //logEntry.CSUriStem = "******";
                                break;
                            case "CWD":
                                response = ChangeWorkingDirectory(arguments);
                                break;
                            case "CDUP":
                                response = ChangeWorkingDirectory("..");
                                break;
                            case "QUIT":
                                response = "221 Service closing control connection";
                                break;
                            case "REIN":
                                //_currentUser = null;
                                UserName = null;
                                PassiveListener = null;
                                DataClient = null;

                                response = "220 Service ready for new user";
                                break;
                            case "PORT":
                                response = Port(arguments);
                                //logEntry.CPort = DataEndpoint.Port.ToString();
                                break;
                            case "PASV":
                                response = Passive();
                                //logEntry.SPort = ((IPEndPoint)PassiveListener.LocalEndpoint).Port.ToString();
                                break;
                            case "TYPE":
                                response = Type(command[1], command.Length == 3 ? command[2] : null);
                                //logEntry.CSUriStem = command[1];
                                break;
                            case "STRU":
                                response = Structure(arguments);
                                break;
                            case "MODE":
                                response = Mode(arguments);
                                break;
                            case "RNFR":
                                renameFrom = arguments;
                                response = "350 Requested file action pending further information";
                                break;
                            case "RNTO":
                                response = Rename(renameFrom, arguments);
                                break;
                            case "DELE":
                                response = Delete(arguments);
                                break;
                            case "RMD":
                                response = RemoveDir(arguments);
                                break;
                            case "MKD":
                                response = CreateDir(arguments);
                                break;
                            case "PWD":
                                response = PrintWorkingDirectory();
                                break;
                            case "RETR":
                                response = Retrieve(arguments);
                                //logEntry.Date = DateTime.Now;
                                break;
                            case "STOR":
                                response = Store(arguments);
                                //logEntry.Date = DateTime.Now;
                                break;
                            case "STOU":
                                response = StoreUnique();
                                //logEntry.Date = DateTime.Now;
                                break;
                            case "APPE":
                                response = Append(arguments);
                                //logEntry.Date = DateTime.Now;
                                break;
                            case "LIST":
                                response = List(arguments ?? CurrentDirectory);
                                //logEntry.Date = DateTime.Now;
                                break;
                            case "SYST":
                                response = "215 UNIX Type: L8";
                                break;
                            case "NOOP":
                                response = "200 OK";
                                break;
                            case "ACCT":
                                response = "200 OK";
                                break;
                            case "ALLO":
                                response = "200 OK";
                                break;
                            case "NLST":
                                response = "502 Command not implemented";
                                break;
                            case "SITE":
                                response = "502 Command not implemented";
                                break;
                            case "STAT":
                                response = "502 Command not implemented";
                                break;
                            case "HELP":
                                response = "502 Command not implemented";
                                break;
                            case "SMNT":
                                response = "502 Command not implemented";
                                break;
                            case "REST":
                                response = "502 Command not implemented";
                                break;
                            case "ABOR":
                                response = "502 Command not implemented";
                                break;

                            // Extensions defined by rfc 2228
                            case "AUTH":
                                response = "502 Command not implemented";
                                //response = Auth(arguments);
                                break;

                            // Extensions defined by rfc 2389
                            case "FEAT":
                                response = FeatureList();
                                break;
                            case "OPTS":
                                response = Options(arguments);
                                break;

                            // Extensions defined by rfc 3659
                            case "MDTM":
                                response = FileModificationTime(arguments);
                                break;
                            case "SIZE":
                                response = FileSize(arguments);
                                break;

                            // Extensions defined by rfc 2428
                            case "EPRT":
                                response = EPort(arguments);
                                //logEntry.CPort = DataEndpoint.Port.ToString();
                                break;
                            case "EPSV":
                                response = EPassive();
                                //logEntry.SPort = ((IPEndPoint)PassiveListener.LocalEndpoint).Port.ToString();
                                break;

                            default:
                                response = "502 Command not implemented";
                                break;
                        }
                    }

                    //logEntry.CSMethod = cmd;
                    //logEntry.CSUsername = UserName;
                    //logEntry.SCStatus = response.Substring(0, response.IndexOf(' '));

                    //Logger.Info(logEntry);

                    if (ControlClient == null || !ControlClient.Connected)
                    {
                        break;
                    }
                    else
                    {
                        ControlWriter.WriteLine(response);
                        ControlWriter.Flush();

                        if (response.StartsWith("221"))
                        {
                            break;
                        }

                        if (cmd == "AUTH" && CertificateFileName != null)
                        {
                            Cert = new X509Certificate(CertificateFileName);

                            SslStream = new SslStream(ControlStream);

                            SslStream.AuthenticateAsServer(Cert);

                            ControlReader = new StreamReader(SslStream);
                            ControlWriter = new StreamWriter(SslStream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }

            Dispose();
        }

        private bool IsPathValid(string path)
        {
            return path.StartsWith(Root);
        }

        private string NormalizeFilename(string path)
        {
            if (path == null)
            {
                path = string.Empty;
            }

            if (path == "/")
            {
                return Root;
            }
            else if (path.StartsWith("/"))
            {
                path = new FileInfo(Path.Combine(Root, path.Substring(1))).FullName;
            }
            else
            {
                path = new FileInfo(Path.Combine(CurrentDirectory, path)).FullName;
            }

            return IsPathValid(path) ? path : null;
        }

        #region FTP Commands

        private string FeatureList()
        {
            ControlWriter.WriteLine("211- Extensions supported:");
            ControlWriter.WriteLine(" MDTM");
            ControlWriter.WriteLine(" SIZE");
            return "211 End";
        }

        private string Options(string arguments)
        {
            return "200 Looks good to me...";
        }

        private string Auth(string authMode)
        {
            if (authMode == "TLS")
            {
                return "234 Enabling TLS Connection";
            }
            else
            {
                return "504 Unrecognized AUTH mode";
            }
        }

        private string User(string username)
        {
            UserName = username;

            return "331 Username ok, need password";
        }

        private string Password(string password)
        {
            //_currentUser = UserStore.Validate(_username, password);

            //if (_currentUser != null)
            //{
            //    _root = _currentUser.HomeDir;
            //    _currentDirectory = _root;

            //    return "230 User logged in";
            //}
            //else
            //{
            //    return "530 Not logged in";
            //}
            return "230 User logged in";
        }

        private string ChangeWorkingDirectory(string pathname)
        {
            if (pathname == "/")
            {
                CurrentDirectory = Root;
            }
            else
            {
                string newDir;

                if (pathname.StartsWith("/"))
                {
                    pathname = pathname.Substring(1).Replace('/', '\\');
                    newDir = Path.Combine(Root, pathname);
                }
                else
                {
                    pathname = pathname.Replace('/', '\\');
                    newDir = Path.Combine(CurrentDirectory, pathname);
                }

                if (Directory.Exists(newDir))
                {
                    CurrentDirectory = new DirectoryInfo(newDir).FullName;

                    if (!IsPathValid(CurrentDirectory))
                    {
                        CurrentDirectory = Root;
                    }
                }
                else
                {
                    CurrentDirectory = Root;
                }
            }

            return "250 Changed to new directory";
        }

        private string Port(string hostPort)
        {
            DataConnectionType = DataConnectionType.Active;

            string[] ipAndPort = hostPort.Split(',');

            byte[] ipAddress = new byte[4];
            byte[] port = new byte[2];

            for (int i = 0; i < 4; i++)
            {
                ipAddress[i] = Convert.ToByte(ipAndPort[i]);
            }

            for (int i = 4; i < 6; i++)
            {
                port[i - 4] = Convert.ToByte(ipAndPort[i]);
            }

            if (BitConverter.IsLittleEndian)
                Array.Reverse(port);

            DataEndpoint = new IPEndPoint(new IPAddress(ipAddress), BitConverter.ToInt16(port, 0));

            return "200 Data Connection Established";
        }

        private string EPort(string hostPort)
        {
            DataConnectionType = DataConnectionType.Active;

            char delimiter = hostPort[0];

            string[] rawSplit = hostPort.Split(new char[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);

            char ipType = rawSplit[0][0];

            string ipAddress = rawSplit[1];
            string port = rawSplit[2];

            DataEndpoint = new IPEndPoint(IPAddress.Parse(ipAddress), int.Parse(port));

            return "200 Data Connection Established";
        }

        private string Passive()
        {
            DataConnectionType = DataConnectionType.Passive;

            IPAddress localIp = ((IPEndPoint)ControlClient.Client.LocalEndPoint).Address;

            PassiveListener = new TcpListener(localIp, 0);
            PassiveListener.Start();

            IPEndPoint passiveListenerEndpoint = (IPEndPoint)PassiveListener.LocalEndpoint;

            byte[] address = passiveListenerEndpoint.Address.GetAddressBytes();
            short port = (short)passiveListenerEndpoint.Port;

            byte[] portArray = BitConverter.GetBytes(port);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(portArray);

            return string.Format("227 Entering Passive Mode ({0},{1},{2},{3},{4},{5})", address[0], address[1], address[2], address[3], portArray[0], portArray[1]);
        }

        private string EPassive()
        {
            DataConnectionType = DataConnectionType.Passive;

            IPAddress localIp = ((IPEndPoint)ControlClient.Client.LocalEndPoint).Address;

            PassiveListener = new TcpListener(localIp, 0);
            PassiveListener.Start();

            IPEndPoint passiveListenerEndpoint = (IPEndPoint)PassiveListener.LocalEndpoint;

            return string.Format("229 Entering Extended Passive Mode (|||{0}|)", passiveListenerEndpoint.Port);
        }

        private string Type(string typeCode, string formatControl)
        {
            switch (typeCode.ToUpperInvariant())
            {
                case "A":
                    ConnectionType = TransferType.Ascii;
                    break;
                case "I":
                    ConnectionType = TransferType.Image;
                    break;
                default:
                    return "504 Command not implemented for that parameter";
            }

            if (!string.IsNullOrWhiteSpace(formatControl))
            {
                switch (formatControl.ToUpperInvariant())
                {
                    case "N":
                        FormatControlType = FormatControlType.NonPrint;
                        break;
                    default:
                        return "504 Command not implemented for that parameter";
                }
            }

            return string.Format("200 Type set to {0}", ConnectionType);
        }

        private string Delete(string pathname)
        {
            pathname = NormalizeFilename(pathname);

            if (pathname != null)
            {
                if (File.Exists(pathname))
                {
                    File.Delete(pathname);
                }
                else
                {
                    return "550 File Not Found";
                }

                return "250 Requested file action okay, completed";
            }

            return "550 File Not Found";
        }

        private string RemoveDir(string pathname)
        {
            pathname = NormalizeFilename(pathname);

            if (pathname != null)
            {
                if (Directory.Exists(pathname))
                {
                    Directory.Delete(pathname);
                }
                else
                {
                    return "550 Directory Not Found";
                }

                return "250 Requested file action okay, completed";
            }

            return "550 Directory Not Found";
        }

        private string CreateDir(string pathname)
        {
            pathname = NormalizeFilename(pathname);

            if (pathname != null)
            {
                if (!Directory.Exists(pathname))
                {
                    Directory.CreateDirectory(pathname);
                }
                else
                {
                    return "550 Directory already exists";
                }

                return "250 Requested file action okay, completed";
            }

            return "550 Directory Not Found";
        }

        private string FileModificationTime(string pathname)
        {
            pathname = NormalizeFilename(pathname);

            if (pathname != null)
            {
                if (File.Exists(pathname))
                {
                    return string.Format("213 {0}", File.GetLastWriteTime(pathname).ToString("yyyyMMddHHmmss.fff"));
                }
            }

            return "550 File Not Found";
        }

        private string FileSize(string pathname)
        {
            pathname = NormalizeFilename(pathname);

            if (pathname != null)
            {
                if (File.Exists(pathname))
                {
                    long length = 0;

                    using (FileStream fs = File.Open(pathname, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        length = fs.Length;
                    }

                    return string.Format("213 {0}", length);
                }
            }

            return "550 File Not Found";
        }

        private string Retrieve(string pathname)
        {
            pathname = NormalizeFilename(pathname);

            if (pathname != null)
            {
                if (File.Exists(pathname))
                {
                    var state = new DataConnectionOperation { Arguments = pathname, Operation = RetrieveOperation };

                    SetupDataConnectionOperation(state);

                    return string.Format("150 Opening {0} mode data transfer for RETR", DataConnectionType);
                }
            }

            return "550 File Not Found";
        }

        private string Store(string pathname)
        {
            pathname = NormalizeFilename(pathname);

            if (pathname != null)
            {
                var state = new DataConnectionOperation { Arguments = pathname, Operation = StoreOperation };

                SetupDataConnectionOperation(state);

                return string.Format("150 Opening {0} mode data transfer for STOR", DataConnectionType);
            }

            return "450 Requested file action not taken";
        }

        private string Append(string pathname)
        {
            pathname = NormalizeFilename(pathname);

            if (pathname != null)
            {
                var state = new DataConnectionOperation { Arguments = pathname, Operation = AppendOperation };

                SetupDataConnectionOperation(state);

                return string.Format("150 Opening {0} mode data transfer for APPE", DataConnectionType);
            }

            return "450 Requested file action not taken";
        }

        private string StoreUnique()
        {
            string pathname = NormalizeFilename(new Guid().ToString());

            var state = new DataConnectionOperation { Arguments = pathname, Operation = StoreOperation };

            SetupDataConnectionOperation(state);

            return string.Format("150 Opening {0} mode data transfer for STOU", DataConnectionType);
        }

        private string PrintWorkingDirectory()
        {
            string current = CurrentDirectory.Replace(Root, string.Empty).Replace('\\', '/');

            if (current.Length == 0)
            {
                current = "/";
            }

            return string.Format("257 \"{0}\" is current directory.", current); ;
        }

        private string List(string pathname)
        {
            pathname = NormalizeFilename(pathname);

            if (pathname != null)
            {
                var state = new DataConnectionOperation { Arguments = pathname, Operation = ListOperation };

                SetupDataConnectionOperation(state);

                return string.Format("150 Opening {0} mode data transfer for LIST", DataConnectionType);
            }

            return "450 Requested file action not taken";
        }

        private string Structure(string structure)
        {
            switch (structure)
            {
                case "F":
                    FileStructureType = FileStructureType.File;
                    break;
                case "R":
                case "P":
                    return string.Format("504 STRU not implemented for \"{0}\"", structure);
                default:
                    return string.Format("501 Parameter {0} not recognized", structure);
            }

            return "200 Command OK";
        }

        private string Mode(string mode)
        {
            if (mode.ToUpperInvariant() == "S")
            {
                return "200 OK";
            }
            else
            {
                return "504 Command not implemented for that parameter";
            }
        }

        private string Rename(string renameFrom, string renameTo)
        {
            if (string.IsNullOrWhiteSpace(renameFrom) || string.IsNullOrWhiteSpace(renameTo))
            {
                return "450 Requested file action not taken";
            }

            renameFrom = NormalizeFilename(renameFrom);
            renameTo = NormalizeFilename(renameTo);

            if (renameFrom != null && renameTo != null)
            {
                if (File.Exists(renameFrom))
                {
                    File.Move(renameFrom, renameTo);
                }
                else if (Directory.Exists(renameFrom))
                {
                    Directory.Move(renameFrom, renameTo);
                }
                else
                {
                    return "450 Requested file action not taken";
                }

                return "250 Requested file action okay, completed";
            }

            return "450 Requested file action not taken";
        }

        #endregion

        #region DataConnection Operations

        private void HandleAsyncResult(IAsyncResult result)
        {
            if (DataConnectionType == DataConnectionType.Active)
            {
                DataClient.EndConnect(result);
            }
            else
            {
                DataClient = PassiveListener.EndAcceptTcpClient(result);
            }
        }

        private void SetupDataConnectionOperation(DataConnectionOperation state)
        {
            if (DataConnectionType == DataConnectionType.Active)
            {
                DataClient = new TcpClient(DataEndpoint.AddressFamily);
                DataClient.BeginConnect(DataEndpoint.Address, DataEndpoint.Port, DoDataConnectionOperation, state);
            }
            else
            {
                PassiveListener.BeginAcceptTcpClient(DoDataConnectionOperation, state);
            }
        }

        private void DoDataConnectionOperation(IAsyncResult result)
        {
            HandleAsyncResult(result);

            DataConnectionOperation op = result.AsyncState as DataConnectionOperation;

            string response;

            using (NetworkStream dataStream = DataClient.GetStream())
            {
                response = op.Operation(dataStream, op.Arguments);
            }

            DataClient.Close();
            DataClient = null;

            ControlWriter.WriteLine(response);
            ControlWriter.Flush();
        }

        private string RetrieveOperation(NetworkStream dataStream, string pathname)
        {
            long bytes = 0;

            using (FileStream fs = new FileStream(pathname, FileMode.Open, FileAccess.Read))
            {
                bytes = CopyStream(fs, dataStream);
            }

            return "226 Closing data connection, file transfer successful";
        }

        private string StoreOperation(NetworkStream dataStream, string pathname)
        {
            long bytes = 0;

            using (FileStream fs = new FileStream(pathname, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 4096, FileOptions.SequentialScan))
            {
                bytes = CopyStream(dataStream, fs);
            }

            LogEntry logEntry = new LogEntry
            {
                Date = DateTime.Now,
                CIP = ClientIP,
                CSMethod = "STOR",
                CSUsername = UserName,
                SCStatus = "226",
                CSBytes = bytes.ToString()
            };

            Logger.Info(logEntry);

            return "226 Closing data connection, file transfer successful";
        }

        private string AppendOperation(NetworkStream dataStream, string pathname)
        {
            long bytes = 0;

            using (FileStream fs = new FileStream(pathname, FileMode.Append, FileAccess.Write, FileShare.None, 4096, FileOptions.SequentialScan))
            {
                bytes = CopyStream(dataStream, fs);
            }

            LogEntry logEntry = new LogEntry
            {
                Date = DateTime.Now,
                CIP = ClientIP,
                CSMethod = "APPE",
                CSUsername = UserName,
                SCStatus = "226",
                CSBytes = bytes.ToString()
            };

            Logger.Info(logEntry);

            return "226 Closing data connection, file transfer successful";
        }

        private string ListOperation(NetworkStream dataStream, string pathname)
        {
            StreamWriter dataWriter = new StreamWriter(dataStream, Encoding.ASCII);

            IEnumerable<string> directories = Directory.EnumerateDirectories(pathname);

            foreach (string dir in directories)
            {
                DirectoryInfo d = new DirectoryInfo(dir);

                string date = d.LastWriteTime < DateTime.Now - TimeSpan.FromDays(180) ?
                    d.LastWriteTime.ToString("MMM dd  yyyy") :
                    d.LastWriteTime.ToString("MMM dd HH:mm");

                string line = string.Format("drwxr-xr-x    2 2003     2003     {0,8} {1} {2}", "4096", date, d.Name);

                dataWriter.WriteLine(line);
                dataWriter.Flush();
            }

            IEnumerable<string> files = Directory.EnumerateFiles(pathname);

            foreach (string file in files)
            {
                FileInfo f = new FileInfo(file);

                string date = f.LastWriteTime < DateTime.Now - TimeSpan.FromDays(180) ?
                    f.LastWriteTime.ToString("MMM dd  yyyy") :
                    f.LastWriteTime.ToString("MMM dd HH:mm");

                string line = string.Format("-rw-r--r--    2 2003     2003     {0,8} {1} {2}", f.Length, date, f.Name);

                dataWriter.WriteLine(line);
                dataWriter.Flush();
            }

            LogEntry logEntry = new LogEntry
            {
                Date = DateTime.Now,
                CIP = ClientIP,
                CSMethod = "LIST",
                CSUsername = UserName,
                SCStatus = "226"
            };

            Logger.Info(logEntry);

            return "226 Transfer complete";
        }

        #endregion

        #region Copy Stream Implementations

        private static long CopyStream(Stream input, Stream output, int bufferSize)
        {
            byte[] buffer = new byte[bufferSize];
            int count = 0;
            long total = 0;

            while ((count = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, count);
                total += count;
            }

            return total;
        }

        private static long CopyStreamAscii(Stream input, Stream output, int bufferSize)
        {
            char[] buffer = new char[bufferSize];
            int count = 0;
            long total = 0;

            using (StreamReader rdr = new StreamReader(input, Encoding.ASCII))
            {
                using (StreamWriter wtr = new StreamWriter(output, Encoding.ASCII))
                {
                    while ((count = rdr.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        wtr.Write(buffer, 0, count);
                        total += count;
                    }
                }
            }

            return total;
        }

        private long CopyStream(Stream input, Stream output)
        {
            Stream limitedStream = output; // new RateLimitingStream(output, 131072, 0.5);

            if (ConnectionType == TransferType.Image)
            {
                return CopyStream(input, limitedStream, 4096);
            }
            else
            {
                return CopyStreamAscii(input, limitedStream, 4096);
            }
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                if (disposing)
                {
                    if (ControlClient != null)
                    {
                        ControlClient.Close();
                    }

                    if (DataClient != null)
                    {
                        DataClient.Close();
                    }

                    if (ControlStream != null)
                    {
                        ControlStream.Close();
                    }

                    if (ControlReader != null)
                    {
                        ControlReader.Close();
                    }

                    if (ControlWriter != null)
                    {
                        ControlWriter.Close();
                    }
                }
            }

            Disposed = true;
        }

        #endregion
    }

    #region Enums

    public enum TransferType
    {
        Ascii,
        Ebcdic,
        Image,
        Local,
    }

    public enum FormatControlType
    {
        NonPrint,
        Telnet,
        CarriageControl,
    }

    public enum DataConnectionType
    {
        Passive,
        Active,
    }

    public enum FileStructureType
    {
        File,
        Record,
        Page,
    }

    #endregion
}