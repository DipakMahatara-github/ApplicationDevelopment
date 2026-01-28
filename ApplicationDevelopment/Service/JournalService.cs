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
                e.SetMoodCategory(); // ✅ ADD
            }

            return entries;
        }

        public static async Task AddEntryAsync(JournalEntry entry)
        {
            await Init();

            entry.SyncTags();
            entry.SetMoodCategory(); // ✅ ADD
            await _db!.InsertAsync(entry);
        }

        public static async Task UpdateEntryAsync(JournalEntry entry)
        {
            await Init();

            entry.SyncTags();
            entry.SetMoodCategory(); // ✅ ADD
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
                entry.SetMoodCategory(); // ✅ ADD
            }

            return entry;
        }
    }
}
