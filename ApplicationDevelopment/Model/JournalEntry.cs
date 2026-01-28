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

        // ✅ MOOD CATEGORY (Positive / Neutral / Negative)
        public string? MoodCategory { get; set; }

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

        // ✅ SET MOOD CATEGORY
        public void SetMoodCategory()
        {
            var positive = new List<string> { "Happy", "Excited", "Relaxed", "Grateful", "Confident" };
            var neutral = new List<string> { "Calm", "Thoughtful", "Curious", "Nostalgic", "Bored" };
            var negative = new List<string> { "Sad", "Angry", "Stressed", "Lonely", "Anxious", "Tired" };

            if (string.IsNullOrEmpty(PrimaryMood))
            {
                MoodCategory = "Unknown";
                return;
            }

            if (positive.Contains(PrimaryMood))
                MoodCategory = "Positive";
            else if (neutral.Contains(PrimaryMood))
                MoodCategory = "Neutral";
            else if (negative.Contains(PrimaryMood))
                MoodCategory = "Negative";
            else
                MoodCategory = "Unknown";
        }
    }
}
