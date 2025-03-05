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

        private static CultureInfo _currentCulture = AvailableCultures[0];

        public static CultureInfo CurrentCulture
        {
            get => _currentCulture;
            set
            {
                if (_currentCulture != value)
                {
                    _currentCulture = value;
                    Thread.CurrentThread.CurrentCulture = value;
                    Thread.CurrentThread.CurrentUICulture = value;
                    OnLanguageChanged();
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
    }
}
