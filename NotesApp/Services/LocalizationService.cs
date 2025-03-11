using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace NotesApp.Services
{
    public class LocalizationService : ILocalizationService
    {
        private static readonly List<CultureInfo> AvailableCultures =
        [
            new CultureInfo("uk"),
            new CultureInfo("en"),
            new CultureInfo("fr")
        ];

        private static readonly Lock _lock = new();
        private static CultureInfo? _currentCulture;
        private readonly ResourceManager _resourceManager;

        public LocalizationService()
        {
            // Ініціалізація ResourceManager для роботи з .resx файлами
            _resourceManager = new ResourceManager("NotesApp.Resources.Strings", Assembly.GetExecutingAssembly());
            _currentCulture = LoadSavedCulture();
            ApplyCulture(_currentCulture);
        }

        public CultureInfo CurrentCulture
        {
            get => _currentCulture ?? throw new InvalidOperationException("Current culture is not set.");
            set
            {
                lock (_lock)
                {
                    if (_currentCulture != value)
                    {
                        _currentCulture = value;
                        ApplyCulture(value);
                        SaveCulture(value);
                        OnLanguageChanged();
                    }
                }
            }
        }

        public event Action? LanguageChanged;

        private void OnLanguageChanged()
        {
            LanguageChanged?.Invoke();
        }

        public void SwitchLanguage()
        {
            if (_currentCulture == null)
            {
                Debug.WriteLine("Поточна культура не встановлена.");
                return;
            }

            int currentIndex = AvailableCultures.IndexOf(_currentCulture);
            int nextIndex = (currentIndex + 1) % AvailableCultures.Count;
            CurrentCulture = AvailableCultures[nextIndex];
        }

        public string GetFlag()
        {
            if (_currentCulture == null)
            {
                Debug.WriteLine("Поточна культура не встановлена.");
                return "🌐"; // Повертаємо прапор за замовчуванням
            }

            return _currentCulture.Name switch
            {
                "uk" => "🇺🇦", // Прапор України
                "en" => "🇬🇧", // Прапор Великобританії
                "fr" => "🇫🇷", // Прапор Франції
                _ => "🌐"       // Прапор за замовчуванням (якщо мова не визначена)
            };
        }

        public string GetString(string key)
        {
            if (_currentCulture == null)
            {
                Debug.WriteLine("Поточна культура не встановлена.");
                return key; // Повертаємо ключ, якщо культура не встановлена
            }

            // Отримуємо значення за ключем для поточної культури
            return _resourceManager.GetString(key, _currentCulture) ?? key;
        }

        private static void ApplyCulture(CultureInfo culture)
        {
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        private static void SaveCulture(CultureInfo culture)
        {
            Debug.WriteLine($"Збереження мови: {culture.Name}");
            SettingsService.SaveSetting("Language", culture.Name);
        }

        private static CultureInfo LoadSavedCulture()
        {
            string? savedCulture = SettingsService.LoadSetting<string>("Language");

            if (!string.IsNullOrEmpty(savedCulture))
            {
                try
                {
                    var culture = new CultureInfo(savedCulture);
                    if (GetAvailableCultures().Contains(culture))
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

            string systemCulture = CultureInfo.CurrentUICulture.Name;
            var systemCultureInfo = new CultureInfo(systemCulture);

            if (GetAvailableCultures().Contains(systemCultureInfo))
            {
                Debug.WriteLine($"Використовується мова пристрою: {systemCultureInfo.Name}");
                return systemCultureInfo;
            }

            Debug.WriteLine("Мова пристрою не підтримується, використовується англійська (en)");
            return new CultureInfo("en");
        }

        private static List<CultureInfo> GetAvailableCultures()
        {
            // Отримуємо всі доступні культури з ресурсів
            var availableCultures = new List<CultureInfo>
            {
                // Додаємо культури, які підтримуються
                new("en"), // Англійська
                new("uk"), // Українська
                new("fr") // Французька
            };

            return availableCultures;
        }
    }
}