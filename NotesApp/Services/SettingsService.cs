using System.Diagnostics;
using System.Text.Json;

namespace NotesApp.Services
{
    public static class SettingsService
    {
        // Використовуємо повний шлях до файлу налаштувань
        private static readonly string SettingsFilePath = Path.Combine(FileSystem.AppDataDirectory, "settings.json");

        // Зберігаємо налаштування
        public static void SaveSetting<T>(string key, T value)
        {
            try
            {
                // Завантажуємо поточні налаштування
                var settings = LoadSettings();

                // Оновлюємо або додаємо нове значення
                settings[key] = JsonSerializer.Serialize(value);

                // Зберігаємо оновлені налаштування у файл
                File.WriteAllText(SettingsFilePath, JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true }));

                Debug.WriteLine($"Налаштування збережено: {key} = {value}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Помилка збереження налаштувань: {ex.Message}");
            }
        }

        // Завантажуємо налаштування за ключем
        public static T? LoadSetting<T>(string key, T? defaultValue = default)
        {
            try
            {
                // Завантажуємо всі налаштування
                var settings = LoadSettings();

                // Якщо ключ існує, десеріалізуємо значення
                if (settings.TryGetValue(key, out var value))
                {
                    Debug.WriteLine($"Завантажено налаштування: {key} = {value}");
                    return JsonSerializer.Deserialize<T>(value);
                }

                Debug.WriteLine($"Налаштування для ключа '{key}' не знайдено. Використовується значення за замовчуванням: {defaultValue}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Помилка завантаження налаштувань: {ex.Message}");
            }

            // Повертаємо значення за замовчуванням, якщо щось пішло не так
            return defaultValue;
        }

        // Завантажуємо всі налаштування з файлу
        private static Dictionary<string, string> LoadSettings()
        {
            try
            {
                // Якщо файл існує, завантажуємо його
                if (File.Exists(SettingsFilePath))
                {
                    var json = File.ReadAllText(SettingsFilePath);
                    return JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
                }

                Debug.WriteLine("Файл налаштувань не знайдено. Використовується порожній словник.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Помилка завантаження налаштувань: {ex.Message}");
            }

            // Якщо файл відсутній або сталася помилка, повертаємо порожній словник
            return new Dictionary<string, string>();
        }
    }
}