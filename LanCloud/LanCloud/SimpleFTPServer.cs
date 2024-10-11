using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System;

namespace LanCloud
{
    public class SimpleFTPServer
    {
        private TcpListener Listener { get; set; }
        private string TransferMode {  get; set; }
        private string BaseDirectory { get; set; }
        private IPAddress DataConnectionAddress { get; set; }
        private int DataConnectionPort { get; set; }
        private TcpListener PassiveListener { get; set; }

        public SimpleFTPServer(string ipAddress, int port, string baseDirectory)
        {
            Listener = new TcpListener(IPAddress.Parse(ipAddress), port);
            BaseDirectory = baseDirectory;
        }

        public async Task StartAsync()
        {
            Listener.Start();
            Console.WriteLine("FTP Server is gestart...");

            using (var logstream = File.OpenWrite("log.txt"))
            using (var logwriter = new StreamWriter(logstream))
            {
                while (true)
                {
                    var client = await Listener.AcceptTcpClientAsync();
                    Console.WriteLine("Client verbonden.");
                    _ = HandleClientAsync(client, logwriter); // Handle each client in a separate task
                }
            }
        }

        private async Task HandleClientAsync(TcpClient client, StreamWriter logwriter)
        {
            using (var networkStream = client.GetStream())
            using (var networkReader = new StreamReader(networkStream, Encoding.ASCII))
            using (var networkWriter = new StreamWriter(networkStream, Encoding.ASCII) { AutoFlush = true })
            {
                IPEndPoint remoteEndPoint = client.Client.RemoteEndPoint as IPEndPoint;
                string ipAdress = remoteEndPoint?.Address?.ToString() ?? "Onbekend";

                var reader = new LoggedStreamReader(logwriter, networkReader, ipAdress);
                var writer = new LoggedStreamWriter(logwriter, networkWriter, ipAdress);

                await writer.WriteLineAsync("220 LanCloud FTP Server Ready");

                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    var commandParts = line.Split(' ');
                    var command = commandParts[0].ToUpper();
                    var argument = commandParts.Length > 1 ? line.Substring(command.Length).Trim() : null;
                    await HandleCommand(client, networkStream, writer, command, argument);
                }
            }
        }

        private async Task HandleCommand(TcpClient client, NetworkStream networkStream, LoggedStreamWriter writer, string command, string argument)
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
                    await writer.WriteLineAsync($"257 \"{BaseDirectory}\" is de huidige directory");
                    break;
                case "CWD":
                    await writer.WriteLineAsync("250 Directory changed successfully.");
                    break;
                case "LIST":
                    await HandleListCommand(writer, networkStream);
                    break;
                case "RETR":
                    await HandleRetrCommand(networkStream, writer, argument);
                    break;
                case "TYPE":
                    await HandleTypeCommand(writer, argument);
                    break;
                case "PORT":
                    await HandlePortCommand(writer, argument);
                    break;
                case "QUIT":
                    await HandleQuit(client, writer);
                    return;
                default:
                    await HandleNotImplemented(writer);
                    break;
            }
        }





        



        

        private async Task HandleListCommand(LoggedStreamWriter writer, NetworkStream controlStream)
        {
            try
            {
                var directoryInfo = new DirectoryInfo(BaseDirectory);
                var files = directoryInfo.GetFiles();

                await writer.WriteLineAsync("150 Opening ASCII mode data connection for directory list.");

                TcpClient dataClient;
                if (PassiveListener != null)
                {
                    // In passieve modus: wachten op inkomende verbinding van de client
                    dataClient = await PassiveListener.AcceptTcpClientAsync();
                    PassiveListener.Stop();
                    PassiveListener = null;
                }
                else
                {
                    // In actieve modus: verbind met de client (IP/poort opgegeven via PORT-commando)
                    dataClient = new TcpClient();
                    await dataClient.ConnectAsync(DataConnectionAddress, DataConnectionPort);
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


        private async Task HandleRetrCommand(NetworkStream networkStream, LoggedStreamWriter writer, string argument)
        {
            if (argument != null)
            {
                var filename = argument;
                var filePath = Path.Combine(BaseDirectory, filename);
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
                        if (PassiveListener != null)
                        {
                            // Passieve modus
                            dataClient = await PassiveListener.AcceptTcpClientAsync();
                            PassiveListener.Stop();
                            PassiveListener = null;
                        }
                        else
                        {
                            // Actieve modus
                            dataClient = new TcpClient();
                            await dataClient.ConnectAsync(DataConnectionAddress, DataConnectionPort);
                        }
                    }
                    catch (Exception)
                    {
                        // Actieve modus faalt: overschakelen naar passieve modus
                        await HandlePasvCommand(writer);
                        dataClient = await PassiveListener.AcceptTcpClientAsync();
                        PassiveListener.Stop();
                        PassiveListener = null;
                    }

                    // Overdracht van het bestand
                    using (var dataStream = dataClient.GetStream())
                    {
                        using (var fileStream = File.OpenRead(filePath))
                        {
                            fileStream.CopyTo(dataStream);
                        }

                        //byte[] fileBytes = File.ReadAllBytes(filePath);
                        //await dataStream.WriteAsync(fileBytes, 0, fileBytes.Length);
                    }

                    await writer.WriteLineAsync("226 Transfer complete.");
                }
                catch (Exception ex)
                {
                    await writer.WriteLineAsync($"550 Failed to open file. Error: {ex.Message}");
                }
            }
            else
            {
                await writer.WriteLineAsync("501 Syntax error in parameters or arguments.");
            }
        }
        private async Task HandlePasvCommand(LoggedStreamWriter writer)
        {
            // Open een nieuwe socket in passieve modus op een willekeurige poort
            PassiveListener = new TcpListener(IPAddress.Any, 0); // 0 betekent dat we een willekeurige poort gebruiken
            PassiveListener.Start();
            var localEndPoint = PassiveListener.LocalEndpoint as IPEndPoint;

            // IP-adres van de server en poort in het juiste formaat
            var address = ((IPEndPoint)PassiveListener.LocalEndpoint).Address;
            var port = ((IPEndPoint)PassiveListener.LocalEndpoint).Port;

            var addressParts = address.ToString().Split('.');
            var portHigh = port / 256;
            var portLow = port % 256;

            // Stuur het 227-antwoord terug naar de client, met de IP en poort
            await writer.WriteLineAsync($"227 Entering Passive Mode ({string.Join(",", addressParts)},{portHigh},{portLow})");
        }
        private async Task HandleTypeCommand(LoggedStreamWriter writer, string argument)
        {
            //if (argument != null && argument.ToUpper() == "A")
            //{
            //    TransferMode = "A"; // Set to ASCII mode
            //    await writer.WriteLineAsync("200 Type set to ASCII mode.");
            //}
            //else
            if (argument != null && argument.ToUpper() == "I")
            {
                TransferMode = "I"; // Set to Binary mode
                await writer.WriteLineAsync("200 Type set to Binary mode.");
            }
            else
            {
                await writer.WriteLineAsync("504 Command not implemented for that parameter.");
            }
        }
        private async Task HandlePortCommand(LoggedStreamWriter writer, string argument)
        {
            if (argument == null)
            {
                await writer.WriteLineAsync("501 Syntax error in parameters or arguments.");
            }

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

            DataConnectionAddress = IPAddress.Parse(ipAddress);
            DataConnectionPort = port;

            Console.WriteLine($"PORT command received, IP: {DataConnectionAddress}, Port: {DataConnectionPort}");

            await writer.WriteLineAsync("200 PORT command successful.");
        }
        private static async Task HandleQuit(TcpClient client, LoggedStreamWriter writer)
        {
            await writer.WriteLineAsync("221 Goodbye.");
            client.Close();
        }
        private static async Task HandleNotImplemented(LoggedStreamWriter writer)
        {
            await writer.WriteLineAsync("502 Command not implemented");
        }

        public void Stop()
        {
            Listener.Stop();
        }
    }


}



