using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Snake
{
    public partial class SnakeForm : Form
    {
        private const int SnakeSize = 16;
        private const int BoardSize = 25;
        private const int TimerInterval = 200;

        private Timer gameTimer;
        private Snake snake;
        private Direction currentDirection;
        private Direction nextDirection;
        private List<Point> foodList;
        private Random random;

        public SnakeForm()
        {
            //InitializeComponent();
            InitializeGame();

            this.Paint += SnakeForm_Paint;
            this.KeyDown += SnakeForm_KeyDown;
        }

        private void InitializeGame()
        {
            gameTimer = new Timer();
            gameTimer.Interval = TimerInterval;
            gameTimer.Tick += GameTimer_Tick;

            snake = new Snake();
            currentDirection = Direction.Right;
            nextDirection = Direction.Right;
            foodList = new List<Point>();
            random = new Random();

            gameTimer.Start();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (!snake.Move(currentDirection, BoardSize))
            {
                gameTimer.Stop();
                MessageBox.Show("Game Over!");
                InitializeGame();
                return;
            }

            if (snake.HeadPosition == GetFoodPosition())
            {
                snake.Grow();
                GenerateFood();
            }

            nextDirection = GetNextDirection();

            Refresh();
        }

        private void SnakeForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (currentDirection != Direction.Down)
                        nextDirection = Direction.Up;
                    break;
                case Keys.Down:
                    if (currentDirection != Direction.Up)
                        nextDirection = Direction.Down;
                    break;
                case Keys.Left:
                    if (currentDirection != Direction.Right)
                        nextDirection = Direction.Left;
                    break;
                case Keys.Right:
                    if (currentDirection != Direction.Left)
                        nextDirection = Direction.Right;
                    break;
            }
        }

        private void SnakeForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            snake.Draw(canvas, SnakeSize);

            foreach (Point foodPosition in foodList)
            {
                canvas.FillRectangle(Brushes.Red, foodPosition.X * SnakeSize, foodPosition.Y * SnakeSize, SnakeSize, SnakeSize);
            }
        }

        private void GenerateFood()
        {
            int x = random.Next(BoardSize);
            int y = random.Next(BoardSize);

            Point foodPosition = new Point(x, y);
            foodList.Add(foodPosition);
        }

        private Point GetFoodPosition()
        {
            foreach (Point foodPosition in foodList)
            {
                if (snake.HeadPosition == foodPosition)
                {
                    foodList.Remove(foodPosition);
                    return foodPosition;
                }
            }

            return Point.Empty;
        }

        private Direction GetNextDirection()
        {
            if (nextDirection == Direction.Left && currentDirection != Direction.Right)
                return Direction.Left;
            if (nextDirection == Direction.Right && currentDirection != Direction.Left)
                return Direction.Right;
            if (nextDirection == Direction.Up && currentDirection != Direction.Down)
                return Direction.Up;
            if (nextDirection == Direction.Down && currentDirection != Direction.Up)
                return Direction.Down;

            return currentDirection;
        }
    }

    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    public class Snake
    {
        private List<Point> body;
        private Direction direction;

        public Snake()
        {
            body = new List<Point>
            {
                new Point(0, 0)
            };

            direction = Direction.Right;
        }

        public Point HeadPosition => body[0];

        public bool Move(Direction direction, int boardSize)
        {
            Point newHead = CalculateNewHead(direction);

            if (IsOutOfBounds(newHead, boardSize) || IsSnakeBody(newHead))
                return false;

            body.Insert(0, newHead);
            body.RemoveAt(body.Count - 1);

            this.direction = direction;
            return true;
        }

        public void Grow()
        {
            Point newHead = CalculateNewHead(direction);
            body.Insert(0, newHead);
        }

        private Point CalculateNewHead(Direction direction)
        {
            Point head = body[0];

            switch (direction)
            {
                case Direction.Left:
                    return new Point(head.X - 1, head.Y);
                case Direction.Right:
                    return new Point(head.X + 1, head.Y);
                case Direction.Up:
                    return new Point(head.X, head.Y - 1);
                case Direction.Down:
                    return new Point(head.X, head.Y + 1);
                default:
                    throw new ArgumentException("Invalid direction");
            }
        }

        private bool IsOutOfBounds(Point position, int boardSize)
        {
            return position.X < 0 || position.X >= boardSize ||
                   position.Y < 0 || position.Y >= boardSize;
        }

        private bool IsSnakeBody(Point position)
        {
            return body.Contains(position);
        }

        public void Draw(Graphics canvas, int snakeSize)
        {
            foreach (Point position in body)
            {
                canvas.FillRectangle(Brushes.Green, position.X * snakeSize, position.Y * snakeSize, snakeSize, snakeSize);
            }
        }
    }
}
