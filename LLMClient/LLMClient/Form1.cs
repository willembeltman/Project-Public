using Newtonsoft.Json;
using System.Diagnostics;

namespace LLMClient;

public partial class Form1 : Form
{
    private const string ConfigFilename = "config.json";
    private Process? CurrentSession;
    private Config CurrentConfig;

    public Form1()
    {
        InitializeComponent();

        CurrentConfig = new Config();
        if (File.Exists(ConfigFilename))
        {
            var configJson = File.ReadAllText(ConfigFilename);
            CurrentConfig = JsonConvert.DeserializeObject<Config>(configJson) ?? new Config();
        }

        if (CurrentConfig.ModelList == null)
        {
            // LLM Lijst inladen met command: "foundry model list"
        }
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        // LLM opstarten met command "foundry model start <modelname>"

        // Process in leven houden in 
        CurrentSession = new Process();

        // Start async process wat de outputstream van het process uitleest en direct wegschrijft naar 
        OutputTextbox.Text += ""; // Let op niet OutputDataReceived maar gewoon byte voor byte uitlezen zodat je de streaming ziet.
    }

    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
        // LLM afsluiten met "/exit"
        
        // Dan de service stoppen met "foundry service stop"
    }

    private void SendButton_Click(object sender, EventArgs e)
    {
        // Hier moeten we het bericht versturen naar de llm

        // Let op: Bericht moet ge-encode worden zodat de enters doorgegeven kunnen worden.
    }

    private void ModelCombo_SelectedIndexChanged(object sender, EventArgs e)
    {
        CurrentConfig.SelectedModel = (ModelCombo.SelectedValue ?? "").ToString();
        SaveConfig();

        // Stop huidige session stoppen

        // Opnieuw opstarten met dropdown keuze
    }

    private void SaveConfig()
    {
        var configJson = JsonConvert.SerializeObject(CurrentConfig);
        File.WriteAllText(ConfigFilename, configJson);
    }

    public class Config
    {
        public DateTime? LastUpdate { get; set; }
        public string? SelectedModel { get; set; }
        public string[]? ModelList { get; set; }
    }
}
