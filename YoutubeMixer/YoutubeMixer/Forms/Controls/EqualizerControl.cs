namespace YoutubeMixer.Forms.Controls
{
    public class EqualizerControl : Control
    {
        public EqualizerControl()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
            BackColor = Color.White;
        }

        public event EventHandler? ValueChanged;

        private double _value = 0;
        private double _dragStartValue = 0;
        private Point _dragStartPos = Point.Empty;
        private bool _isDragging = false;

        public double Range { get; set; } = 24;

        public double Value
        {
            get { return _value; }
            set
            {
                value = Math.Min(Math.Max(value, Range * -1), Range);
                if (value != _value)
                {
                    _value = value;
                    Invalidate();
                    ValueChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        int CalculateAngle(double val)
        {
            return Convert.ToInt32(270 + val * 180 / 24);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            Rectangle bounds = ClientRectangle;

            // Calculate the center of the knob
            int centerX = bounds.Left + bounds.Width / 2;
            int centerY = bounds.Top + bounds.Height / 2;
            Point center = new Point(centerX, centerY);

            // Calculate the radius of the knob
            int radius = Convert.ToInt32(Math.Min(bounds.Width, bounds.Height) / 3);
            int vijf = radius / 5;
            int twee = vijf / 2 + 1;

            g.Clear(SystemColors.Control);

            // Draw the shadow
            Rectangle shadowBounds = new Rectangle(centerX - radius + vijf, centerY - radius + vijf, radius * 2, radius * 2);
            g.FillEllipse(new SolidBrush(Color.FromArgb(50, Color.Black)), shadowBounds);

            // Draw the outer circle
            g.DrawEllipse(new Pen(Color.Black), centerX - radius, centerY - radius, radius * 2, radius * 2);

            // Draw the inner circle
            g.FillEllipse(Brushes.White, centerX - radius + twee, centerY - radius + twee, (radius - twee) * 2, (radius - twee) * 2);

            // Draw the indicator line
            int angle = CalculateAngle(_value);
            int indicatorLength = radius * 9 / 10;
            Point indicatorEnd = new Point(
                centerX + (int)(indicatorLength * Math.Cos(angle * Math.PI / 180)),
                centerY + (int)(indicatorLength * Math.Sin(angle * Math.PI / 180))
            );
            g.DrawLine(new Pen(Color.Black), center, indicatorEnd);

            // Draw the numbers outside the knob
            int numberRadius = Convert.ToInt32(Convert.ToDouble(radius) * 1.4);
            float fontsize = Convert.ToSingle(radius / 5);
            Font font = new Font("Arial", fontsize, FontStyle.Bold);
            Brush brush = new SolidBrush(Color.Black);

            var start = Range * -1 * 3 / 4;
            var min = Range * 3 / 4;
            var plus = Range / 4;

            for (double i = start; i <= min; i += plus)
            {
                int numberAngle = CalculateAngle(i);
                Point numberPoint = new Point(
                    centerX + (int)(numberRadius * Math.Cos(numberAngle * Math.PI / 180)),
                    centerY + (int)(numberRadius * Math.Sin(numberAngle * Math.PI / 180))
                );
                string numberString = i.ToString();
                SizeF stringSize = g.MeasureString(numberString, font);
                Point stringPoint = new Point(
                    (int)(numberPoint.X - stringSize.Width / 2),
                    (int)(numberPoint.Y - stringSize.Height / 2)
                );
                g.DrawString(numberString, font, brush, stringPoint);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            _dragStartPos = e.Location;
            _dragStartValue = Value;
            _isDragging = true;
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (_isDragging)
            {
                double delta = _dragStartPos.Y - e.Y;
                double valueDelta = delta / 8; // adjust this value to change sensitivity
                Value = Math.Min(Math.Max(_dragStartValue + valueDelta, Range * -1), Range);
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _isDragging = false;
        }
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            Value = 0;
        }
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            int valueDelta = e.Delta / SystemInformation.MouseWheelScrollDelta; // adjust this value to change sensitivity
            Value = Math.Min(Math.Max(_value + valueDelta, 0), 100);
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            switch (e.KeyCode)
            {
                case Keys.Up:
                    Value = Math.Min(_value + 1, 100);
                    break;
                case Keys.Down:
                    Value = Math.Max(_value - 1, 0);
                    break;
            }
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // Force the control to be square
            int size = Math.Min(Width, Height);
            Width = size;
            Height = size;
        }
    }
}
