using System.Text.Json;

namespace NotesApp
{
    public static class NoteService
    {
        private static readonly string filePath = Path.Combine(FileSystem.AppDataDirectory, "notes.json");
        private static List<Note> _cachedNotes = new List<Note>();

        // Завантажити нотатки з файлу асинхронно
        public static async Task<List<Note>> LoadNotesAsync()
        {
            if (_cachedNotes.Count > 0)
                return _cachedNotes;

            if (!File.Exists(filePath))
                return _cachedNotes = new List<Note>();

            try
            {
                string json = await File.ReadAllTextAsync(filePath);
                _cachedNotes = JsonSerializer.Deserialize<List<Note>>(json) ?? new List<Note>();
                return _cachedNotes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при завантаженні нотаток: {ex.Message}");
                return _cachedNotes = new List<Note>();
            }
        }

        public static async Task SaveNotesAsync(IEnumerable<Note> notes)
        {
            try
            {
                string json = JsonSerializer.Serialize(notes, new JsonSerializerOptions { WriteIndented = false });
                await File.WriteAllTextAsync(filePath, json);
                _cachedNotes = notes.ToList(); // Оновлюємо кеш після успішного збереження
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при збереженні нотаток: {ex.Message}");
            }
        }

        // Додати нову нотатку асинхронно
        public static async Task AddNoteAsync(Note note)
        {
            var notes = await LoadNotesAsync();
            notes.Add(note);
            await SaveNotesAsync(notes);
        }
    }
}