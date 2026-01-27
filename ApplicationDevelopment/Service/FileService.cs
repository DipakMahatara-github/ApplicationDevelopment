using System;
using System.IO;
using System.Threading.Tasks;

namespace ApplicationDevelopment.Service
{
    public static class FileService
    {
        public static async Task<string> SavePdfAsync(byte[] pdfBytes)
        {
            var fileName = $"Journal_{DateTime.Now:yyyyMMddHHmmss}.pdf";

            // ✅ REAL MAC USER DOCUMENTS PATH (NOT SANDBOX)
            var homePath = Environment.GetEnvironmentVariable("HOME");
            var documentsPath = Path.Combine(homePath!, "Documents");

            var filePath = Path.Combine(documentsPath, fileName);

            await File.WriteAllBytesAsync(filePath, pdfBytes);

            return filePath;
        }
    }
}