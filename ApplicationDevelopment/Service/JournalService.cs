using ApplicationDevelopment.Model;
using SQLite;

namespace ApplicationDevelopment.Service
{
    // Service layer responsible for database operations related to JournalEntry
    public class JournalService
    {
        // SQLite database connection (shared across the app)
        private static SQLiteAsyncConnection? _db;

        // Initialize database connection and ensure required tables/columns exist
        private static async Task Init()
        {
            if (_db != null) return; // Prevent multiple initializations

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "journal.db3");

            // Create database file if it does not exist
            if (!File.Exists(dbPath))
            {
                File.Create(dbPath).Close();
            }

            _db = new SQLiteAsyncConnection(dbPath);

            // Create JournalEntry table if not already created
            await _db.CreateTableAsync<JournalEntry>();

            // Ensure required columns exist (for backward compatibility)
            await EnsureMoodCategoryColumn();
            await EnsureTitleColumn();
        }

        // Ensures the MoodCategory column exists in the database table
        private static async Task EnsureMoodCategoryColumn()
        {
            var columns = await _db!.GetTableInfoAsync("JournalEntry");

            if (!columns.Any(c => c.Name == "MoodCategory"))
            {
                await _db.ExecuteAsync("ALTER TABLE JournalEntry ADD COLUMN MoodCategory TEXT");
            }
        }

        // Ensures the Title column exists in the database table
        private static async Task EnsureTitleColumn()
        {
            var columns = await _db!.GetTableInfoAsync("JournalEntry");

            if (!columns.Any(c => c.Name == "Title"))
            {
                await _db.ExecuteAsync("ALTER TABLE JournalEntry ADD COLUMN Title TEXT");
            }
        }

        // Retrieves all journal entries sorted by date (latest first)
        public static async Task<List<JournalEntry>> GetAllEntriesAsync()
        {
            await Init();

            var entries = await _db!.Table<JournalEntry>()
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            // Load tags and compute mood category for each entry
            foreach (var e in entries)
            {
                e.LoadTags();
                e.SetMoodCategory();
            }

            return entries;
        }

        // Retrieves today's journal entry (if any)
        public static async Task<JournalEntry?> GetTodayEntryAsync()
        {
            await Init();

            var start = DateTime.Today;
            var end = start.AddDays(1);

            var entry = await _db!.Table<JournalEntry>()
                .Where(e => e.CreatedAt >= start && e.CreatedAt < end)
                .FirstOrDefaultAsync();

            if (entry != null)
            {
                entry.LoadTags();
                entry.SetMoodCategory();
            }

            return entry;
        }

        // Adds a new journal entry (restricted to one entry per day)
        public static async Task<bool> AddEntryAsync(JournalEntry entry)
        {
            await Init();

            var todayEntry = await GetTodayEntryAsync();

            // Prevent multiple entries on the same day
            if (todayEntry != null)
            {
                return false;
            }

            // Synchronize tags and assign mood category before saving
            entry.SyncTags();
            entry.SetMoodCategory();

            await _db!.InsertAsync(entry);
            return true;
        }

        // Updates an existing journal entry in the database
        public static async Task UpdateEntryAsync(JournalEntry entry)
        {
            await Init();

            entry.SyncTags();
            entry.SetMoodCategory();

            await _db!.UpdateAsync(entry);
        }

        // Deletes a journal entry by its unique ID
        public static async Task DeleteEntryAsync(Guid id)
        {
            await Init();

            var entry = await GetEntryByIdAsync(id);
            if (entry != null)
                await _db!.DeleteAsync(entry);
        }

        // Retrieves a specific journal entry using its ID
        public static async Task<JournalEntry?> GetEntryByIdAsync(Guid id)
        {
            await Init();

            var entry = await _db!.Table<JournalEntry>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (entry != null)
            {
                entry.LoadTags();
                entry.SetMoodCategory();
            }

            return entry;
        }
    }
}
