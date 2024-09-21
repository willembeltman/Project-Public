using YoutubeMixer.AudioSources;
using YoutubeMixer.Interfaces;
using YoutubeMixer.Models;

namespace YoutubeMixer.UserControls
{
    public partial class Deck : UserControl, IPitchBendController
    {
        private bool IsDragging { get; set; }
        private Point LastMousePosition { get; set; }
        private Point CurrentMousePosition { get; set; }

        public Deck()
        {
            InitializeComponent();
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

        private void trackBarPitch_Scroll(object sender, EventArgs e)
        {

        }

        private void buttonPlayPause_Click(object sender, EventArgs e)
        {
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

        private void buttonPitchRange_Click(object sender, EventArgs e)
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

            var offset2 = (buttonPitchControl.Width - labelPitch.Width) / 2;

            labelPitch.Top = ClientRectangle.Height - labelPitch.Height;
            labelPitch.Left = ClientRectangle.Width - labelPitch.Width - offset2;

            var offset = (buttonPitchControl.Width - trackBarPitch.Width) / 2;

            trackBarPitch.Top = buttonPitchControl.Bottom;
            trackBarPitch.Left = ClientRectangle.Width - trackBarPitch.Width - offset;
            trackBarPitch.Height = ClientRectangle.Height - labelPitch.Height - buttonPitchControl.Bottom;

            DisplayControl.Left = buttonHotcue1.Right;
            DisplayControl.Top = 0;
            DisplayControl.Width = ClientRectangle.Width - buttonHotcue1.Width - buttonPitchRange.Width;
            DisplayControl.Height = ClientRectangle.Height;
        }
    }
}
