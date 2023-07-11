using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System.Diagnostics;
using YoutubeMixer.UserControls;

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

    public class YoutubeController
    {
        public YoutubeController(Deck deck, MixerChannel mixerChannel)
        {
            Deck = deck;
            Deck.Controller = this;

            MixerChannel = mixerChannel;
            MixerChannel.Controller = this;

            Driver = new ChromeDriver();
            JsExecutor = Driver;

            Thread = new Thread(new ThreadStart(Thread_Start));
        }

        private Deck Deck { get; }
        private MixerChannel MixerChannel { get; }
        private ChromeDriver Driver { get; }
        private IJavaScriptExecutor JsExecutor { get; }
        private Thread Thread { get; }

        private bool KillSwitch { get; set; }
        private bool ResetVolume { get; set; }
        private bool ResetEqualizer { get; set; }
        private bool ResetPlaybackSpeed { get; set; }

        private double Volume { get; set; }
        private double BassVolume { get; set; }
        private double MidVolume { get; set; }
        private double HighVolume { get; set; }
        private double PlaybackSpeed { get; set; } = 1;

        public void SetVolume(double volume)
        {
            Volume = volume;
            ResetVolume = true;
        }
        public void SetEqualizer(double bassVolume, double midVolume, double highVolume)
        {
            BassVolume = bassVolume;
            MidVolume = midVolume;
            HighVolume = highVolume;
            ResetEqualizer = true;
        }
        public void SetPlaybackSpeed(double playbackSpeed)
        {
            PlaybackSpeed = playbackSpeed;
            ResetVolume = true;
        }

        public void Start()
        {
            Thread.Start();
        }
        private void Thread_Start()
        {
            // Navigate browser to YouTube
            Driver.Navigate().GoToUrl("https://www.youtube.com");

            // Wait for the video to load and pause it
            Thread_Mainloop();

            // Quit the Chrome driver
            Driver.Quit();


            // Wait for the videos to load and pause them
            //WaitForAndPauseVideo();
            //WaitForAndPauseVideo(driver2);

            // Get the URLs of the paused videos
            //string videoUrl1 = GetPausedVideoUrl(driver1);
            //string videoUrl2 = GetPausedVideoUrl(driver2);

            // Process the video URLs in your program
            //ProcessVideoUrls(videoUrl1);//, videoUrl2);

            // Quit the Chrome drivers
            //driver1.Quit();
            //driver2.Quit();

            //DrawPlaybackDisplay("My Video Title", TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(60), 0.5);
        }
        private void Thread_Mainloop()
        {
            while (!KillSwitch)
            {
                try
                {
                    var video = GetVideoElement();
                    InjectEqualizer(video);
                    TrackWhilePlaying(video);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    Thread.Sleep(100);
                }
            }
        }

        private IWebElement GetVideoElement()
        {
            // Wait for the video to load
            //var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            var video = Driver.FindElement(By.CssSelector("video.html5-main-video"));
            while (!KillSwitch && video.GetAttribute("paused") == "true")
            {
                Thread.Sleep(100);
            }

            return video;
        }

        private void InjectEqualizer(IWebElement video)
        {
            bool hasAttribute = (bool)JsExecutor.ExecuteScript("return arguments[0].hasAttribute('eq-injected')", video);
            if (!hasAttribute)
            {
                JsExecutor.ExecuteScript("arguments[0].setAttribute('eq-injected', 'true')", video);

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

                    // play the video
                    arguments[0].play();

                    // function to update filter parameters
                    function updateFilterParams(newGains) {
                        for (var i = 0; i < filters.length; i++) {
                            filters[i].gain.value = newGains[i];
                        }
                    }

                    // expose the updateFilterParams function to the global scope
                    window.updateFilterParams = updateFilterParams;





                    // create a meter using a ScriptProcessorNode
                    var bufferSize = 2048;
                    var meter = context.createScriptProcessor(bufferSize, 1, 1);
                    var sampleBuffer = new Float32Array(bufferSize);
                    var rms = 0;

                    meter.onaudioprocess = function(event) {
                        var inputBuffer = event.inputBuffer;
                        var inputData = inputBuffer.getChannelData(0);
                        for (var i = 0; i < inputData.length; i++) {
                            sampleBuffer[i] = inputData[i];
                        }
                        // calculate the RMS level of the samples
                        rms = 0;
                        for (var i = 0; i < sampleBuffer.length; i++) {
                            rms += sampleBuffer[i] * sampleBuffer[i];
                        }
                        rms = Math.sqrt(rms / sampleBuffer.length);
                    }

                    // connect the meter to the source
                    source.connect(meter);
                    meter.connect(context.destination);

                    // play the video
                    arguments[0].play();

                    // function to get the current vu-meter reading
                    function getVuMeter() {
                        return rms;
                    }

                    // expose the getVuMeter function to the global scope
                    window.getVuMeter = getVuMeter;


                ";

                JsExecutor.ExecuteScript(filterCode, video);
            }
        }
        private void TrackWhilePlaying(IWebElement video)
        {
            while (!KillSwitch && video.GetAttribute("paused") != "true")
            {
                if (MixerChannel != null && Deck != null)
                {
                    SyncVideoInformation(video);
                    SetPitchbendIfNeeded(video);
                    SetVolumeIfNeeded(video);
                    SetEqualizerIfNeeded(video);
                }

                // Sleep the thread for 16 ms (60fps)
                Thread.Sleep(16);
            }
        }
        private void SetEqualizerIfNeeded(IWebElement video)
        {
            if (ResetEqualizer)
            {
                ResetEqualizer = false;
                double[] newGains = { BassVolume, BassVolume, MidVolume, MidVolume, HighVolume, HighVolume };
                JsExecutor.ExecuteScript("window.updateFilterParams(arguments[0]);", newGains);
            }
        }
        private void SetVolumeIfNeeded(IWebElement video)
        {
            if (ResetVolume)
            {
                ResetVolume = false;
                JsExecutor.ExecuteScript("arguments[0].volume = arguments[1];", video, Volume);
            }
        }
        private void SetPitchbendIfNeeded(IWebElement video)
        {
            var pitchbendState = Deck.GetPitchbendState();
            if (pitchbendState.IsDragging)
            {
                ResetPlaybackSpeed = true;

                int deltaY = pitchbendState.DeltaY;
                Debug.WriteLine($"DeltaY = {deltaY}");

                if (pitchbendState.DeltaY > 0)
                {
                    double speedChange = deltaY * 0.01; // adjust this constant to control the sensitivity of the control
                    double newSpeed = Math.Max(0.25, Math.Min(4.0, PlaybackSpeed + speedChange)); // limit the speed to a reasonable range
                    JsExecutor.ExecuteScript($"arguments[0].playbackRate = {newSpeed.ToString("F3").Replace(",", ".")};", video);
                }
                else
                {
                    double speedChange = deltaY * 0.005; // adjust this constant to control the sensitivity of the control
                    double newSpeed = Math.Max(0.25, Math.Min(4.0, PlaybackSpeed + speedChange)); // limit the speed to a reasonable range
                    JsExecutor.ExecuteScript($"arguments[0].playbackRate = {newSpeed.ToString("F3").Replace(",", ".")};", video);
                }
            }
            else if (ResetPlaybackSpeed)
            {
                JsExecutor.ExecuteScript($"arguments[0].playbackRate = {PlaybackSpeed.ToString("F3").Replace(",", ".")};", video);
                ResetPlaybackSpeed = false;
            }
        }
        private void SyncVideoInformation(IWebElement video)
        {
            var title = Driver?.Title ?? "";
            var state = (Dictionary<string, object>)JsExecutor
                .ExecuteScript(@"return { vuMeter: window.getVuMeter(), currentTime: arguments[0].currentTime, totalDuration: arguments[0].duration };", video);
            var vuMeter = Convert.ToDouble(state["vuMeter"]);
            var currentTime = Convert.ToDouble(state["currentTime"]);
            var totalDuration = Convert.ToDouble(state["totalDuration"]);

            MixerChannel.SetVuMeter(vuMeter);
            Deck.SetVideoInformation(title, currentTime, totalDuration);
        }

        public void Quit()
        {
            KillSwitch = true;
            Thread.Join();
        }

    }
}
