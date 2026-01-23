using ApplicationDevelopment.Model;
using SQLite;

namespace ApplicationDevelopment.Service
{
    public static class JournalService
    {
        private static SQLiteAsyncConnection? _db;

        private static async Task Init()
        {
            if (_db != null) return;

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "journal.db3");
            _db = new SQLiteAsyncConnection(dbPath);
            await _db.CreateTableAsync<JournalEntry>();
        }

        public static async Task<List<JournalEntry>> GetAllEntriesAsync()
        {
            await Init();
            return await _db!.Table<JournalEntry>()
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public static async Task AddEntryAsync(JournalEntry entry)
        {
            await Init();
            await _db!.InsertAsync(entry);
        }

        public static async Task UpdateEntryAsync(JournalEntry entry)
        {
            await Init();
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
            return await _db!.Table<JournalEntry>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}