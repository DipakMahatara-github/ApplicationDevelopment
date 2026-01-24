using System.ComponentModel.DataAnnotations;
using SQLite;
using System.Text.Json;

namespace ApplicationDevelopment.Model
{
    public class JournalEntry
    {
        [PrimaryKey]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Please write something.")]
        public string? Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public string? PrimaryMood { get; set; }

        public string? SecondaryMood { get; set; }

        // ✅ Stored in SQLite
        public string TagsJson { get; set; } = "[]";

        // ✅ Used in UI (REAL LIST)
        [Ignore]
        public List<string> Tags { get; set; } = new();

        // ✅ Sync Tags <-> TagsJson
        public void SyncTags()
        {
            TagsJson = JsonSerializer.Serialize(Tags);
        }

        public void LoadTags()
        {
            Tags = string.IsNullOrEmpty(TagsJson)
                ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(TagsJson)!;
        }

        [Ignore]
        public string? CurrentTagInput { get; set; }
    }
}