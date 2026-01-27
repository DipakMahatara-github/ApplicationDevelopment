using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using ApplicationDevelopment.Model;

namespace ApplicationDevelopment.Service
{
    public class PdfExportService
    {
        public byte[] GenerateJournalPdf(List<JournalEntry> entries)
        {
            using var ms = new MemoryStream();

            var writer = new PdfWriter(ms);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            // ✅ Title
            document.Add(new Paragraph("Journal History Report")
                .SetFontSize(18));

            document.Add(new Paragraph($"Generated on: {DateTime.Now}")
                .SetFontSize(10));

            document.Add(new Paragraph(" ")); // spacing

            foreach (var entry in entries)
            {
                document.Add(new Paragraph($"Date: {entry.CreatedAt:MMM dd, yyyy HH:mm}"));
                document.Add(new Paragraph($"Mood: {entry.PrimaryMood} {(!string.IsNullOrEmpty(entry.SecondaryMood) ? "/ " + entry.SecondaryMood : "")}"));
                document.Add(new Paragraph($"Content: {entry.Content ?? ""}"));

                if (entry.Tags != null && entry.Tags.Any())
                {
                    document.Add(new Paragraph("Tags: " + string.Join(", ", entry.Tags)));
                }

                document.Add(new Paragraph("-------------------------------------"));
            }

            document.Close();
            return ms.ToArray();
        }
    }
}