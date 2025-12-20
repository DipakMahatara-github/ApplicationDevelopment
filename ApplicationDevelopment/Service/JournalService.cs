using ApplicationDevelopment.Model;

namespace ApplicationDevelopment.Service
{
    public static class JournalService
    {
        public static List<JournalEntry> AllEntries { get; } = new List<JournalEntry>();

        public static void AddEntry(JournalEntry entry)
        {
            AllEntries.Add(entry);
        }

        public static JournalEntry? GetEntryById(Guid id)
        {
            return AllEntries.Find(x => x.Id == id);
        }

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