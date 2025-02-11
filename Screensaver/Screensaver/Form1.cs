using System.Diagnostics;

namespace Screensaver;

public partial class Form1 : Form, IParent
{
    int ParticalCount = 12;

    public Form1()
    {
        InitializeComponent();

        Random = new Random();
        List = new Partical[ParticalCount];
        Stopwatch = Stopwatch.StartNew();
        for (int i = 0; i < ParticalCount; i++)
        {
            var part = new Partical(this);
            List[i] = part;
        }
        foreach (var part in List)
        {
            part.FindPosition();
        }
    }

    public Random Random { get; }
    public Partical[] List { get; }
    public Stopwatch Stopwatch { get; }

    private void Form1_Paint(object sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        g.Clear(Color.Black);

        foreach (var part in List)
        {
            part.Calculate(Stopwatch);
            part.DrawParticle(g);
        }
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
        Invalidate();
    }
}

public interface IParent
{
    Random Random { get; }
    Partical[] List { get; }
    Stopwatch Stopwatch { get; }
    Rectangle ClientRectangle { get; }
}
public class Partical
{
    public Partical(IParent parent)
    {
        Parent = parent;
        Size = Random.Next(Math.Min(ClientRectangle.Width, ClientRectangle.Height) / 4);
        SpeedX = Random.Next(ClientRectangle.Width) - ClientRectangle.Width / 2;
        SpeedY = Random.Next(ClientRectangle.Height) - ClientRectangle.Height / 2;
    }

    public IParent Parent { get; }
    public Rectangle ClientRectangle => Parent.ClientRectangle;
    public Random Random => Parent.Random;

    public double LastElapsed { get; set; }
    public int Size { get; set; }
    public double SpeedX { get; set; }
    public double SpeedY { get; set; }
    public int X { get; set; }
    public int Y { get; set; }

    public void FindPosition()
    {
        while (Parent.List.Any(a => a != this && a.IsOverlappingWith(this)))
        {
            X = Random.Next(ClientRectangle.Width - Size);
            Y = Random.Next(ClientRectangle.Height - Size);
        }
    }

    private bool IsOverlappingWith(Partical otherParticle)
    {
        var deltaX = otherParticle.X - X;
        var deltaY = otherParticle.Y - Y;
        var distance = Convert.ToInt32(Math.Sqrt(deltaX * deltaX + deltaY * deltaY));
        var minDistance = (otherParticle.Size + Size) / 2;
        var overlap = distance - minDistance;
        return distance < minDistance;
    }

    private void BounceWith2(Partical otherParticle, Point originalPositionOfOtherParticle, double elapsedSinceOrignalPosition)
    {
        // Bereken de vector tussen beide deeltjes
        double deltaX = otherParticle.X - X;
        double deltaY = otherParticle.Y - Y;
        double distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        double minDistance = (otherParticle.Size + Size) / 2;
        double overlap = minDistance - distance;

        // Bereken punt waar de botsing is, er vanuit gaande dat 'this' particle stil staat

        // Bereken hoe ver 'originalPositionOfOtherParticle' daar vandaan is

        // Bereken hoeveel tijd er voor nodig was om daar te komen met de huidige SpeedX en SpeedY

        // Bereken hoeveel tijd er dan nog over is ten opzichte van elapsed
        var elapsedRemaining = 0;

        // Stel de positie van otherParticle in op positie van botsing (dus daar waar ze elkaar nog niet raken)

        // Massa berekenen op basis van grootte (oppervlakte als proxy voor massa)
        double mass1 = (Size * Size) / 4.0;
        double mass2 = (otherParticle.Size * otherParticle.Size) / 4.0;

        // Normale vector
        double normalX = deltaX / distance;
        double normalY = deltaY / distance;

        // Tangentiële vector (loodrecht op de normale)
        double tangentX = -normalY;
        double tangentY = normalX;

        // Projecteer snelheden op normale en tangentiële vector
        double speedNormal1 = SpeedX * normalX + SpeedY * normalY;
        double speedNormal2 = otherParticle.SpeedX * normalX + otherParticle.SpeedY * normalY;

        double speedTangent1 = SpeedX * tangentX + SpeedY * tangentY;
        double speedTangent2 = otherParticle.SpeedX * tangentX + otherParticle.SpeedY * tangentY;

        // **Elastic collision formule met massa**
        double newSpeedNormal1 = (speedNormal1 * (mass1 - mass2) + 2 * mass2 * speedNormal2) / (mass1 + mass2);
        double newSpeedNormal2 = (speedNormal2 * (mass2 - mass1) + 2 * mass1 * speedNormal1) / (mass1 + mass2);

        // Zet de snelheden terug om naar x en y
        SpeedX = newSpeedNormal1 * normalX + speedTangent1 * tangentX;
        SpeedY = newSpeedNormal1 * normalY + speedTangent1 * tangentY;

        otherParticle.SpeedX = newSpeedNormal2 * normalX + speedTangent2 * tangentX;
        otherParticle.SpeedY = newSpeedNormal2 * normalY + speedTangent2 * tangentY;

        // Bereken nieuwe positie van otherParticle op basis van nieuwe speedx/y aan de hand van elapsedRemaining

    }
    private void BounceWith(Partical otherParticle, Point originalPositionOfOtherParticle, double elapsedSinceOriginalPosition)
    {
        // Bereken de vector tussen beide deeltjes
        double deltaX = otherParticle.X - X;
        double deltaY = otherParticle.Y - Y;
        double distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        double minDistance = (otherParticle.Size + Size) / 2.0;
        double overlap = minDistance - distance;

        if (distance == 0) return; // Voorkom deling door nul

        // **Stap 1: Botsingspunt berekenen**
        // Verplaats otherParticle achteruit tot het moment van botsing
        double travelX = otherParticle.X - originalPositionOfOtherParticle.X;
        double travelY = otherParticle.Y - originalPositionOfOtherParticle.Y;
        double travelDistance = Math.Sqrt(travelX * travelX + travelY * travelY);

        // Hoe ver is de botsing vanaf de oorspronkelijke positie?
        double travelFraction = (travelDistance - overlap) / travelDistance;
        if (travelFraction < 0) travelFraction = 0; // Voorkom negatieve waarden

        // Bepaal botsingspositie
        double collisionX = originalPositionOfOtherParticle.X + travelX * travelFraction;
        double collisionY = originalPositionOfOtherParticle.Y + travelY * travelFraction;

        // **Stap 2: Tijd tot botsing berekenen**
        double speedMagnitude = Math.Sqrt(otherParticle.SpeedX * otherParticle.SpeedX + otherParticle.SpeedY * otherParticle.SpeedY);
        double timeToCollision = (travelFraction * elapsedSinceOriginalPosition);

        // Resterende tijd na botsing
        double elapsedRemaining = elapsedSinceOriginalPosition - timeToCollision;

        // **Stap 3: Zet otherParticle terug op het botsingspunt**
        otherParticle.X = (int)collisionX;
        otherParticle.Y = (int)collisionY;

        // **Stap 4: Bereken massa op basis van grootte**
        double mass1 = (Size * Size) / 4.0;
        double mass2 = (otherParticle.Size * otherParticle.Size) / 4.0;

        // **Stap 5: Normale en tangentiële vectoren**
        double normalX = deltaX / distance;
        double normalY = deltaY / distance;

        double tangentX = -normalY;
        double tangentY = normalX;

        // **Stap 6: Projecteer snelheden**
        double speedNormal1 = SpeedX * normalX + SpeedY * normalY;
        double speedNormal2 = otherParticle.SpeedX * normalX + otherParticle.SpeedY * normalY;

        double speedTangent1 = SpeedX * tangentX + SpeedY * tangentY;
        double speedTangent2 = otherParticle.SpeedX * tangentX + otherParticle.SpeedY * tangentY;

        // **Stap 7: Elastic collision formule met massa**
        double newSpeedNormal1 = (speedNormal1 * (mass1 - mass2) + 2 * mass2 * speedNormal2) / (mass1 + mass2);
        double newSpeedNormal2 = (speedNormal2 * (mass2 - mass1) + 2 * mass1 * speedNormal1) / (mass1 + mass2);

        // **Stap 8: Zet snelheden terug om naar x en y**
        SpeedX = newSpeedNormal1 * normalX + speedTangent1 * tangentX;
        SpeedY = newSpeedNormal1 * normalY + speedTangent1 * tangentY;

        otherParticle.SpeedX = newSpeedNormal2 * normalX + speedTangent2 * tangentX;
        otherParticle.SpeedY = newSpeedNormal2 * normalY + speedTangent2 * tangentY;

        // **Stap 9: Bereken nieuwe positie van otherParticle met elapsedRemaining**
        otherParticle.X += (int)(otherParticle.SpeedX * elapsedRemaining);
        otherParticle.Y += (int)(otherParticle.SpeedY * elapsedRemaining);
    }

    public void Calculate(Stopwatch stopwatch)
    {
        var originalPoint = new Point(X, Y);

        var newElapsed = stopwatch.Elapsed.TotalSeconds;
        var elapsed = newElapsed - LastElapsed;
        LastElapsed = newElapsed;

        var distanceX = Convert.ToInt32(elapsed * SpeedX);
        var distanceY = Convert.ToInt32(elapsed * SpeedY);

        X += distanceX;
        Y += distanceY;

        if (X < 0 && SpeedX < 0)
        {
            X *= -1;
            SpeedX *= -1;
        }
        var width = ClientRectangle.Width - Size;
        if (X > width && SpeedX > 0)
        {
            X = width - (X - width);
            SpeedX *= -1;
        }
        if (Y < 0 && SpeedY < 0)
        {
            Y *= -1;
            SpeedY *= -1;
        }
        var height = ClientRectangle.Height - Size;
        if (Y > height && SpeedY > 0)
        {
            Y = height - (Y - height);
            SpeedY *= -1;
        }

        foreach (var particle in Parent.List)
        {
            if (particle != this && particle.IsOverlappingWith(this))
            {
                particle.BounceWith(this, originalPoint, elapsed);
            }
        }
    }

    public void DrawParticle(Graphics g)
    {
        g.FillEllipse(Brushes.DarkGray, X, Y, Size, Size);
        g.DrawEllipse(Pens.White, X, Y, Size, Size);
    }
}
