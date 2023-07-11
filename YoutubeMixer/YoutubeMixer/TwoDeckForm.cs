namespace YoutubeMixer.Forms
{
    public partial class TwoDeckForm : Form
    {
        public TwoDeckForm()
        {
            InitializeComponent();

            LeftYoutubeController = new YoutubeController(DeckLeft, Mixer.LeftMixerChannel);
            //RightYoutubeController = new YoutubeController(DeckRight, Mixer.RightMixerChannel);
        }

        public YoutubeController LeftYoutubeController { get; }
        //public YoutubeController RightYoutubeController { get; }

        private void DeckForm_Load(object sender, EventArgs e)
        {
            // Set the width of the form to the screen width
            this.Width = Screen.PrimaryScreen.WorkingArea.Width;

            // Reposition the form to the bottom of the screen
            int screenHeight = Screen.PrimaryScreen.WorkingArea.Height;
            int formHeight = this.Height;
            int formX = (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2;
            int formY = screenHeight - formHeight;
            this.Location = new Point(formX, formY);

            // Start the youtube controllers
            LeftYoutubeController.Start();
            //RightYoutubeController.Start();
        }
        private void TwoDeckForm_Resize(object sender, EventArgs e)
        {
            var padding = 8;

            var deck_width = (ClientRectangle.Width - Mixer.Width - padding * 4) / 2;
            var deck_height = ClientRectangle.Height - padding * 2;
            var deckright_left = padding + deck_width + padding + Mixer.Width + padding; 
            var mixer_left = deck_width + padding * 2;
            var mixer_top = (ClientRectangle.Height - Mixer.Height - padding * 2) / 2;

            DeckLeft.Top = padding;
            DeckLeft.Left = padding;
            DeckLeft.Width = deck_width;
            DeckLeft.Height = deck_height;

            Mixer.Left = mixer_left;
            Mixer.Top = mixer_top;

            DeckRight.Top = padding;
            DeckRight.Left = deckright_left;
            DeckRight.Width = deck_width;
            DeckRight.Height = deck_height;
        }
        private void TwoDeckForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            LeftYoutubeController.Quit();
            //RightYoutubeController.Dispose();
        }
    }
}