using SQLite;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace ApplicationDevelopment.Model
{
    public class JournalEntry
    {
        // ✅ PRIMARY KEY
        [PrimaryKey]
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // ✅ MOODS
        public string? PrimaryMood { get; set; }
        public string? SecondaryMood { get; set; }

        // ✅ CONTENT
        [Required(ErrorMessage = "Please write something.")]
        public string? Content { get; set; }

        // ✅ TAGS STORED IN SQLITE (JSON STRING)
        public string TagsJson { get; set; } = "[]";

        // ✅ TAG LIST (NOT STORED IN DB)
        [Ignore]
        public List<string> Tags { get; set; } = new();

        // ✅ INPUT FIELD (NOT STORED IN DB)
        [Ignore]
        public string CurrentTagInput { get; set; } = "";

        // ✅ Convert List → JSON before saving
        public void SyncTags()
        {
            TagsJson = JsonSerializer.Serialize(Tags);
        }

        // ✅ Convert JSON → List after loading
        public void LoadTags()
        {
            Tags = string.IsNullOrEmpty(TagsJson)
                ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(TagsJson)!;
        }
    }
}