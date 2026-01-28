using SQLite;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;

namespace ApplicationDevelopment.Model
{
    // Model class representing a single journal entry in the application
    public class JournalEntry
    {
        // Unique identifier for each journal entry (Primary Key in SQLite)
        [PrimaryKey]
        public Guid Id { get; set; } = Guid.NewGuid();

        // Date and time when the journal entry was created
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Title of the journal entry (optional)
        public string? Title { get; set; }

        // Primary mood selected by the user
        public string? PrimaryMood { get; set; }

        // Additional moods selected by the user (optional)
        public string? SecondaryMood1 { get; set; }
        public string? SecondaryMood2 { get; set; }

        // Computed mood category based on the primary mood (Positive / Neutral / Negative / Unknown)
        public string? MoodCategory { get; set; }

        // Main journal content (mandatory field validated using DataAnnotations)
        [Required(ErrorMessage = "Please write something.")]
        public string? Content { get; set; }

        // Tags stored in the database as JSON string (to support multiple tags in SQLite)
        public string TagsJson { get; set; } = "[]";

        // List of tags used in the application logic (not stored directly in database)
        [Ignore]
        public List<string> Tags { get; set; } = new();

        // Temporary input field for adding new tags (not stored in database)
        [Ignore]
        public string CurrentTagInput { get; set; } = "";

        // Converts the Tags list into JSON format before saving to the database
        public void SyncTags()
        {
            TagsJson = JsonSerializer.Serialize(Tags);
        }

        // Loads tags from JSON string into the Tags list after retrieving from the database
        public void LoadTags()
        {
            Tags = string.IsNullOrEmpty(TagsJson)
                ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(TagsJson)!;
        }

        // Determines the mood category based on the primary mood selected by the user
        public void SetMoodCategory()
        {
            var positive = new List<string> { "Happy", "Excited", "Relaxed", "Grateful", "Confident" };
            var neutral = new List<string> { "Calm", "Thoughtful", "Curious", "Nostalgic", "Bored" };
            var negative = new List<string> { "Sad", "Angry", "Stressed", "Lonely", "Anxious", "Tired" };

            // If no primary mood is selected, assign Unknown category
            if (string.IsNullOrEmpty(PrimaryMood))
            {
                MoodCategory = "Unknown";
                return;
            }

            // Assign mood category based on predefined mood lists
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
