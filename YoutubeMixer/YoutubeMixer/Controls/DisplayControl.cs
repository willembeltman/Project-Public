using OpenQA.Selenium.DevTools.V117.Runtime;
using YoutubeMixer.Properties;

namespace YoutubeMixer.Controls
{
    public class DisplayControl : Control
    {
        public DisplayControl()
        {
            DoubleBuffered = true;
        }
        public string? Title { get; set; }
        public TimeSpan CurrentTime { get; set; } = TimeSpan.FromSeconds(29);
        public TimeSpan TotalTime { get; set; } = TimeSpan.FromSeconds(60);

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;

            var timeLeft = CurrentTime - TotalTime;

            // Calculate the progress percentage
            double progress = CurrentTime.TotalSeconds / TotalTime.TotalSeconds;
            if (double.IsNaN(progress)) progress = 0;

            // Calculate the rotation angle based on the current time position of the playback
            double revolutionsPerSecond = 45d / 60.0;
            double revolutionsPerDuration = revolutionsPerSecond * TotalTime.TotalSeconds;
            double rotationAngle = (progress * revolutionsPerDuration) * 360.0;

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
            using (Pen pen = new Pen(Color.Black))
            {
                // Draw the current time
                g.DrawString(CurrentTime.ToString("h':'mm':'ss'.'ff"), font, brush, 5, 5);

                // Draw the progress bar
                g.FillRectangle(brush, 115, 7, Convert.ToInt32(Convert.ToDouble(ClientRectangle.Width - 222) * progress), 18);
                g.DrawRectangle(pen, 110, 5, Convert.ToInt32(Convert.ToDouble(ClientRectangle.Width - 222) + 4), 21);

                // Draw the time left
                g.DrawString(timeLeft.ToString("h':'mm':'ss'.'ff"), font, brush, ClientRectangle.Width - 105, 5);

                // Draw the current time
                g.DrawString(Title, font, brush, 5, 30);
            }
        }

        public void UpdateDisplay()
        {
            BeginInvoke(Invalidate);
        }
    }
}
