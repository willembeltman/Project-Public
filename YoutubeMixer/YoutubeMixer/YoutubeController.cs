using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System.Diagnostics;
using YoutubeMixer.UserControls;
using System.Windows.Forms;

namespace YoutubeMixer
{
    /// <summary>
    /// Todo:
    /// - Mixer aansluiten / berekenen
    /// - Pitch range aansluiten
    /// - Pitch aansluiten
    /// - Pitch control aansluiten
    /// - Hele cue systeem
    /// </summary>

    public class YoutubeController : IController
    {
        public YoutubeController(Deck deck, MixerChannel mixerChannel)
        {
            Deck = deck;
            Deck.Controller = this;

            MixerChannel = mixerChannel;
            MixerChannel.Controller = this;

            Driver = new ChromeDriver();
            JsExecutor = Driver;
        }

        private Deck Deck { get; }
        private MixerChannel MixerChannel { get; }
        private ChromeDriver Driver { get; }
        private IJavaScriptExecutor JsExecutor { get; }

        private bool _SetVolume { get; set; }
        private bool _SetEqualizer { get; set; }
        private bool _SetPlaybackSpeed { get; set; }

        private double _Volume { get; set; }
        private double _BassVolume { get; set; }
        private double _MidVolume { get; set; }
        private double _HighVolume { get; set; }
        private double _PlaybackSpeed { get; set; } = 1;

        private IWebElement? VideoElement { get; set; }

        public void SetVolume(double volume)
        {
            _Volume = volume;
            _SetVolume = true;
        }
        public void SetEqualizer(double bassVolume, double midVolume, double highVolume)
        {
            _BassVolume = bassVolume;
            _MidVolume = midVolume;
            _HighVolume = highVolume;
            _SetEqualizer = true;
        }
        public void SetPlaybackSpeed(double playbackSpeed)
        {
            _PlaybackSpeed = playbackSpeed;
            _SetVolume = true;
        }

        public void Start()
        {
            // Navigate browser to YouTube
            Driver.Navigate().GoToUrl("https://www.youtube.com");
        }
        public bool Loop()
        {
            try
            {
                // If the video element has not been loaded
                if (VideoElement == null)
                {
                    // Try to ge the video element
                    VideoElement = Driver.FindElement(By.CssSelector("video.html5-main-video"));

                    // Try to inject the equalizer and vu meter
                    TryInjectEqualizerAndVuMeter();
                }

                // If the video element has been found
                if (VideoElement != null)
                {
                    // If video is playing
                    if (VideoElement.GetAttribute("paused") != "true")
                    {
                        // Do playing operations
                        SyncVideoInformation();
                        SetPitchbendIfNeeded();
                        SetVolumeIfNeeded();
                        SetEqualizerIfNeeded();

                        // Ask for short delay
                        return true;
                    }
                    else
                    {
                        // Reset videoelement and try again
                        VideoElement = null;
                    }
                }
            }
            catch (Exception ex)
            {
                // Error occured, write error to console
                Debug.WriteLine(ex.Message);

                // Reset videoelement and try again
                VideoElement = null;
            }
            return false;
        }
        public void Stop()
        {
            // Quit the Chrome driver
            Driver.Quit();
        }

        private void TryInjectEqualizerAndVuMeter()
        {
            bool hasAttribute = (bool)JsExecutor.ExecuteScript("return arguments[0].hasAttribute('eq-injected')", VideoElement);
            if (!hasAttribute)
            {
                string filterCode = @"
                    var context = new AudioContext();
                    var source = context.createMediaElementSource(arguments[0]);

                    // create an equalizer using BiquadFilterNodes
                    var filters = [];
                    var frequencies = [60, 170, 350, 1000, 3500, 10000];
                    var gains = [0, 0, 0, 0, 0, 0];
                    for (var i = 0; i < frequencies.length; i++) {
                        var filter = context.createBiquadFilter();
                        filter.type = 'peaking';
                        filter.frequency.value = frequencies[i];
                        filter.gain.value = gains[i];
                        filters.push(filter);
                    }

                    // connect the filters in series
                    source.connect(filters[0]);
                    for (var i = 0; i < filters.length - 1; i++) {
                        filters[i].connect(filters[i + 1]);
                    }
                    filters[filters.length - 1].connect(context.destination);


                    // function to update filter parameters
                    function updateFilterParams(newGains) {
                        for (var i = 0; i < filters.length; i++) {
                            filters[i].gain.value = newGains[i];
                        }
                    }

                    // expose the updateFilterParams function to the global scope
                    window.updateFilterParams = updateFilterParams;





                    // create a meter using a ScriptProcessorNode
                    var bufferSize = 512;
                    var meter = context.createScriptProcessor(bufferSize, 1, 1);
                    var maxLevel = 0;

                    meter.onaudioprocess = function(event) {
                        try {
                            var inputBuffer = event.inputBuffer;
                            var inputData1 = inputBuffer.getChannelData(0);
                            for (var i = 0; i < inputData1.length; i++) {
                                let data = inputData1[i];
                                if (data < 0) {
                                    data = data * -1;           
                                }
                                if (maxLevel < data) {
                                    maxLevel = data;           
                                }
                            }
                        }
                        catch (e) {
                            console.log(e);
                        }
                    }

                    // connect the meter to the source
                    source.connect(meter);
                    meter.connect(context.destination);

                    // function to get the current vu-meter reading
                    function getVuMeter() {
                        let res = maxLevel;
                        maxLevel = 0;
                        return res;
                    }

                    // expose the getVuMeter function to the global scope
                    window.getVuMeter = getVuMeter;



                    arguments[0].setAttribute('eq-injected', 'true');
                ";

                JsExecutor.ExecuteScript(filterCode, VideoElement);

            }
        }

        private void SetEqualizerIfNeeded()
        {
            if (_SetEqualizer)
            {
                _SetEqualizer = false;
                double[] newGains = { _BassVolume, _BassVolume, _MidVolume, _MidVolume, _HighVolume, _HighVolume };
                JsExecutor.ExecuteScript("window.updateFilterParams(arguments[0]);", newGains);
            }
        }
        private void SetVolumeIfNeeded()
        {
            if (_SetVolume)
            {
                _SetVolume = false;
                JsExecutor.ExecuteScript("arguments[0].volume = arguments[1];", VideoElement, _Volume);
            }
        }
        private void SetPitchbendIfNeeded()
        {
            var pitchbendState = Deck.GetPitchbendState();
            if (pitchbendState.IsDragging)
            {
                _SetPlaybackSpeed = true;

                int deltaY = pitchbendState.DeltaY;
                Debug.WriteLine($"DeltaY = {deltaY}");

                if (pitchbendState.DeltaY > 0)
                {
                    double speedChange = deltaY * 0.01; // adjust this constant to control the sensitivity of the control
                    double newSpeed = Math.Max(0.25, Math.Min(4.0, _PlaybackSpeed + speedChange)); // limit the speed to a reasonable range
                    JsExecutor.ExecuteScript($"arguments[0].playbackRate = {newSpeed.ToString("F3").Replace(",", ".")};", VideoElement);
                }
                else
                {
                    double speedChange = deltaY * 0.005; // adjust this constant to control the sensitivity of the control
                    double newSpeed = Math.Max(0.25, Math.Min(4.0, _PlaybackSpeed + speedChange)); // limit the speed to a reasonable range
                    JsExecutor.ExecuteScript($"arguments[0].playbackRate = {newSpeed.ToString("F3").Replace(",", ".")};", VideoElement);
                }
            }
            else if (_SetPlaybackSpeed)
            {
                JsExecutor.ExecuteScript($"arguments[0].playbackRate = {_PlaybackSpeed.ToString("F3").Replace(",", ".")};", VideoElement);
                _SetPlaybackSpeed = false;
            }
        }
        private void SyncVideoInformation()
        {
            var title = Driver?.Title ?? "";
            var state = (Dictionary<string, object>)JsExecutor
                .ExecuteScript(@"return { vuMeter: window.getVuMeter(), currentTime: arguments[0].currentTime, totalDuration: arguments[0].duration };", VideoElement);
            var vuMeter = Convert.ToDouble(state["vuMeter"]);
            var currentTime = Convert.ToDouble(state["currentTime"]);
            var totalDuration = Convert.ToDouble(state["totalDuration"]);

            MixerChannel.SetVuMeter(vuMeter);
            Deck.SetVideoInformation(title, currentTime, totalDuration);
        }


    }
}
