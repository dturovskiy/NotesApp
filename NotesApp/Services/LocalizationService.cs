using System.Diagnostics;
using System.Globalization;

namespace NotesApp.Services
{
    public static class LocalizationService
    {
        private static readonly List<CultureInfo> AvailableCultures = new()
        {
            new CultureInfo("uk"),
            new CultureInfo("en"),
            new CultureInfo("fr")
        };

        private static readonly object _lock = new();
        private static CultureInfo _currentCulture;

        static LocalizationService()
        {
            _currentCulture = LoadSavedCulture();
            Thread.CurrentThread.CurrentCulture = _currentCulture;
            Thread.CurrentThread.CurrentUICulture = _currentCulture;
        }

        public static CultureInfo CurrentCulture
        {
            get => _currentCulture;
            set
            {
                lock (_lock)
                {
                    if (_currentCulture != value)
                    {
                        _currentCulture = value;
                        Thread.CurrentThread.CurrentCulture = value;
                        Thread.CurrentThread.CurrentUICulture = value;
                        SaveCulture(value);
                        OnLanguageChanged();
                    }
                }
            }
        }

        public static event Action? LanguageChanged;

        private static void OnLanguageChanged()
        {
            LanguageChanged?.Invoke();
        }

        public static void SwitchLanguage()
        {
            int currentIndex = AvailableCultures.IndexOf(_currentCulture);
            int nextIndex = (currentIndex + 1) % AvailableCultures.Count;
            CurrentCulture = AvailableCultures[nextIndex];
        }

        public static void SetLanguage(string languageCode)
        {
            var culture = new CultureInfo(languageCode);
            if (AvailableCultures.Contains(culture))
            {
                CurrentCulture = culture;
            }
            else
            {
                Debug.WriteLine($"Мова '{languageCode}' не підтримується.");
            }
        }

        public static string GetFlag()
        {
            return _currentCulture.Name switch
            {
                "uk" => "🇺🇦", // Прапор України
                "en" => "🇬🇧", // Прапор Великобританії
                "fr" => "🇫🇷", // Прапор Франції
                _ => "🌐"       // Прапор за замовчуванням (якщо мова не визначена)
            };
        }

        private static void SaveCulture(CultureInfo culture)
        {
            Debug.WriteLine($"Збереження мови: {culture.Name}");
            SettingsService.SaveSetting("Language", culture.Name);  // Використовуємо "Language" як ключ
        }

        private static CultureInfo LoadSavedCulture()
        {
            // Пробуємо завантажити збережену мову
            string? savedCulture = SettingsService.LoadSetting<string>("Language");

            if (!string.IsNullOrEmpty(savedCulture))
            {
                try
                {
                    var culture = new CultureInfo(savedCulture);
                    if (AvailableCultures.Contains(culture))
                    {
                        Debug.WriteLine($"Завантажена мова: {culture.Name}");
                        return culture;
                    }
                }
                catch (CultureNotFoundException)
                {
                    Debug.WriteLine($"Мова '{savedCulture}' не знайдена.");
                }
            }

            // Якщо збереженої мови немає або вона не підтримується, використовуємо мову пристрою
            string systemCulture = CultureInfo.CurrentUICulture.Name;
            var systemCultureInfo = new CultureInfo(systemCulture);

            if (AvailableCultures.Contains(systemCultureInfo))
            {
                Debug.WriteLine($"Використовується мова пристрою: {systemCultureInfo.Name}");
                return systemCultureInfo;
            }

            // Якщо мова пристрою не підтримується, повертаємо англійську
            Debug.WriteLine("Мова пристрою не підтримується, використовується англійська (en)");
            return new CultureInfo("en");
        }
    }
}