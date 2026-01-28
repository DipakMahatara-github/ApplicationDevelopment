using ApplicationDevelopment.Model;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace ApplicationDevelopment.Service
{
    // Service responsible for generating PDF reports from journal entries
    public class PdfExportService
    {
        // Generates a PDF document from a list of journal entries and returns it as byte array
        public byte[] GenerateJournalPdf(List<JournalEntry> entries)
        {
            // Create an in-memory stream to store the PDF file
            using var stream = new MemoryStream();

            // Initialize PDF writer and document using iText library
            var writer = new PdfWriter(stream);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            // Handle case when there are no journal entries to export
            if (entries == null || entries.Count == 0)
            {
                document.Add(new Paragraph("No journal entries found."));
                document.Close();
                return stream.ToArray();
            }

            // Sort entries chronologically by creation date
            entries = entries.OrderBy(e => e.CreatedAt).ToList();

            // Determine date range of exported entries
            var startDate = entries.First().CreatedAt;
            var endDate = entries.Last().CreatedAt;

            // Calculate summary statistics for the report
            int totalEntries = entries.Count;
            int positiveCount = entries.Count(e => e.MoodCategory == "Positive");
            int neutralCount = entries.Count(e => e.MoodCategory == "Neutral");
            int negativeCount = entries.Count(e => e.MoodCategory == "Negative");

            // =========================
            // Report Title Section
            // =========================
            var title = new Paragraph("Journal History Report")
                .SetFontSize(20);

            document.Add(title);

            // Add metadata and summary information to the PDF
            document.Add(new Paragraph($"Generated on: {DateTime.Now:dd MMM yyyy HH:mm}"));
            document.Add(new Paragraph($"Date Range: {startDate:dd MMM yyyy} - {endDate:dd MMM yyyy}"));
            document.Add(new Paragraph($"Total Entries: {totalEntries}"));
            document.Add(new Paragraph($"Summary: Positive = {positiveCount}, Neutral = {neutralCount}, Negative = {negativeCount}"));
            document.Add(new Paragraph("--------------------------------------------------\n"));

            // =========================
            // Journal Entries Section
            // =========================
            foreach (var entry in entries)
            {
                document.Add(new Paragraph(""));

                // Display entry date and time
                document.Add(new Paragraph($"Date: {entry.CreatedAt:dd MMM yyyy HH:mm}"));

                // Display title only if it exists
                if (!string.IsNullOrWhiteSpace(entry.Title))
                    document.Add(new Paragraph($"Title: {entry.Title}"));

                // Combine primary and secondary moods without duplicates or empty values
                var moods = new List<string>();

                if (!string.IsNullOrWhiteSpace(entry.PrimaryMood))
                    moods.Add(entry.PrimaryMood);

                if (!string.IsNullOrWhiteSpace(entry.SecondaryMood1) && !moods.Contains(entry.SecondaryMood1))
                    moods.Add(entry.SecondaryMood1);

                if (!string.IsNullOrWhiteSpace(entry.SecondaryMood2) && !moods.Contains(entry.SecondaryMood2))
                    moods.Add(entry.SecondaryMood2);

                document.Add(new Paragraph($"Mood: {string.Join(" / ", moods)}"));

                // Display mood category
                document.Add(new Paragraph($"Mood Category: {entry.MoodCategory}"));

                // Display journal content
                document.Add(new Paragraph($"Content: {entry.Content}"));

                // Display tags if available
                if (entry.Tags != null && entry.Tags.Any())
                    document.Add(new Paragraph($"Tags: {string.Join(", ", entry.Tags)}"));

                // Separator between entries
                document.Add(new Paragraph("--------------------------------------------------"));
            }

            // Finalize and close the PDF document
            document.Close();

            // Return generated PDF as byte array
            return stream.ToArray();
        }
    }
}
