namespace MyAlgorithm;

public interface IPlayer
{
    long Id { get; }
    double Score { get; set; }
    long Rank { get; set; }
    DateTime RankDate { get; set; }
}

public interface IGame
{
    long WinnerId { get; }
    long LoserId { get; }
}

public record SubScore(
    IPlayer Me,
    IPlayer Other,
    double Score);

public class Calculator(IPlayer[] Players, IGame[] Games)
{
    public void Calculate()
    {
        // Need at least two players and one game
        if (Players.Count() < 2 || Games.Any() == false) return;

        // Create results tables
        List<SubScore> allSubScores = new List<SubScore>();

        // Iterate through all players
        foreach (var me in Players)
        {
            // Setup new subscore list for player
            List<SubScore> playerSubScores = new List<SubScore>();

            // Setup min and max score
            double maxscore = 0;
            double minscore = 0;

            // Iterate through all players except itself
            foreach (var other in Players.Where(a => a.Id != me.Id))
            {
                // Get the score
                double? score = Compair(other, me);

                // Has a connection?
                if (score.HasValue)
                {
                    // Create subscore
                    var subScore = new SubScore(me, other, score.Value);

                    // Set limits
                    if (maxscore < score.Value) maxscore = score.Value;
                    if (minscore > score.Value) minscore = score.Value;

                    // Add it to the subscores
                    playerSubScores.Add(subScore);
                }
            }

            // Recalculate scores with limits
            foreach (var subscore in playerSubScores)
            {
                // Score greater than zero?
                if (subscore.Score > 0)
                    allSubScores.Add(new SubScore(subscore.Me, subscore.Other, (subscore.Score / maxscore) * 1));
                // Score smaller than zero
                else if (subscore.Score < 0)
                    allSubScores.Add(new SubScore(subscore.Me, subscore.Other, (subscore.Score / minscore) * -1));
                // Score is zero
                else
                    allSubScores.Add(new SubScore(subscore.Me, subscore.Other, 0));
            }
        }

        // Calculate the overall score for each player
        foreach (IPlayer player in Players)
        {
            if (allSubScores.Where(a => a.Other.Id == player.Id).Any())
            {
                // Set the score
                player.Score = allSubScores
                            .Where(a => a.Other.Id == player.Id)
                            .Average(a => a.Score);
            }
        }

        // Set Rank
        double lastscore = 2;
        long rank = 0;
        foreach (IPlayer player in Players.OrderByDescending(a => a.Score))
        {
            if (player.Score < lastscore)
            {
                // Add a rank higher
                lastscore = player.Score;
                rank++;
            }

            // Set the Rank
            player.Rank = rank;

            // Set the date
            player.RankDate = DateTime.Now;
        }
    }

    private double? Compair(IPlayer me, IPlayer other)
    {
        // What is the direct score?
        double? direct = CompairDirect(me, other);

        // What is the relative score?
        double? relative = CompairAlternatives(me, other);

        // Direct and relative filled?
        if (direct == null)
        {
            if (relative == null) return null;
            else return relative;
        }
        else
        {
            if (relative == null) return direct;
            else return direct + relative;
        }
    }
    private double? CompairAlternatives(IPlayer me, IPlayer other)
    {
        // Make list for scores
        List<double> scores = new List<double>();

        // Select all players that are NOT trick or compairtrick
        foreach (IPlayer relation in Players.Where(a => a.Id != me.Id && a.Id != other.Id))
        {
            // Get trick <> relationtrick score
            double? first = CompairDirect(me, relation);

            // Get relationtrick <> compairtrick score
            double? second = CompairDirect(relation, other);

            // If both are filled, there is a relationship
            if (first != null && second != null)
            {
                // Add the score
                scores.Add(first.Value + second.Value);
            }
        }

        // Any scores found?
        if (!scores.Any()) return null;

        // Return average
        return scores.Average();
    }
    private double? CompairDirect(IPlayer me, IPlayer other)
    {
        // Get Wins
        double wins = Games.Count(a => a.WinnerId == me.Id && a.LoserId == other.Id);

        // Get loses
        double loses = Games.Count(a => a.LoserId == me.Id && a.WinnerId == other.Id);

        // No wins or loses?
        if ((loses == 0) && (wins == 0)) return null;

        // Calculate score
        return (wins / (loses + wins)) * 2 - 1;

        // -1          0          +1
        // =========================
    }
}