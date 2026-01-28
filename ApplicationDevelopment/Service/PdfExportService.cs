using ApplicationDevelopment.Model;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace ApplicationDevelopment.Service
{
    public class PdfExportService
    {
        public byte[] GenerateJournalPdf(List<JournalEntry> entries)
        {
            using var stream = new MemoryStream();

            var writer = new PdfWriter(stream);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            if (entries == null || entries.Count == 0)
            {
                document.Add(new Paragraph("No journal entries found."));
                document.Close();
                return stream.ToArray();
            }

            // ✅ Sort entries by date
            entries = entries.OrderBy(e => e.CreatedAt).ToList();

            var startDate = entries.First().CreatedAt;
            var endDate = entries.Last().CreatedAt;

            int totalEntries = entries.Count;
            int positiveCount = entries.Count(e => e.MoodCategory == "Positive");
            int neutralCount = entries.Count(e => e.MoodCategory == "Neutral");
            int negativeCount = entries.Count(e => e.MoodCategory == "Negative");

            // =========================
            // ✅ TITLE (NO TextAlignment)
            // =========================
            var title = new Paragraph("Journal History Report")
                .SetFontSize(20);

            document.Add(title);

            document.Add(new Paragraph($"Generated on: {DateTime.Now:dd MMM yyyy HH:mm}"));
            document.Add(new Paragraph($"Date Range: {startDate:dd MMM yyyy} - {endDate:dd MMM yyyy}"));
            document.Add(new Paragraph($"Total Entries: {totalEntries}"));
            document.Add(new Paragraph($"Summary: Positive = {positiveCount}, Neutral = {neutralCount}, Negative = {negativeCount}"));
            document.Add(new Paragraph("--------------------------------------------------\n"));

            // =========================
            // ✅ ENTRIES
            // =========================
            foreach (var entry in entries)
            {
                document.Add(new Paragraph(""));

                document.Add(new Paragraph($"Date: {entry.CreatedAt:dd MMM yyyy HH:mm}"));

                if (!string.IsNullOrWhiteSpace(entry.Title))
                    document.Add(new Paragraph($"Title: {entry.Title}"));

                // ✅ Fix mood display (no duplicates, no empty values)
                var moods = new List<string>();

                if (!string.IsNullOrWhiteSpace(entry.PrimaryMood))
                    moods.Add(entry.PrimaryMood);

                if (!string.IsNullOrWhiteSpace(entry.SecondaryMood1) && !moods.Contains(entry.SecondaryMood1))
                    moods.Add(entry.SecondaryMood1);

                if (!string.IsNullOrWhiteSpace(entry.SecondaryMood2) && !moods.Contains(entry.SecondaryMood2))
                    moods.Add(entry.SecondaryMood2);

                document.Add(new Paragraph($"Mood: {string.Join(" / ", moods)}"));

                document.Add(new Paragraph($"Mood Category: {entry.MoodCategory}"));

                document.Add(new Paragraph($"Content: {entry.Content}"));

                if (entry.Tags != null && entry.Tags.Any())
                    document.Add(new Paragraph($"Tags: {string.Join(", ", entry.Tags)}"));

                document.Add(new Paragraph("--------------------------------------------------"));
            }


            document.Close();
            return stream.ToArray();
        }
    }
}
