using System.Diagnostics;
using System.Text;

namespace LLMClient;

public partial class Form1 : Form
{
    private Process? CurrentSession;
    private Config CurrentConfig;
    private CancellationTokenSource? _readingCts;

    public Form1()
    {
        InitializeComponent();
        CurrentConfig = ConfigHelper.LoadConfig();
    }

    private async void Form1_Load(object sender, EventArgs e)
    {
        // Laad models uit foundry (direct bij start)
        if (CurrentConfig.ModelList == null)
        {
            CurrentConfig.ModelList = await LoadModelListAsync();
            CurrentConfig.Save();
        }

        if (CurrentConfig.ModelList != null)
        {
            // UI updaten
            //if (ModelCombo.InvokeRequired)
            //{
            //    ModelCombo.Invoke(new Action(() =>
            //    {
            //        ModelCombo.Items.Clear();
            //        ModelCombo.Items.AddRange(CurrentConfig.ModelList);
            //        if (!string.IsNullOrWhiteSpace(CurrentConfig.SelectedModel))
            //        {
            //            ModelCombo.SelectedItem = CurrentConfig.SelectedModel;
            //        }
            //    }));
            //}
            //else
            //{
            ModelCombo.Items.Clear();
            ModelCombo.Items.AddRange(CurrentConfig.ModelList);
            if (!string.IsNullOrWhiteSpace(CurrentConfig.SelectedModel))
            {
                ModelCombo.SelectedItem = CurrentConfig.SelectedModel;
            }
            //}

            // Als er een geselecteerd model in config staat: probeer die te starten
            if (!string.IsNullOrWhiteSpace(CurrentConfig.SelectedModel))
            {
                ModelCombo.SelectedItem = CurrentConfig.SelectedModel;
                await StartModelSessionAsync(CurrentConfig.SelectedModel);
            }
        }
    }

    private async void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
        // Probeer netjes af te sluiten
        await StopCurrentSessionAsync(sendExit: true);
        await RunCommandAsync("foundry", "service stop");
    }

    private async void SendButton_Click(object sender, EventArgs e)
    {
        if (CurrentSession == null || CurrentSession.HasExited)
        {
            MessageBox.Show("Geen actieve sessie om naar te sturen.");
            return;
        }

        var text = PromptTextbox.Text ?? "";
        if (string.IsNullOrWhiteSpace(text)) return;

        // Encodeer enters zodat ze als enters doorgegeven worden (we kiezen hier voor "\n" escapings)
        // Zo kun je in de LLM-repl bijvoorbeeld een speciale decode logica hebben,
        // of de CLI begrijpt daadwerkelijk literal "\n".
        // Als jouw foundry-llm REPL gewoon stdin accepteert met nieuwe regels, gebruik dan: message = text;
        var encoded = StringHelper.EncodeMessagePreserveNewlines(text);

        try
        {
            // write encoded message and then send a newline so the REPL receives it
            await CurrentSession.StandardInput.WriteLineAsync(encoded);
            await CurrentSession.StandardInput.FlushAsync();

            // Optioneel: echo in output (kan handig zijn)
            AppendOutput($"\n> {text}\n");
            PromptTextbox.Clear();
        }
        catch (Exception ex)
        {
            AppendOutput($"\n[Error writing to process stdin: {ex.Message}]\n");
        }
    }

    private async void ModelCombo_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selected = (ModelCombo.SelectedItem ?? "").ToString();
        CurrentConfig.SelectedModel = selected;
        CurrentConfig.Save();

        // Stop huidige session
        await StopCurrentSessionAsync(sendExit: true);

        // Start nieuw model
        if (!string.IsNullOrWhiteSpace(selected))
        {
            await StartModelSessionAsync(selected);
        }
    }


    #region Helpers: encode / append output


    private void AppendOutput(string text)
    {
        if (OutputTextbox.InvokeRequired)
        {
            OutputTextbox.Invoke(new Action(() =>
            {
                OutputTextbox.AppendText(text);
                OutputTextbox.ScrollToCaret();
            }));
        }
        else
        {
            OutputTextbox.AppendText(text);
            OutputTextbox.ScrollToCaret();
        }
    }

    private void SetStatus(string text)
    {
        if (StatusLabel.InvokeRequired)
        {
            StatusLabel.Invoke(new Action(() => StatusLabel.Text = text));
        }
        else
        {
            StatusLabel.Text = text;
        }
    }

    #endregion

    #region Foundry / process handling
    private async Task<string[]?> LoadModelListAsync()
    {
        SetStatus("Models ophalen...");
        try
        {
            var result = await RunCommandCaptureStdoutAsync("foundry", "model list");

            var lines = result.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var models = new List<string>();

            foreach (var line in lines)
            {
                // Sla header en scheidingslijnen over
                if (line.StartsWith("Alias") || line.StartsWith("---")) continue;

                // Splits de regel op whitespace
                var parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length > 0)
                {
                    // laatste kolom is altijd het Model ID
                    var modelId = parts[^1];
                    models.Add(modelId);
                }
            }

            SetStatus($"Models geladen ({models.Count})");
            return models.ToArray();
        }
        catch (Exception ex)
        {
            SetStatus("Kon models niet ophalen");
            //AppendOutput($"[Error loading model list: {ex.Message}]\n");
        }
        return null;
    }

    private Task StartModelSessionAsync(string modelName)
    {
        SetStatus($"Start model {modelName}...");
        //AppendOutput($"\n[Starting model {modelName}]\n");

        // Run "foundry model start <model>" and keep process open (interactive)
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "foundry",
                Arguments = $"model run {EscapeArgument(modelName)}",
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8
            };

            var proc = new Process { StartInfo = psi };
            proc.Start();

            CurrentSession = proc;

            // Cancel any previous read
            _readingCts?.Cancel();
            _readingCts = new CancellationTokenSource();

            // Start byte-by-byte reader for stdout
            _ = Task.Run(() => ReadStdOutByteByByte(proc, _readingCts.Token));

            // Start reader for stderr (line-based but shown)
            _ = Task.Run(() => ReadStdErr(proc, _readingCts.Token));

            SetStatus($"Model {modelName} gestart");
        }
        catch (Exception ex)
        {
            AppendOutput($"[Error starting model: {ex.Message}]\n");
            SetStatus("Start mislukt");
        }

        return Task.CompletedTask;
    }

    private async Task StopCurrentSessionAsync(bool sendExit)
    {
        if (CurrentSession == null) return;

        try
        {
            SetStatus("Sessiestop...");
            if (sendExit && !CurrentSession.HasExited)
            {
                try
                {
                    // stuur exit commando naar de actieve sessie
                    await CurrentSession.StandardInput.WriteLineAsync("/exit");
                    await CurrentSession.StandardInput.FlushAsync();
                }
                catch
                {
                    // ignore write failures
                }
            }

            // Allow some time for graceful exit
            var exited = CurrentSession.WaitForExit(2500);
            if (!exited)
            {
                try
                {
                    CurrentSession.Kill(true);
                }
                catch { }
            }

            // Cancel readers
            _readingCts?.Cancel();
            _readingCts = null;

            //AppendOutput("\n[Session stopped]\n");
            CurrentSession.Dispose();
            CurrentSession = null;
            SetStatus("Sessie gestopt");
        }
        catch (Exception ex)
        {
            AppendOutput($"[Error stopping session: {ex.Message}]\n");
        }
    }

    #endregion

    #region Low-level IO

    private async Task ReadStdOutByteByByte(Process proc, CancellationToken ct)
    {
        try
        {
            var stream = proc.StandardOutput.BaseStream;
            var buffer = new byte[1];

            while (!ct.IsCancellationRequested && proc != null && !proc.HasExited)
            {
                var read = await stream.ReadAsync(buffer, 0, 1, ct).ConfigureAwait(false);
                if (read == 0)
                {
                    // EOF - break
                    break;
                }

                // decode byte(s) to UTF8 string
                var s = Encoding.UTF8.GetString(buffer, 0, read);

                // append to output textbox on UI thread
                AppendOutput(s);
            }
        }
        catch (OperationCanceledException)
        {
            // expected during stop
        }
        catch (Exception ex)
        {
            AppendOutput($"\n[Error reading stdout: {ex.Message}]\n");
        }
    }

    private async Task ReadStdErr(Process proc, CancellationToken ct)
    {
        try
        {
            var sr = proc.StandardError;
            while (!ct.IsCancellationRequested && !proc.HasExited)
            {
                var line = await sr.ReadLineAsync();
                if (line == null) break;
                AppendOutput($"\n[stderr] {line}\n");
            }
        }
        catch (OperationCanceledException) { }
        catch (Exception ex)
        {
            AppendOutput($"\n[Error reading stderr: {ex.Message}]\n");
        }
    }

    private static string EscapeArgument(string arg)
    {
        // Minimal escaping for spaces
        if (arg.Contains(" "))
        {
            return $"\"{arg}\"";
        }
        return arg;
    }

    private async Task<string> RunCommandCaptureStdoutAsync(string fileName, string arguments)
    {
        var psi = new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = arguments,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            CreateNoWindow = true,
            StandardOutputEncoding = Encoding.UTF8
        };

        using var proc = new Process { StartInfo = psi };
        var sb = new StringBuilder();
        proc.Start();
        using var sr = proc.StandardOutput;
        while (!sr.EndOfStream)
        {
            var line = await sr.ReadLineAsync();
            if (line != null)
            {
                sb.AppendLine(line);
            }
        }
        proc.WaitForExit();
        return sb.ToString();
    }

    private async Task<int> RunCommandAsync(string fileName, string arguments)
    {
        var psi = new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = arguments,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var proc = new Process { StartInfo = psi };
        proc.Start();
        await proc.WaitForExitAsync();
        return proc.ExitCode;
    }

    #endregion

}

