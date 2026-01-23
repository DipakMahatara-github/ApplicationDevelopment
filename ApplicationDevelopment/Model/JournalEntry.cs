using SQLite;
using System.ComponentModel.DataAnnotations;

namespace ApplicationDevelopment.Model
{
    public class JournalEntry
    {
        [PrimaryKey]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string? Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string? PrimaryMood { get; set; }
        public string? SecondaryMood { get; set; }

        public string TagsString { get; set; } = "";

        [Ignore]
        public List<string> Tags
        {
            get => string.IsNullOrEmpty(TagsString) ? new List<string>() : TagsString.Split(',').ToList();
            set => TagsString = string.Join(",", value);
        }

        [Ignore]
        public string? CurrentTagInput { get; set; }
    }
}