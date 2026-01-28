using System;
using System.IO;
using System.Threading.Tasks;

namespace ApplicationDevelopment.Service
{
    // Utility class responsible for saving generated PDF files to the user's system
    public static class PdfFileSaver
    {
        // Saves PDF bytes to a file and returns the full file path
        public static async Task<string> SavePdfAsync(byte[] pdfBytes)
        {
            // Generate a unique file name using current timestamp to avoid overwriting files
            var fileName = $"Journal_{DateTime.Now:yyyyMMddHHmmss}.pdf";

            // Get the user's home directory path from the environment variables
            var homePath = Environment.GetEnvironmentVariable("HOME");

            // Build the path to the Documents folder inside the user's home directory
            var documentsPath = Path.Combine(homePath!, "Documents");

            // Combine folder path and file name to create the full file path
            var filePath = Path.Combine(documentsPath, fileName);

            // Write the PDF byte array asynchronously to the file system
            await File.WriteAllBytesAsync(filePath, pdfBytes);

            // Return the saved file path so it can be displayed to the user
            return filePath;
        }
    }
}