using YoutubeMixer.Models;

namespace YoutubeMixer.UserControls
{
    public partial class Deck : UserControl
    {
        private bool IsDragging { get; set; }
        private Point LastMousePosition { get; set; }
        private Point CurrentMousePosition { get; set; }

        public Deck()
        {
            InitializeComponent();
        }

        public YoutubeController? Controller { get; set; }
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
        public void SetVideoInformation(string title, double currentTime, double totalDuration)
        {
            DisplayControl.Title = title;
            DisplayControl.CurrentTime = TimeSpan.FromSeconds(currentTime);
            DisplayControl.TotalTime = TimeSpan.FromSeconds(totalDuration);
            DisplayControl.UpdateDisplay();
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
    }
}
