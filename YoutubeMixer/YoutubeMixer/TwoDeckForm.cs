using YoutubeMixer.AudioSources;

namespace YoutubeMixer.Forms
{
    public partial class TwoDeckForm : Form
    {
        public YoutubeAudioSource LeftAudioSource { get; }
        //public YoutubeAudioSource RightAudioSource { get; }

        public TwoDeckForm()
        {
            InitializeComponent();

            LeftAudioSource = new YoutubeAudioSource(Mixer.LeftMixerChannel, DeckLeft, Mixer.LeftMixerChannel);
            Mixer.LeftMixerChannel.AudioSource = LeftAudioSource;
            DeckLeft.AudioSource = LeftAudioSource;

            //RightAudioSource = new YoutubeAudioSource(Mixer.RightMixerChannel, DeckRight, Mixer.RightMixerChannel);
            //Mixer.RightMixerChannel.AudioSource = RightAudioSource;
            //DeckRight.AudioSource = RightAudioSource;
        }

        private void DeckForm_Load(object sender, EventArgs e)
        {
            // Set the width of the form to the screen width
            this.Width = Screen.PrimaryScreen!.WorkingArea.Width;

            // Reposition the form to the bottom of the screen
            int screenHeight = Screen.PrimaryScreen.WorkingArea.Height;
            int formHeight = this.Height;
            int formX = (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2;
            int formY = screenHeight - formHeight;
            this.Location = new Point(formX, formY);

            TwoDeckForm_Resize(sender, e);
        }
        private void TwoDeckForm_Resize(object sender, EventArgs e)
        {
            var deck_width = (ClientRectangle.Width - Mixer.Width) / 2;

            DeckLeft.Top = 0;
            DeckLeft.Left = 0;
            DeckLeft.Width = deck_width;
            DeckLeft.Height = ClientRectangle.Height;

            Mixer.Left = DeckLeft.Right;
            Mixer.Top = 0;
            Mixer.Height = ClientRectangle.Height;

            DeckRight.Top = 0;
            DeckRight.Left = Mixer.Right;
            DeckRight.Width = deck_width;
            DeckRight.Height = ClientRectangle.Height;
        }
        private void TwoDeckForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            LeftAudioSource.Dispose();
            //RightAudioSource.Dispose();
        }
    }
}