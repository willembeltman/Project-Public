using CPUCalculator2.Enums;

namespace CPUCalculator2.Data;

public class PassmarkScore
{
    public string? Name { get; set; }
    public string? Link { get; set; }
    public double Score { get; set; }
    public ScoreTypeEnum ScoreType { get; internal set; }
}