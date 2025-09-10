using System.Diagnostics;
using System.Text;

namespace LLMClient;

public partial class MainForm : Form
{
    private Process? CurrentSession;
    private Config CurrentConfig;
    private CancellationTokenSource? _readingCts;

    public MainForm()
    {
        InitializeComponent();
        CurrentConfig = ConfigHelper.LoadConfig();
    }

    private async void MainForm_Load(object sender, EventArgs e)
    {
        // Laad models uit foundry (direct bij start)
        if (CurrentConfig.ModelList == null || 
            CurrentConfig.LastUpdate == null || 
            CurrentConfig.LastUpdate < DateTime.UtcNow.AddMonths(-1))
        {
            try
            {
                SetStatus("Models ophalen...");
                CurrentConfig.ModelList = await ModelRepository.LoadModelListAsync();
                CurrentConfig.LastUpdate = DateTime.UtcNow;
                CurrentConfig.Save();
            }
            catch (Exception ex)
            {
                SetStatus("Kon models niet ophalen");
                AppendOutput($"[Error loading model list: {ex.Message}]\n");
            }
        }

        if (CurrentConfig.ModelList != null)
        {
            UpdateModelList(CurrentConfig.ModelList, CurrentConfig.SelectedModel);
        }
    }

    private async void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        // Probeer netjes af te sluiten
        await StopCurrentSessionAsync(sendExitCommand: true);
        await ProcessHelper.RunCommandAsync("foundry", "service stop");
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
        var encoded = text.EncodeMessagePreserveNewlines();

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
        await StopCurrentSessionAsync(sendExitCommand: true);

        // Start nieuw model
        if (!string.IsNullOrWhiteSpace(selected))
        {
            await StartModelSessionAsync(selected);
        }
    }

    private Task StartModelSessionAsync(string modelName)
    {
        SetStatus($"Start model {modelName}...");
        AppendOutput($"\n[Starting model {modelName}]\n");

        // Run "foundry model start <model>" and keep process open (interactive)
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "foundry",
                Arguments = $"model run {modelName.EscapeArgument()}",
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
            _ = Task.Run(() => ReadStdOut(proc, _readingCts.Token));

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

    private async Task StopCurrentSessionAsync(bool sendExitCommand)
    {
        if (CurrentSession == null) return;

        try
        {
            SetStatus("Sessiestop...");
            if (sendExitCommand && !CurrentSession.HasExited)
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

    #region Low-level IO

    private async Task ReadStdOut(Process proc, CancellationToken ct)
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

    #endregion

    #region Gui

    private void UpdateModelList(string[] modelList, string? selectedModel)
    {
        if (ModelCombo.InvokeRequired)
        {
            ModelCombo.Invoke(new Action(() =>
            {
                UpdateModelList(modelList, selectedModel);
            }));
        }
        else
        {
            ModelCombo.Items.Clear();
            ModelCombo.Items.AddRange(modelList);
            if (!string.IsNullOrWhiteSpace(selectedModel))
            {
                ModelCombo.SelectedItem = selectedModel; // Causes model to be started
            }
        }
    }

    private void AppendOutput(string text)
    {
        if (OutputTextbox.InvokeRequired)
        {
            OutputTextbox.Invoke(new Action(() => AppendOutput(text)));
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
            StatusLabel.Invoke(new Action(() => SetStatus(text)));
        }
        else
        {
            StatusLabel.Text = text;
        }
    }

    #endregion
}

