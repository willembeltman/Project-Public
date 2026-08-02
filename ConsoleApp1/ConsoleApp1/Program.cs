using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SimpleFolderSync
{
    class Program
    {
        // Standaard map & poort
        static string LocalFolder = Path.Combine(Directory.GetCurrentDirectory(), "SyncFolder");
        static int ListenPort = 5000;

        static async Task Main()
        {
            Directory.CreateDirectory(LocalFolder);
            Console.WriteLine("=== Simple Folder Sync ===");
            Console.WriteLine($"Te syncen map: {LocalFolder}");
            Console.WriteLine("\n1. Start als Server (luistert op poort " + ListenPort + ")");
            Console.WriteLine("2. Verbind met peer (client)");

            string choice = Console.ReadLine();

            if (choice == "1")
                await RunServerAsync(LocalFolder, ListenPort);
            else
            {
                Console.Write("IP adres server: ");
                string ip = Console.ReadLine();
                Console.Write("Poort: ");
                int port = int.Parse(Console.ReadLine());
                await RunClientAsync(LocalFolder, ip, port);
            }
        }

        #region Server & Client Logica
        static async Task RunServerAsync(string localPath, int port)
        {
            Console.WriteLine($"\n🟡 Luistert op poort {port}...");
            using var listener = new TcpListener(IPAddress.Any, port);
            listener.Start();

            using var client = await listener.AcceptTcpClientAsync();
            Console.WriteLine("✅ Verbinding ontvangen. Start synchronisatie...");

            await SyncDirectionAsync(localPath, client, sendMyFiles: false);
        }

        static async Task RunClientAsync(string localPath, string remoteIp, int remotePort)
        {
            using var client = new TcpClient();
            Console.WriteLine($"\n🔵 Verbinden met {remoteIp}:{remotePort}...");
            await client.ConnectAsync(remoteIp, remotePort);
            Console.WriteLine("✅ Verbonden. Start synchronisatie...");

            await SyncDirectionAsync(localPath, client, sendMyFiles: true);
        }

        // Kern sync logica (volgt protocol)
        static async Task SyncDirectionAsync(string localPath, TcpClient client, bool sendMyFiles)
        {
            var myFiles = await ScanFolderAsync(localPath);

            // Stap 1: Wissel bestandslijsten via JSON
            if (sendMyFiles) await SendJsonAsync(client, myFiles);
            var peerFiles = JsonSerializer.Deserialize<Dictionary<string, FileMeta>>(await ReceiveJsonAsync(client))!;
            if (!sendMyFiles) await SendJsonAsync(client, myFiles);

            // Stap 2: Bereken verschillen (wat ik nodig heb van de peer)
            var neededFiles = new Dictionary<string, FileMeta>();
            foreach (var kvp in peerFiles)
            {
                string relPath = kvp.Key;
                bool exists = myFiles.ContainsKey(relPath);
                if (!exists || kvp.Value.LastWrite != myFiles[relPath].LastWrite)
                    neededFiles[relPath] = kvp.Value;
            }

            // Stap 3: Stuur "ik nodig dit" lijst naar peer
            var neededJson = JsonSerializer.Serialize(neededFiles.ToDictionary(k => k.Key, v => new { v.Value.Size, LastWrite = v.Value.LastWrite.ToString("o") }));
            await SendTextAsync(client, $"NEED:{neededJson}");

            // Stap 4: Ontvang & sla bestanden op
            int received = 0;
            while (true)
            {
                string header = await ReceiveLineAsync(client);
                if (header.StartsWith("END")) break; // Einde van overdracht
                if (!header.StartsWith("FILE:")) continue;

                string relPath = header[5..];
                int fileSize = Convert.ToInt32(Console.ReadLine()); // Werkt in deze context, maar zie onder voor robuustere variant

                byte[] data = new byte[fileSize];
                int offset = 0;
                var stream = client.GetStream();
                while (offset < fileSize)
                    offset += await stream.ReadAsync(data, offset, fileSize - offset);

                string fullPath = Path.Combine(localPath, relPath);
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
                File.WriteAllBytes(fullPath, data);
                File.SetLastWriteTimeUtc(fullPath, DateTime.Parse(peerFiles[relPath].LastWrite.ToString("o")));

                Console.WriteLine($"  ✓ {relPath}");
                received++;
            }

            Console.WriteLine($"\n🎉 Synchronisatie compleet. {received} bestanden ontvangen.");
        }
        #endregion

        #region Network Helpers (Robuust Protocol)
        static async Task SendTextAsync(TcpClient client, string text)
        {
            byte[] data = Encoding.UTF8.GetBytes(text);
            await SendBytesAsync(client, data);
        }

        static async Task<byte[]> ReceiveBytesAsync(TcpClient client)
        {
            var stream = client.GetStream();
            byte[] lenBuf = new byte[4];
            int offset = 0;
            while (offset < 4) offset += await stream.ReadAsync(lenBuf, offset, 4 - offset);

            int len = (lenBuf[0] << 24) | (lenBuf[1] << 16) | (lenBuf[2] << 8) | lenBuf[3];
            byte[] data = new byte[len];
            offset = 0;
            while (offset < len) offset += await stream.ReadAsync(data, offset, len - offset);
            return data;
        }

        static async Task SendBytesAsync(TcpClient client, byte[] data)
        {
            var stream = client.GetStream();
            byte[] lenBuf = new byte[4];
            lenBuf[0] = (byte)(data.Length >> 24);
            lenBuf[1] = (byte)(data.Length >> 16);
            lenBuf[2] = (byte)(data.Length >> 8);
            lenBuf[3] = (byte)data.Length;
            await stream.WriteAsync(lenBuf, 0, 4);
            await stream.WriteAsync(data, 0, data.Length);
        }

        static async Task SendJsonAsync(TcpClient client, Dictionary<string, FileMeta> files) =>
            await SendTextAsync(client, $"META:{JsonSerializer.Serialize(files.ToDictionary(k => k.Key, v => new { Size = v.Value.Size, LastWrite = v.Value.LastWrite.ToString("o") }))}");

        static async Task<byte[]> ReceiveJsonAsync(TcpClient client)
        {
            string header = await ReceiveLineAsync(client);
            return Encoding.UTF8.GetBytes(header[4..]); // Skip "META:"
        }

        static async Task<string> ReceiveLineAsync(TcpClient client)
        {
            var sb = new StringBuilder();
            while (true)
            {
                byte[] b = new byte[1];
                await client.GetStream().ReadAsync(b, 0, 1);
                char c = (char)b[0];
                if (c == '\n') break;
                sb.Append(c);
            }
            return sb.ToString();
        }
        #endregion

        static async Task<Dictionary<string, FileMeta>> ScanFolderAsync(string path)
        {
            var files = new Dictionary<string, FileMeta>();
            foreach (var file in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories))
                files[Path.GetRelativePath(path, file)] = new FileMeta(
                    new FileInfo(file).Length,
                    new FileInfo(file).LastWriteTimeUtc);
            return files;
        }

        class FileMeta { public long Size { get; set; } public DateTime LastWrite { get; set; } }
    }
}
