using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        public long Fps { get; set; } = 10;
        public Bitmap Output { get; private set; }
        public Stopwatch Stopwatch { get; private set; }
        public long FrameIndex { get; set; }
        public int Width { get; set; } = 1280;
        public int Height { get; set; } = 720;
        public int GridSize { get; set; } = 32;
        public Point Direction { get; set; }
        public List<Point> Body { get; set; } = new List<Point>();
        public Random Rnd { get; private set; }
        public Point Food { get; set; }
        public int GridWidth { get; private set; }
        public int GridHeight { get; private set; }
        public bool GameOver { get; private set; }

        public Form1()
        {
            InitializeComponent();

            KeyDown += Form1_KeyDown;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    Direction = new Point(0, -1);
                    break;
                case Keys.Down:
                    Direction = new Point(0, 1);
                    break;
                case Keys.Left:
                    Direction = new Point(-1, 0);
                    break;
                case Keys.Right:
                    Direction = new Point(1, 0);
                    break;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GridWidth = Width / GridSize;
            GridHeight = Height / GridSize;
            Body.Add(new Point(GridWidth / 2, GridHeight / 2));

            Rnd = new Random();
            PlaceFood();

            Output = new Bitmap(1280, 720);
            Stopwatch = Stopwatch.StartNew();
            Direction = new Point(1, 0);
            Application.Idle += Application_Idle;
        }

        private void PlaceFood()
        {
            do
            {
                Food = new Point(Rnd.Next(GridWidth), Rnd.Next(GridHeight));
            }
            while (Body.Any(a => a == Food));
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            var next = FrameIndex * 1000 / Fps;
            int wait = Convert.ToInt32(next - Stopwatch.ElapsedMilliseconds);
            if (wait <= 0)
            {
                //Thread.Sleep(wait);

                Render();
                pictureBox1.Image = Output;

                FrameIndex++;
            }
        }

        private void Render()
        {
            using (Graphics g = Graphics.FromImage(Output))
            {
                g.Clear(Color.Black); // Achtergrondkleur van de bitmap instellen

                if (GameOver)
                {
                    Font DefaultFont = new Font("Arial", 30);
                    string gameOverText = "GAME OVER";
                    SizeF textSize = g.MeasureString(gameOverText, DefaultFont);

                    // Bereken de positie van de tekst om deze in het midden van het scherm te plaatsen
                    float textX = (Width - textSize.Width) / 2;
                    float textY = (Height - textSize.Height) / 2;

                    g.DrawString(gameOverText, DefaultFont, Brushes.White, textX, textY);
                }
                else
                {
                    var head = Body[0];
                    var newloc = new Point(head.X + Direction.X, head.Y + Direction.Y);

                    if (newloc.X < 0 || newloc.X > GridWidth ||
                        newloc.Y < 0 || newloc.Y > GridHeight)
                    {
                        GameOver = true;
                        return;
                    }

                    Body.Insert(0, newloc);

                    if (newloc == Food)
                        PlaceFood();
                    else
                        Body.RemoveAt(Body.Count - 1);

                    foreach (var point in Body)
                        g.FillRectangle(Brushes.White, new Rectangle(point.X * GridSize, point.Y * GridSize, GridSize, GridSize));

                    g.FillRectangle(Brushes.White, new Rectangle(Food.X * GridSize, Food.Y * GridSize, GridSize, GridSize));
                }
            }
        }
    }
}
