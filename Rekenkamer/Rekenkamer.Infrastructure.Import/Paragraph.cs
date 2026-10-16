using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Core;

namespace Rekenkamer.Infrastructure.Import;

public class Paragraph
{
    // Toleranties (in PDF-units)
    const double maxVerticalGap = 12;   // regelafstand
    const double maxHorizontalGap = 20; // woordafstand

    public Paragraph(int pageNumber, PdfRectangle boundingBox)
    {
        PageNumber = pageNumber;
        BoundingBox = boundingBox;
    }
    public Paragraph(int pageNumber, PdfRectangle boundingBox, string paragraphText, float confidence)
    {
        PageNumber = pageNumber;
        BoundingBox = boundingBox;
        ParagraphText = paragraphText;
        Confidence = confidence;
    }

    public int PageNumber { get; }
    private string? ParagraphText { get; }
    public float? Confidence { get; }

    public PdfRectangle BoundingBox { get; private set; }
    private List<Word> Words { get; set; } = new();

    public string Text
    {
        get
        {
            if (ParagraphText != null)
                return ParagraphText;

            var lines = Words!
                .GroupBy(w => Math.Round(w.BoundingBox.Bottom, 1))
                .OrderByDescending(g => g.Key)
                .Select(line =>
                    string.Join(" ", line
                        .OrderBy(w => w.BoundingBox.Left)
                        .Select(w => w.Text)));

            var text = string.Join(Environment.NewLine, lines);
            return text;
        }
    }

    public void Add(Word word)
    {
        Words.Add(word);

        // Bounding box uitbreiden (union)
        var left = Math.Min(BoundingBox.Left, word.BoundingBox.Left);
        var bottom = Math.Min(BoundingBox.Bottom, word.BoundingBox.Bottom);
        var right = Math.Max(BoundingBox.Right, word.BoundingBox.Right);
        var top = Math.Max(BoundingBox.Top, word.BoundingBox.Top);

        BoundingBox = new PdfRectangle(left, bottom, right, top);
    }
    public bool IsNear(Word word)
    {
        var box = word.BoundingBox;

        // Verticale afstand
        var verticalGap = Math.Min(
            Math.Abs(box.Bottom - BoundingBox.Top),
            Math.Abs(BoundingBox.Bottom - box.Top)
        );

        // Horizontale overlap
        var horizontalOverlap =
            box.Left <= BoundingBox.Right &&
            box.Right >= BoundingBox.Left;

        // Dicht genoeg verticaal?
        if (verticalGap > maxVerticalGap)
            return false;

        // Overlap of bijna overlap horizontaal?
        if (horizontalOverlap)
            return true;

        // Alternatief: kleine horizontale afstand
        var horizontalGap = Math.Min(
            Math.Abs(box.Left - BoundingBox.Right),
            Math.Abs(BoundingBox.Left - box.Right)
        );

        return horizontalGap < maxHorizontalGap;
    }

}
