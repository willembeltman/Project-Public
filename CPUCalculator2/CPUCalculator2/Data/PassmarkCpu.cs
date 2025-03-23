namespace CPUCalculator2.Data
{
    public class PassmarkCpu
    {
        public PassmarkCpu()
        {
            Name = "";
            FullName = "";
            Link = "";
        }
        public PassmarkCpu(string name, string fullName, string link, double singleScore, double multiScore, double? ocScore)
        {
            Name = name;
            FullName = fullName;
            Link = link;
            SingleScore = singleScore;
            MultiScore = multiScore;
            OcScore = ocScore;
        }

        public string Name { get; set; }
        public string FullName { get; set; }
        public string Link { get; set; }
        public double SingleScore { get; set; }
        public double MultiScore { get; set; }
        public double? OcScore { get; set; }
    }
}