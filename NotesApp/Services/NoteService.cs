using System.Text.Json;

namespace NotesApp.Services
{
    public static class NoteService
    {
        private static readonly string filePath = Path.Combine(FileSystem.AppDataDirectory, "notes.json");
        private static List<Note> _cachedNotes = [];

        // Створюємо статичний екземпляр JsonSerializerOptions для повторного використання
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            WriteIndented = false // Налаштування серіалізації
        };

        // Завантажити нотатки з файлу асинхронно
        public static async Task<List<Note>> LoadNotesAsync()
        {
            if (_cachedNotes.Count > 0)
                return _cachedNotes;

            if (!File.Exists(filePath))
                return _cachedNotes = [];

            try
            {
                string json = await File.ReadAllTextAsync(filePath);
                _cachedNotes = JsonSerializer.Deserialize<List<Note>>(json, _jsonOptions) ?? [];
                return _cachedNotes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при завантаженні нотаток: {ex.Message}");
                return _cachedNotes = [];
            }
        }

        // Зберегти нотатки асинхронно
        public static async Task SaveNotesAsync(IEnumerable<Note> notes)
        {
            if (notes == null)
            {
                Console.WriteLine("Список нотаток є null.");
                return;
            }

            try
            {
                string json = JsonSerializer.Serialize(notes, _jsonOptions);
                await File.WriteAllTextAsync(filePath, json);
                _cachedNotes = [.. notes]; // Оновлюємо кеш після успішного збереження
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при збереженні нотаток: {ex.Message}");
            }
        }

        // Додати нову нотатку асинхронно
        public static async Task AddNoteAsync(Note note)
        {
            if (note == null)
            {
                Console.WriteLine("Нотатка є null.");
                return;
            }

            var notes = await LoadNotesAsync();
            notes.Add(note);
            await SaveNotesAsync(notes);
        }
    }
}