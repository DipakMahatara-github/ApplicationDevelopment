using System.ComponentModel.DataAnnotations;

namespace ApplicationDevelopment.Model
{
    public class JournalEntry
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Please write something in your journal.")]
        public string? Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "A primary mood is required.")]
        public string? PrimaryMood { get; set; }

        public string? SecondaryMood { get; set; }

        public List<string> Tags { get; set; } = new List<string>();

        public string? CurrentTagInput { get; set; }
    }
}