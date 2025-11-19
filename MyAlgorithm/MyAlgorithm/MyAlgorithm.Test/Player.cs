
namespace MyAlgorithm.Test;

public class Player(long id, string name) : IPlayer
{
    public long Id { get; set; } = id;
    public string Name { get; set; } = name;
    public double Score { get; set; }
    public long Rank { get; set; }
    public DateTime RankDate { get; set; }
}