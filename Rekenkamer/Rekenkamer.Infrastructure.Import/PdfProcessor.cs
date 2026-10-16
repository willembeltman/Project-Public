using Microsoft.Extensions.AI;
using Pgvector;
using Rekenkamer.Infrastructure.Data.Entities;
using SmartComponents.LocalEmbeddings;
using Tesseract;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Rendering.Skia;

namespace Rekenkamer.Infrastructure.Import;

/// <summary>
/// Singleton
/// </summary>
public class PdfProcessor(
    IEmbeddingGenerator<string, Embedding<float>> generator)
{
    public async Task<long> ReadFile(ApplicationDbContext db, LocalEmbedder embedder, Shared.Dtos.UploadFile file, long currentIndex = 0, Shared.Interfaces.IUploadHub? responseHub = null)
    {
        var fullName = $"{file.RelativePath}/{file.Name}";

        if (db.Documents.Any(a => a.Name == file.Name && a.Size == file.Bytes.Length))
        {
            SendStatus(responseHub, ++currentIndex, $"Error reading {fullName}: File already found");
            return currentIndex;
        }

        //SendStatus(responseHub, ++currentIndex, $"Inlezen van file: {fileName}");

        var paragraphs = ProcessPdf(file, ref currentIndex, responseHub);

        //        // Semantic Kernel splitst op basis van tokens/woorden en behoudt zinnen!
        //#pragma warning disable SKEXP0050 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        //        List<string> lines = TextChunker.SplitPlainTextLines(completeText, maxTokensPerLine: 40);
        //        List<string> chunks = TextChunker.SplitPlainTextParagraphs(lines, maxTokensPerParagraph: 200, overlapTokens: 30);
        //#pragma warning restore SKEXP0050 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

        var doc = new Document
        {
            RelativePath = file.RelativePath,
            Name = file.Name,
            FullName = fullName,
            Size = file.Bytes.Length
        };
        foreach (var (paragraph, index) in paragraphs.Select((t, i) => (t, i)))
        {
            var documentQuery = $"search_document: {paragraph.Text}";
            var rawVector = await generator.GenerateVectorAsync(documentQuery);
            var vector = new Vector(rawVector.ToArray());

            var text = paragraph.Text;
            //var embedding = embedder.Embed(text);
            var chunk = new DocumentChunk
            {
                Content = text,
                ChunkIndex = index,
                PageNumber = paragraph.PageNumber,
                Vector = vector,
                //EmbeddingF32 = embedding.Buffer.ToArray(),
                ParagraphConfidence = paragraph.Confidence,
                Left = paragraph.BoundingBox.Left,
                Width = paragraph.BoundingBox.Width,
                Top = paragraph.BoundingBox.Top,
                Height = paragraph.BoundingBox.Height
            };
            doc.Chunks.Add(chunk);
        }
        db.Documents.Add(doc);
        db.SaveChanges();

        return currentIndex;
    }

    private static void SendStatus(Shared.Interfaces.IUploadHub? responseHub, long index, string value)
    {
        responseHub?.UpdateStatus(index, $"{DateTime.Now} {value}");
        Console.WriteLine($"{DateTime.Now} {value}");
    }

    private List<Paragraph> ProcessPdf(Shared.Dtos.UploadFile file, ref long currentIndex, Shared.Interfaces.IUploadHub? responseHub)
    {
        using var document = PdfDocument.Open(file.Bytes);
        document.AddSkiaPageFactory();

        var fileName = $"{file.RelativePath}/{file.Name}";
        SendStatus(responseHub, ++currentIndex, $"Busy reading {file.RelativePath}/{file.Name} (PDF pages: {document.NumberOfPages})");

        var responseParagraphs = new List<Paragraph>();
        for (int pageNumber = 1; pageNumber <= document.NumberOfPages; pageNumber++)
        {
            var page = document.GetPage(pageNumber);
            var words = page.GetWords();
            var pageParagraphs = GetText(words, pageNumber);

            if (pageParagraphs.Count == 0)
            {
                SendStatus(responseHub, ++currentIndex, $"Page {pageNumber}: no text → OCR needed");

                // Dit werkt nu correct zonder uitzonderingen!
                using var image = document.GetPageAsPng(pageNumber);
                pageParagraphs = ProcessAsImage(image.ToArray(), pageNumber);
            }

            responseParagraphs.AddRange(pageParagraphs);
        }
        return responseParagraphs;
    }

    private List<Paragraph> GetText(IEnumerable<Word> words, int pageIndex)
    {
        var paragraphs = new List<Paragraph>();
        foreach (var word in words)
        {
            var currentParagraph = paragraphs
                .FirstOrDefault(paragraph => paragraph.IsNear(word));
            if (currentParagraph == null)
            {
                currentParagraph = new Paragraph(pageIndex, word.BoundingBox);
                paragraphs.Add(currentParagraph);
            }
            currentParagraph.Add(word);
        }
        return paragraphs;
    }

    private List<Paragraph> ProcessAsImage(byte[] image, int pageNumber)
    {
        using var engine = new Tesseract.TesseractEngine(
            @"./tessdata",
            "nld+eng",
            Tesseract.EngineMode.Default
        );

        using var pix = Tesseract.Pix.LoadFromMemory(image);
        using var page = engine.Process(pix);

        var paragraphs = new List<Paragraph>();

        // 3. Vraag de ResultIterator op van het Page object
        using (var iter = page.GetIterator())
        {
            iter.Begin();

            do
            {
                // 4. Haal de tekst op van het gewenste niveau (PageIteratorLevel.Paragraph)
                string paragraphText = iter.GetText(PageIteratorLevel.Para);

                // 5. Optioneel: Haal de coördinaten (regio) en betrouwbaarheidsscore op
                if (iter.TryGetBoundingBox(PageIteratorLevel.Para, out Rect boundingBox))
                {
                    float confidence = iter.GetConfidence(PageIteratorLevel.Para);

                    var paragraph = new Paragraph(
                        pageNumber,
                        new PdfRectangle(boundingBox.X1, boundingBox.Y1, boundingBox.X2, boundingBox.Y2),
                        paragraphText,
                        confidence);
                    paragraphs.Add(paragraph);
                }

                // 6. Spring naar de volgende paragraaf via het gekozen niveau
            } while (iter.Next(PageIteratorLevel.Para));
        }
        return paragraphs;
    }
}
