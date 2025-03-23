using CPUCalculator2.Services;

namespace CPUCalculator2.Data;

public class PassmarkSimpleCpu
{
    public string? Name { get; set; }
    public string? Link { get; set; }
    public double Score { get; set; }
    public ScoreTypeEnum ScoreType { get; internal set; }
}