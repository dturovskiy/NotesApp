using System.Diagnostics;

namespace NotesApp.Services
{
    public class ThemeService : IThemeService
    {
        private const string ThemeKey = "AppTheme";

        // Подія для сповіщення про зміну теми
        public event Action? ThemeChanged;

        // Зміна теми
        public void ToggleTheme()
        {
            if (Application.Current is null) return;

            // Змінюємо тему на протилежну
            Application.Current.UserAppTheme = Application.Current.UserAppTheme == AppTheme.Light ? AppTheme.Dark : AppTheme.Light;

            // Зберігаємо вибір теми
            SaveTheme(Application.Current.UserAppTheme);

            // Сповіщаємо про зміну теми
            OnThemeChanged();
        }

        // Збереження теми
        private static void SaveTheme(AppTheme theme)
        {
            try
            {
                Debug.WriteLine($"Збереження теми: {theme}");
                SettingsService.SaveSetting(ThemeKey, theme.ToString());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Помилка збереження теми: {ex.Message}");
            }
        }

        // Завантаження теми
        public AppTheme LoadTheme()
        {
            try
            {
                string? savedTheme = SettingsService.LoadSetting<string>(ThemeKey);

                if (!string.IsNullOrEmpty(savedTheme) && Enum.TryParse(savedTheme, out AppTheme theme))
                {
                    Debug.WriteLine($"Завантажена тема: {theme}");
                    return theme;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Помилка завантаження теми: {ex.Message}");
            }

            // Повертаємо тему за замовчуванням (світла)
            Debug.WriteLine("Використовується тема за замовчуванням: Light");
            return AppTheme.Light;
        }

        // Оновлення іконки теми
        public string GetThemeIcon()
        {
            var currentTheme = LoadTheme();
            return currentTheme == AppTheme.Dark ? "☀️" : "🌙";
        }

        // Сповіщення про зміну теми
        private void OnThemeChanged()
        {
            ThemeChanged?.Invoke();
        }
    }
}