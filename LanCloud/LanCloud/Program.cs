using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System;

namespace LanCloud
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //string prefix = "http://localhost:8080/";
            //string baseFolder = @"D:\Willem\Videos"; // Vervang dit met je map
            //var server = new SimpleHttpServer(prefix, baseFolder);
            //server.Start();

            string ipAddress = "127.0.0.1";
            int port = 21;
            string baseDirectory = @"D:\Willem\Videos";

            if (!Directory.Exists(baseDirectory))
            {
                Console.WriteLine($"De directory {baseDirectory} bestaat niet.");
                return;
            }

            var server = new SimpleFTPServer(ipAddress, port, baseDirectory);
            await server.StartAsync();
        }
    }



    public class SimpleFTPServer
    {
        private TcpListener _listener;
        private string _transferMode;
        private readonly string _baseDirectory;
        private IPAddress _dataConnectionAddress;
        private int _dataConnectionPort;
        private TcpListener _passiveListener;

        public SimpleFTPServer(string ipAddress, int port, string baseDirectory)
        {
            _listener = new TcpListener(IPAddress.Parse(ipAddress), port);
            _baseDirectory = baseDirectory;
        }

        public async Task StartAsync()
        {
            _listener.Start();
            Console.WriteLine("FTP Server is gestart...");

            using (var logstream = File.OpenWrite("log.txt"))
            using (var logwriter = new StreamWriter(logstream))
                while (true)
                {
                    var client = await _listener.AcceptTcpClientAsync();
                    Console.WriteLine("Client verbonden.");
                    _ = HandleClientAsync(client, logwriter); // Handle each client in a separate task
                }
        }

        private async Task HandleClientAsync(TcpClient client, StreamWriter logwriter)
        {
            using (var networkStream = client.GetStream())
            using (var networkReader = new StreamReader(networkStream, Encoding.ASCII))
            using (var networkWriter = new StreamWriter(networkStream, Encoding.ASCII) { AutoFlush = true })
            {
                var reader = new LogReader(logwriter, networkReader);
                var writer = new LogWriter(logwriter, networkWriter);

                // Stuur welkom bericht
                await writer.WriteLineAsync("220 Simple FTP Server Ready");

                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    Console.WriteLine($"Ontvangen: {line}");
                    var commandParts = line.Split(' ');
                    var command = commandParts[0].ToUpper();
                    var argument = commandParts.Length > 1 ? line.Substring(command.Length).Trim() : null;
                    await HandleCommand(client, networkStream, writer, command, argument);
                }
            }
        }

        private async Task HandleCommand(TcpClient client, NetworkStream networkStream, LogWriter writer, string command, string argument)
        {
            switch (command)
            {
                case "USER":
                    await writer.WriteLineAsync("331 Gebruikersnaam OK, wachtwoord vereist");
                    break;
                case "PASS":
                    await writer.WriteLineAsync("230 Gebruiker ingelogd");
                    break;
                case "PWD":
                    await writer.WriteLineAsync($"257 \"{_baseDirectory}\" is de huidige directory");
                    break;
                case "CWD":
                    await writer.WriteLineAsync("250 Directory changed successfully.");
                    break;
                case "LIST":
                    await HandleListCommand(writer, networkStream);
                    break;
                case "RETR":
                    if (argument != null)
                    {
                        await HandleRetrCommand(writer, networkStream, argument);
                    }
                    else
                    {
                        await writer.WriteLineAsync("501 Syntax error in parameters or arguments.");
                    }
                    break;
                case "TYPE":
                    if (argument != null && argument.ToUpper() == "A")
                    {
                        _transferMode = "A"; // Set to ASCII mode
                        await writer.WriteLineAsync("200 Type set to ASCII mode.");
                    }
                    else if (argument != null && argument.ToUpper() == "I")
                    {
                        _transferMode = "I"; // Set to Binary mode
                        await writer.WriteLineAsync("200 Type set to Binary mode.");
                    }
                    else
                    {
                        await writer.WriteLineAsync("504 Command not implemented for that parameter.");
                    }
                    break;
                case "PORT":
                    if (argument != null)
                    {
                        await HandlePortCommand(writer, argument);
                    }
                    else
                    {
                        await writer.WriteLineAsync("501 Syntax error in parameters or arguments.");
                    }
                    break;
                case "QUIT":
                    await writer.WriteLineAsync("221 Goodbye.");
                    client.Close();
                    return;
                default:
                    await writer.WriteLineAsync("502 Command not implemented");
                    break;
            }
        }

        private async Task HandleListCommand(LogWriter writer, NetworkStream controlStream)
        {
            try
            {
                var directoryInfo = new DirectoryInfo(_baseDirectory);
                var files = directoryInfo.GetFiles();

                await writer.WriteLineAsync("150 Opening ASCII mode data connection for directory list.");

                TcpClient dataClient;
                if (_passiveListener != null)
                {
                    // In passieve modus: wachten op inkomende verbinding van de client
                    dataClient = await _passiveListener.AcceptTcpClientAsync();
                    _passiveListener.Stop();
                    _passiveListener = null;
                }
                else
                {
                    // In actieve modus: verbind met de client (IP/poort opgegeven via PORT-commando)
                    dataClient = new TcpClient();
                    await dataClient.ConnectAsync(_dataConnectionAddress, _dataConnectionPort);
                }

                using (var dataStream = dataClient.GetStream())
                using (var dataWriter = new StreamWriter(dataStream, Encoding.ASCII))
                {
                    foreach (var file in files)
                    {
                        // Verkrijg bestandsspecificaties
                        var permissions = "-rw-r--r--"; // Fictieve bestandspermissies (voor een FTP-server meestal niet van toepassing)
                        var fileSize = file.Length;
                        var lastModified = file.LastWriteTime.ToString("MMM dd HH:mm"); // Formaat zoals in 'ls -l'
                        var owner = "owner"; // Fictieve eigenaar (je kunt dit eventueel aanpassen)
                        var group = "group"; // Fictieve groep (je kunt dit eventueel aanpassen)

                        // Outputformaat van 'ls -l': <permissions> <links> <owner> <group> <size> <date> <name>
                        string fileInfo = $"{permissions} 1 {owner} {group} {fileSize,8} {lastModified} {file.Name}";

                        // Schrijf de informatie naar de client
                        await dataWriter.WriteLineAsync(fileInfo);
                    }

                    await dataWriter.FlushAsync(); // Zorg ervoor dat alles is verzonden
                }

                dataClient.Close(); // Sluit de data-verbinding na het verzenden van de lijst

                await writer.WriteLineAsync("226 Transfer complete."); // Stuur de 226 na het sluiten van de verbinding
            }
            catch (Exception ex)
            {
                await writer.WriteLineAsync($"550 Failed to list directory. Error: {ex.Message}");
            }
        }


        private async Task HandlePortCommand(LogWriter writer, string argument)
        {
            var parts = argument.Split(',');
            if (parts.Length != 6)
            {
                await writer.WriteLineAsync("501 Syntax error in parameters or arguments.");
                return;
            }

            // Parse IP address
            string ipAddress = string.Join(".", parts[0], parts[1], parts[2], parts[3]);

            // Parse port
            int port = (int.Parse(parts[4]) * 256) + int.Parse(parts[5]);

            _dataConnectionAddress = IPAddress.Parse(ipAddress);
            _dataConnectionPort = port;

            Console.WriteLine($"PORT command received, IP: {_dataConnectionAddress}, Port: {_dataConnectionPort}");

            await writer.WriteLineAsync("200 PORT command successful.");
        }
        private async Task HandlePasvCommand(LogWriter writer)
        {
            // Open een nieuwe socket in passieve modus op een willekeurige poort
            _passiveListener = new TcpListener(IPAddress.Any, 0); // 0 betekent dat we een willekeurige poort gebruiken
            _passiveListener.Start();
            var localEndPoint = _passiveListener.LocalEndpoint as IPEndPoint;

            // IP-adres van de server en poort in het juiste formaat
            var address = ((IPEndPoint)_passiveListener.LocalEndpoint).Address;
            var port = ((IPEndPoint)_passiveListener.LocalEndpoint).Port;

            var addressParts = address.ToString().Split('.');
            var portHigh = port / 256;
            var portLow = port % 256;

            // Stuur het 227-antwoord terug naar de client, met de IP en poort
            await writer.WriteLineAsync($"227 Entering Passive Mode ({string.Join(",", addressParts)},{portHigh},{portLow})");
        }
        private async Task HandleRetrCommand(LogWriter writer, NetworkStream controlStream, string filename)
        {
            var filePath = Path.Combine(_baseDirectory, filename);
            if (!File.Exists(filePath))
            {
                await writer.WriteLineAsync("550 Bestand niet gevonden.");
                return;
            }

            try
            {
                await writer.WriteLineAsync("150 Opening data connection for file transfer.");

                TcpClient dataClient = null;
                try
                {
                    if (_passiveListener != null)
                    {
                        // Passieve modus
                        dataClient = await _passiveListener.AcceptTcpClientAsync();
                        _passiveListener.Stop();
                        _passiveListener = null;
                    }
                    else
                    {
                        // Actieve modus
                        dataClient = new TcpClient();
                        await dataClient.ConnectAsync(_dataConnectionAddress, _dataConnectionPort);
                    }
                }
                catch (Exception)
                {
                    // Actieve modus faalt: overschakelen naar passieve modus
                    await HandlePasvCommand(writer);
                    dataClient = await _passiveListener.AcceptTcpClientAsync();
                    _passiveListener.Stop();
                    _passiveListener = null;
                }

                // Overdracht van het bestand
                using (var dataStream = dataClient.GetStream())
                {
                    byte[] fileBytes = File.ReadAllBytes(filePath);
                    await dataStream.WriteAsync(fileBytes, 0, fileBytes.Length);
                }

                await writer.WriteLineAsync("226 Transfer complete.");
            }
            catch (Exception ex)
            {
                await writer.WriteLineAsync($"550 Failed to open file. Error: {ex.Message}");
            }
        }


        public void Stop()
        {
            _listener.Stop();
        }
    }


}



