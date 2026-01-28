using SQLite;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;

namespace ApplicationDevelopment.Model
{
    public class JournalEntry
    {
        [PrimaryKey]
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // ✅ TITLE
        public string? Title { get; set; }

        // ✅ PRIMARY MOOD
        public string? PrimaryMood { get; set; }

        // ✅ SECONDARY MOODS
        public string? SecondaryMood1 { get; set; }
        public string? SecondaryMood2 { get; set; }

        // ✅ MOOD CATEGORY
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
