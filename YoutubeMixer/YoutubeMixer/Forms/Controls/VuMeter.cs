using YoutubeMixer.Library.Interfaces;

namespace YoutubeMixer.Forms.Controls
{
    public partial class VuMeter : Control
    {
        private const int NumLeds = 12;

        public VuMeter()
        {
            DoubleBuffered = true;
            Resize += VuMeter_Resize;
        }

        public void InitializeDraw()
        {
            if (PreviousValue != Value)
            {
                BeginInvoke(Invalidate);
                PreviousValue = Value;
            }
        }

        private void VuMeter_Resize(object? sender, EventArgs e)
        {
            PreviousValue = -1;
        }

        public IAudioSource? AudioSource { get; set; }

        private double MaxValue { get; set; } = 0.00000001;
        private double PreviousValue { get; set; }
        private double Value => AudioSource?.VuMeter != null ? AudioSource.VuMeter : 0;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.Clear(BackColor);

            Brush onBrush = new SolidBrush(Color.FromArgb(0, 255, 0));
            Brush offBrush = new SolidBrush(Color.FromArgb(100, 0, 0));

            int width = ClientRectangle.Width;
            int LedSize = Convert.ToInt32(Convert.ToDouble(ClientRectangle.Height) / NumLeds * 0.8);
            int LedSpacing = Convert.ToInt32(Convert.ToDouble(ClientRectangle.Height) / NumLeds * 0.2);

            double positiveValue = Math.Abs(Value);
            MaxValue = Math.Max(positiveValue, MaxValue);

            for (int i = 0; i < NumLeds; i++)
            {
                int x = 0;
                int y = (NumLeds - i - 1) * (LedSize + LedSpacing);

                if (positiveValue >= MaxValue * (i + 1) / NumLeds)
                {
                    g.FillRectangle(onBrush, x, y, width, LedSize);
                }
                else
                {
                    g.FillRectangle(offBrush, x, y, width, LedSize);
                }
            }

            onBrush.Dispose();
            offBrush.Dispose();
        }
    }
}
