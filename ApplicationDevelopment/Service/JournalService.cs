using ApplicationDevelopment.Model;
using SQLite;

namespace ApplicationDevelopment.Service
{
    public class JournalService
    {
        private static SQLiteAsyncConnection? _db;

        private static async Task Init()
        {
            if (_db != null) return;

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "journal.db3");

            if (!File.Exists(dbPath))
            {
                File.Create(dbPath).Close();
            }

            _db = new SQLiteAsyncConnection(dbPath);

            await _db.CreateTableAsync<JournalEntry>();

            await EnsureMoodCategoryColumn();
            await EnsureTitleColumn();
        }

        private static async Task EnsureMoodCategoryColumn()
        {
            var columns = await _db!.GetTableInfoAsync("JournalEntry");

            if (!columns.Any(c => c.Name == "MoodCategory"))
            {
                await _db.ExecuteAsync("ALTER TABLE JournalEntry ADD COLUMN MoodCategory TEXT");
            }
        }

        private static async Task EnsureTitleColumn()
        {
            var columns = await _db!.GetTableInfoAsync("JournalEntry");

            if (!columns.Any(c => c.Name == "Title"))
            {
                await _db.ExecuteAsync("ALTER TABLE JournalEntry ADD COLUMN Title TEXT");
            }
        }

        public static async Task<List<JournalEntry>> GetAllEntriesAsync()
        {
            await Init();

            var entries = await _db!.Table<JournalEntry>()
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            foreach (var e in entries)
            {
                e.LoadTags();
                e.SetMoodCategory();
            }

            return entries;
        }

        // ✅ FIXED: Get today's entry (SQLite-safe)
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

        // ✅ Add entry (only one per day)
        public static async Task<bool> AddEntryAsync(JournalEntry entry)
        {
            await Init();

            var todayEntry = await GetTodayEntryAsync();

            if (todayEntry != null)
            {
                return false; // ❌ already exists today
            }

            entry.SyncTags();
            entry.SetMoodCategory();

            await _db!.InsertAsync(entry);
            return true;
        }

        public static async Task UpdateEntryAsync(JournalEntry entry)
        {
            await Init();

            entry.SyncTags();
            entry.SetMoodCategory();

            await _db!.UpdateAsync(entry);
        }

        public static async Task DeleteEntryAsync(Guid id)
        {
            await Init();

            var entry = await GetEntryByIdAsync(id);
            if (entry != null)
                await _db!.DeleteAsync(entry);
        }

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
