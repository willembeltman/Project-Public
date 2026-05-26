var subtitles = SrtParser.Parse("OpnameEnverOrigineel.srt");

var list = new List<ParsedSubtitle>();
var currentIndex = 1;
var currentSubtitle = (ParsedSubtitle?)null;
var currentText = string.Empty;

foreach (var subtitle in subtitles)
{
    var start = subtitle.Start;
    var end = subtitle.End;
    var speaker = subtitle.Text.Split(':').First();
    var text = subtitle.Text.Substring(speaker.Length + 1).TrimStart();
    if (currentSubtitle == null || 
        currentSubtitle.Speaker != speaker ||
        currentSubtitle.End != start ||
        currentSubtitle.Text.Split('\n').Length > 7)
    {
        currentSubtitle = new ParsedSubtitle(currentIndex, start, end, speaker, text);
        list.Add(currentSubtitle);
        currentIndex++;
    }
    else
    {
        currentSubtitle.End = end;
        currentSubtitle.Text += Environment.NewLine + text;
    }
}

var output = string.Empty;

foreach (var subtitle in list)
{
    output += $@"{subtitle.Index}
{subtitle.Start:hh\:mm\:ss\,fff} --> {subtitle.End:hh\:mm\:ss\,fff}
{subtitle.Speaker}: „{subtitle.Text}”

";

    Console.WriteLine($@"{subtitle.Index}
{subtitle.Start:hh\:mm\:ss\,fff} --> {subtitle.End:hh\:mm\:ss\,fff}
{subtitle.Speaker}: „{subtitle.Text}”

");
}

File.WriteAllText("OpnameEnver.srt", output);

class ParsedSubtitle
{
    public ParsedSubtitle(int index, TimeSpan start, TimeSpan end, string author, string text)
    {
        Index = index;
        Start = start;
        End = end;
        Speaker = author;
        Text = text;
    }

    public int Index { get; set; }
    public TimeSpan Start { get; set; }
    public TimeSpan End { get; set; }
    public string Speaker { get; set; }
    public string Text { get; set; }
}