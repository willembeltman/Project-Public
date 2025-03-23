namespace CPUCalculator2.Data;

public class DownloadedCpu : PassmarkCpu
{
    public DownloadedCpu() : base()
    {
    }
    public DownloadedCpu(PassmarkCpu cpu) : base(
        cpu.Name,
        cpu.FullName,
        cpu.Link,
        cpu.SingleScore,
        cpu.MultiScore,
        cpu.OcScore)
    {
    }

    public DateTime Downloaded { get; set; } = DateTime.Now;
}
