using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeMixer.Controls
{
    public partial class VuMeter : Control
    {
        private const int NumLeds = 12;
        private double MaxValue = 0.00000001;

        private double _value = 0;

        public double Value
        {
            get { return _value; }
            set
            {
                _value = value;
            }
        }

        public void UpdateDisplay()
        {
            BeginInvoke(() => {
                Invalidate();
            });

        }

        public VuMeter()
        {
            DoubleBuffered = true;
        }

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

            double positiveValue = Math.Abs(_value);
            MaxValue = Math.Max(positiveValue, MaxValue);

            for (int i = 0; i < NumLeds; i++)
            {
                int x = 0;
                int y = ((NumLeds - i - 1) * (LedSize + LedSpacing));

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
