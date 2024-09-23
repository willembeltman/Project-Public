using YoutubeMixer.Library.Interfaces;
using YoutubeMixer.Library.Models;
using YoutubeMixer.Properties;

namespace YoutubeMixer.Forms.Controls
{
    public class DisplayControl : Control, ITimelineProcessor
    {
        public DisplayControl()
        {
            DoubleBuffered = true;
            Resize += DisplayControl_Resize;
        }

        public void InitializeDraw()
        {
            if (PreviousCurrentTime != CurrentTime ||
                PreviousTitle != Title ||
                PreviousTotalDuration != TotalDuration ||
                PreviousKickDetected != KickDetected)
            {
                BeginInvoke(Invalidate);
                PreviousCurrentTime = CurrentTime;
                PreviousTitle = Title;
                PreviousTotalDuration = TotalDuration;
                PreviousKickDetected = KickDetected;
            }
        }

        private void DisplayControl_Resize(object? sender, EventArgs e)
        {
            PreviousCurrentTime = TimeSpan.Zero;
            PreviousTitle = "null";
            PreviousTotalDuration = TimeSpan.Zero;
        }

        public IAudioSource? AudioSource { get; set; }

        private string? PreviousTitle { get; set; }
        private TimeSpan PreviousCurrentTime { get; set; }
        private TimeSpan PreviousTotalDuration { get; set; }
        private bool PreviousKickDetected { get; set; }

        private string? Title => AudioSource?.Title;
        private TimeSpan CurrentTime => AudioSource?.CurrentTime != null ? TimeSpan.FromSeconds(AudioSource.CurrentTime) : TimeSpan.FromSeconds(29);
        private TimeSpan TotalDuration => AudioSource?.TotalDuration != null ? TimeSpan.FromSeconds(AudioSource.TotalDuration) : TimeSpan.FromSeconds(60);
        private bool KickDetected => AudioSource?.KickDetected ?? false;

        public void ProcessTimelineData(double currentTime, double previousTime, TimeLineItem[] timelineItems)
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;

            var timeLeft = CurrentTime - TotalDuration;

            // Calculate the progress percentage
            double progress = CurrentTime.TotalSeconds / TotalDuration.TotalSeconds;
            if (double.IsNaN(progress)) progress = 0;

            // Calculate the rotation angle based on the current time position of the playback
            double revolutionsPerSecond = 45d / 60.0;
            double revolutionsPerDuration = revolutionsPerSecond * TotalDuration.TotalSeconds;
            double rotationAngle = progress * revolutionsPerDuration * 360.0;

            // Clear screen            
            g.Clear(Color.White);

            Image RecordImage = Resources.Record;
            // Draw the record
            if (RecordImage != null)
            {
                var recordSize = ClientRectangle.Height;
                if (recordSize > ClientRectangle.Width) recordSize = ClientRectangle.Width;
                int recordWidth = recordSize;
                int recordHeight = recordSize;
                int x = (ClientRectangle.Width - recordWidth) / 2;
                int y = (ClientRectangle.Height - recordHeight) / 2;

                // Create a new bitmap with the same dimensions as the record image
                Bitmap rotatedRecordImage = new Bitmap(recordWidth, recordHeight);
                using (Graphics rotatedRecordGraphics = Graphics.FromImage(rotatedRecordImage))
                {
                    // Rotate the record image
                    rotatedRecordGraphics.TranslateTransform(recordWidth / 2, recordHeight / 2);
                    rotatedRecordGraphics.RotateTransform(Convert.ToSingle(rotationAngle));
                    rotatedRecordGraphics.DrawImage(RecordImage, -recordWidth / 2, -recordHeight / 2, recordWidth, recordHeight);
                    rotatedRecordGraphics.ResetTransform();
                }

                // Draw the rotated record image onto the graphic instance
                g.DrawImage(rotatedRecordImage, x, y, recordWidth, recordHeight);
            }

            using (Font font = new Font("Arial", 8))
            using (Brush brush = new SolidBrush(Color.Black))
            using (Brush redbrush = new SolidBrush(Color.Red))
            using (Pen pen = new Pen(Color.Black))
            {
                // Draw the current time
                g.DrawString(CurrentTime.ToString("h':'mm':'ss'.'ff"), font, brush, 5, 5);

                if (KickDetected)
                {
                    g.FillRectangle(redbrush, 0, ClientRectangle.Height - 16, ClientRectangle.Width, 16);
                }

                // Draw the progress bar
                g.FillRectangle(brush, 115, 7, Convert.ToInt32(Convert.ToDouble(ClientRectangle.Width - 222) * progress), 18);
                g.DrawRectangle(pen, 110, 5, Convert.ToInt32(Convert.ToDouble(ClientRectangle.Width - 222) + 4), 21);

                // Draw the time left
                g.DrawString(timeLeft.ToString("h':'mm':'ss'.'ff"), font, brush, ClientRectangle.Width - 105, 5);

                // Draw the current time
                g.DrawString(Title, font, brush, 5, 30);
            }
        }

    }
}
