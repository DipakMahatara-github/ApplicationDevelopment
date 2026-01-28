using ApplicationDevelopment.Model;
using iText.Kernel.Pdf;

namespace ApplicationDevelopment.Service
{
    public class PdfExportService
    {
        public byte[] GenerateJournalPdf(List<JournalEntry> entries)
        {
            using var stream = new MemoryStream();

            var writer = new PdfWriter(stream);
            var pdf = new PdfDocument(writer);
            var document = new iText.Layout.Document(pdf);

            // ✅ Title
            document.Add(new iText.Layout.Element.Paragraph("Journal History Report")
                .SetFontSize(20)
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

            document.Add(new iText.Layout.Element.Paragraph(
                $"Generated on: {DateTime.Now:dd MMM yyyy HH:mm}\n"));

            foreach (var entry in entries)
            {
                document.Add(new iText.Layout.Element.Paragraph(
                    "--------------------------------------------------"));

                document.Add(new iText.Layout.Element.Paragraph(
                    $"Date: {entry.CreatedAt:dd MMM yyyy HH:mm}"));

                if (!string.IsNullOrWhiteSpace(entry.Title))
                    document.Add(new iText.Layout.Element.Paragraph(
                        $"Title: {entry.Title}"));

                document.Add(new iText.Layout.Element.Paragraph(
                    $"Mood: {entry.PrimaryMood} / {entry.SecondaryMood1} / {entry.SecondaryMood2}"));

                document.Add(new iText.Layout.Element.Paragraph(
                    $"Mood Category: {entry.MoodCategory}"));

                document.Add(new iText.Layout.Element.Paragraph(
                    $"Content: {entry.Content}"));

                if (entry.Tags != null && entry.Tags.Any())
                    document.Add(new iText.Layout.Element.Paragraph(
                        $"Tags: {string.Join(", ", entry.Tags)}"));

                document.Add(new iText.Layout.Element.Paragraph("\n"));
            }

            document.Close();

            return stream.ToArray();
        }
    }
}
