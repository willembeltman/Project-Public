namespace ConsoleApp1;

class Program
{
    //static void Main (string[] args)
    //{
    //    using (var game = new Game(500, 500))
    //    {
    //        game.Run();
    //    }
    //}
    public static void Main()
    {
        using (VideoPlayer player = new VideoPlayer())
        {
            player.Run(); 
        }
    }
}