using ApplicationDevelopment.Model;

namespace ApplicationDevelopment.Service
{
    public static class JournalService
    {
        // List of all journal entries
        public static List<JournalEntry> AllEntries { get; } = new List<JournalEntry>();

        // Add a new entry
        public static void AddEntry(JournalEntry entry)
        {
            AllEntries.Add(entry);
        }

        // Get an entry by ID
        public static JournalEntry? GetEntryById(Guid id)
        {
            return AllEntries.Find(x => x.Id == id);
        }

        // Update an existing entry
        public static void UpdateEntry(JournalEntry updatedEntry)
        {
            var index = AllEntries.FindIndex(x => x.Id == updatedEntry.Id);
            if (index >= 0)
            {
                AllEntries[index] = updatedEntry;
            }
        }

        // Delete an entry by ID
        public static void DeleteEntry(Guid id)
        {
            var itemToRemove = AllEntries.Find(x => x.Id == id);
            if (itemToRemove != null)
            {
                AllEntries.Remove(itemToRemove);
            }
        }
    }
}