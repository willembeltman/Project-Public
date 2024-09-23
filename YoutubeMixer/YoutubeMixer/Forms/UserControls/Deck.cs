using YoutubeMixer.Forms.Controls;
using YoutubeMixer.Library.Interfaces;
using YoutubeMixer.Library.Models;

namespace YoutubeMixer.UserControls
{
    public enum PitchRange
    {
        Range8,
        Range16,
        Range25,
        Range50,
        Range100,
    }

    public partial class Deck : UserControl, IPitchBendController, ITimelineProcessor
    {
        static int Precicion = 10;
        static int PrecicionDecimals = 1;
        private bool IsDragging { get; set; }
        private Point LastMousePosition { get; set; }
        private Point CurrentMousePosition { get; set; }

        public Deck()
        {
            InitializeComponent();
            trackBarPitch.Maximum = 100 * Precicion;
            trackBarPitch.Value = 100 * Precicion;
            PitchRange = PitchRange.Range16;
        }

        public void InitializeDraw()
        {
            DisplayControl.InitializeDraw();

            if (AudioSource == null)
            {
                buttonPlayPause.Enabled = false;
                return;
            }
            buttonPlayPause.Text = AudioSource.IsPlaying ? "Pause" : "Play";
            buttonPlayPause.Enabled = AudioSource.IsReady;
            buttonCue.Enabled = AudioSource.IsReady;
            buttonHotcue1.Enabled = AudioSource.IsReady;
            buttonHotcue2.Enabled = AudioSource.IsReady;
            buttonHotcue3.Enabled = AudioSource.IsReady;
            buttonHotcue4.Enabled = AudioSource.IsReady;
            buttonSetHotcue.Enabled = AudioSource.IsReady;
        }

        public IAudioSource? AudioSource { get { return DisplayControl.AudioSource; } set { DisplayControl.AudioSource = value; } }

        public PitchBendState GetPitchbendState()
        {
            if (IsDragging)
            {
                var curPos = CurrentMousePosition;
                var deltaY = (curPos.Y - LastMousePosition.Y) * -1;
                LastMousePosition = curPos;
                return new PitchBendState()
                {
                    IsDragging = true,
                    DeltaY = deltaY
                };
            }

            return new PitchBendState();
        }

        private PitchRange _PitchRange { get; set; }
        private PitchRange PitchRange
        {
            get { return _PitchRange; }
            set
            {
                switch (value)
                {
                    case PitchRange.Range8:
                        SetRange(92, 108);
                        break;
                    case PitchRange.Range16:
                        SetRange(84, 116);
                        break;
                    case PitchRange.Range25:
                        SetRange(75, 125);
                        break;
                    case PitchRange.Range50:
                        SetRange(50, 150);
                        break;
                    case PitchRange.Range100:
                        SetRange(0, 200);
                        break;
                }
                _PitchRange = value;
            }
        }


        public void ProcessTimelineData(double currentTime, double previousTime, TimeLineItem[] timelineItems)
        {
            DisplayControl.ProcessTimelineData(currentTime, previousTime, timelineItems);
        }

        private void PlaybackDisplay_MouseDown(object sender, MouseEventArgs e)
        {
            CurrentMousePosition = e.Location;
            LastMousePosition = e.Location;
            IsDragging = true;
        }
        private void PlaybackDisplay_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsDragging)
            {
                CurrentMousePosition = e.Location;
            }
        }
        private void PlaybackDisplay_MouseUp(object sender, MouseEventArgs e)
        {
            IsDragging = false;
        }

        private void trackBarPitch_ValueChanged(object sender, EventArgs e)
        {
            if (AudioSource == null) return;
            var playbackSpeed = Convert.ToDouble(trackBarPitch.Value) / Precicion / 100;
            var percentage = Convert.ToDouble(trackBarPitch.Value) / Precicion - 100;
            AudioSource.PlaybackSpeed = playbackSpeed;
            labelPitch.Text = $"{percentage.ToString("F" + PrecicionDecimals)}%";
            ResizeLabelPitch();
        }
        private void buttonPitchRange_Click(object sender, EventArgs e)
        {
            var setnext = false;
            var values = Enum.GetValues<PitchRange>();
            foreach (var value in values)
            {
                if (setnext)
                {
                    PitchRange = value;
                    return;
                }
                if (PitchRange == value)
                {
                    setnext = true;
                }
            }
            PitchRange = values.First();
        }

        private void SetRange(int min, int max)
        {
            var min2 = min * Precicion;
            var max2 = max * Precicion;

            if (trackBarPitch.Value < min2)
                trackBarPitch.Value = min2;
            trackBarPitch.Minimum = min2;
            if (trackBarPitch.Value > max2)
                trackBarPitch.Value = max2;
            trackBarPitch.Maximum = max2;

            buttonPitchRange.Text = (max - 100).ToString() + "%";
        }

        private void buttonPlayPause_Click(object sender, EventArgs e)
        {
            if (AudioSource == null) return;
            AudioSource.PlayPause();
        }
        private void buttonCue_Click(object sender, EventArgs e)
        {
        }

        private void buttonSetHotcue_Click(object sender, EventArgs e)
        {

        }
        private void buttonHotcue1_Click(object sender, EventArgs e)
        {

        }
        private void buttonHotcue2_Click(object sender, EventArgs e)
        {

        }
        private void buttonHotcue3_Click(object sender, EventArgs e)
        {

        }
        private void buttonHotcue4_Click(object sender, EventArgs e)
        {

        }

        private void buttonPitchControl_Click(object sender, EventArgs e)
        {

        }

        private void Deck_Load(object sender, EventArgs e)
        {
            Deck_Resize(sender, e);
        }

        private void Deck_Resize(object sender, EventArgs e)
        {
            var margin = 5;

            buttonHotcue1.Left = 0;
            buttonHotcue1.Top = 0;

            buttonHotcue2.Left = 0;
            buttonHotcue2.Top = buttonHotcue1.Bottom + margin;

            buttonHotcue3.Left = 0;
            buttonHotcue3.Top = buttonHotcue2.Bottom + margin;

            buttonHotcue4.Left = 0;
            buttonHotcue4.Top = buttonHotcue3.Bottom + margin;

            buttonSetHotcue.Left = 0;
            buttonSetHotcue.Top = buttonHotcue4.Bottom + margin;

            buttonPlayPause.Left = 0;
            buttonPlayPause.Top = ClientRectangle.Height - buttonPlayPause.Height;

            buttonCue.Left = 0;
            buttonCue.Top = buttonPlayPause.Top - buttonCue.Height - margin;

            buttonPitchRange.Top = 0;
            buttonPitchRange.Left = ClientRectangle.Width - buttonPitchRange.Width;

            buttonPitchControl.Top = buttonPitchRange.Bottom + margin;
            buttonPitchControl.Left = ClientRectangle.Width - buttonPitchControl.Width;
            ResizeLabelPitch();

            var offset = (buttonPitchControl.Width - trackBarPitch.Width) / 2;

            trackBarPitch.Top = buttonPitchControl.Bottom;
            trackBarPitch.Left = ClientRectangle.Width - trackBarPitch.Width - offset;
            trackBarPitch.Height = ClientRectangle.Height - labelPitch.Height - buttonPitchControl.Bottom;

            DisplayControl.Left = buttonHotcue1.Right;
            DisplayControl.Top = 0;
            DisplayControl.Width = ClientRectangle.Width - buttonHotcue1.Width - buttonPitchRange.Width;
            DisplayControl.Height = ClientRectangle.Height;
        }

        private void ResizeLabelPitch()
        {
            var offset2 = (buttonPitchControl.Width - labelPitch.Width) / 2;

            labelPitch.Top = ClientRectangle.Height - labelPitch.Height;
            labelPitch.Left = ClientRectangle.Width - labelPitch.Width - offset2;
        }

    }
}
