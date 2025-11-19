namespace MyAlgorithm.Test;

public class Tests
{
    [Test]
    public void Test1()
    {
        var Players = new IPlayer[]
        {
            new Player(1, "Willem"),
            new Player(2, "Piet"),
            new Player(3, "Klaas")
        };
        var Games = new IGame[]
        {
            new Game(1, 2),
            new Game(2, 3),
            new Game(3, 1)
        };
        var calculator = new Calculator(Players, Games);
        calculator.Calculate();

        Assert.That(Players[0].Rank, Is.EqualTo(1));
        Assert.That(Players[1].Rank, Is.EqualTo(1));
        Assert.That(Players[2].Rank, Is.EqualTo(1));
        Assert.Pass();
    }

    [Test]
    public void Test2()
    {
        var Players = new IPlayer[]
        {
            new Player(1, "Willem"),
            new Player(2, "Piet"),
            new Player(3, "Klaas")
        };
        var Games = new IGame[]
        {
            new Game(1, 2),
            new Game(2, 3),
            new Game(1, 3)
        };
        var calculator = new Calculator(Players, Games);
        calculator.Calculate();

        Assert.That(Players[0].Rank, Is.EqualTo(1));
        Assert.That(Players[1].Rank, Is.EqualTo(2));
        Assert.That(Players[2].Rank, Is.EqualTo(3));
        Assert.Pass();
    }
    [Test]
    public void Test3()
    {
        var Players = new IPlayer[]
        {
            new Player(1, "Willem"),
            new Player(2, "Piet"),
            new Player(3, "Klaas")
        };
        var Games = new IGame[]
        {
            new Game(1, 2),
            new Game(2, 1),
            new Game(1, 3)
        };
        var calculator = new Calculator(Players, Games);
        calculator.Calculate();

        Assert.That(Players[0].Rank, Is.EqualTo(1));
        Assert.That(Players[1].Rank, Is.EqualTo(1));
        Assert.That(Players[2].Rank, Is.EqualTo(2));
        Assert.Pass();
    }
}
