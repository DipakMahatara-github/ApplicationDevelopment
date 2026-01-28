using SQLite;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace ApplicationDevelopment.Model
{
    public class JournalEntry
    {
        [PrimaryKey]
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // ✅ PRIMARY MOOD
        [Required]
        public string? PrimaryMood { get; set; }

        // ✅ SECONDARY MOODS (UP TO 2)
        public string? SecondaryMood1 { get; set; }
        public string? SecondaryMood2 { get; set; }

        // ✅ CONTENT
        [Required(ErrorMessage = "Please write something.")]
        public string? Content { get; set; }

        // ✅ TAGS (JSON stored in DB)
        public string TagsJson { get; set; } = "[]";

        // ✅ TAG LIST (NOT stored in DB)
        [Ignore]
        public List<string> Tags { get; set; } = new();

        // ✅ TAG INPUT (NOT stored in DB)
        [Ignore]
        public string CurrentTagInput { get; set; } = "";

        // ✅ Sync tags before saving
        public void SyncTags()
        {
            TagsJson = JsonSerializer.Serialize(Tags);
        }

        // ✅ Load tags after reading
        public void LoadTags()
        {
            Tags = string.IsNullOrEmpty(TagsJson)
                ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(TagsJson)!;
        }
    }
}