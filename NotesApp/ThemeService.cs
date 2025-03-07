//using NotesApp.Services;

//namespace NotesApp
//{
//    public static class ThemeService
//    {
//        // Зміна теми
//        public static void SwitchTheme()
//        {
//            if (Application.Current is null) return;

//            // Перемикання між світлою та темною темою
//            Application.Current.UserAppTheme = Application.Current.UserAppTheme == AppTheme.Light ? AppTheme.Dark : AppTheme.Light;

//            // Зберігаємо поточну тему в налаштуваннях
//            SaveTheme();
//        }

//        // Оновлення кнопки з темою
//        public static void UpdateThemeButton(Button themeSwitchButton)
//        {
//            if (Application.Current?.RequestedTheme == AppTheme.Dark)
//            {
//                themeSwitchButton.Text = "☀️";
//            }
//            else
//            {
//                themeSwitchButton.Text = "🌙";
//            }
//        }

//        // Завантаження теми з налаштувань
//        public static void LoadSavedTheme()
//        {
//            var savedTheme = SettingsService.LoadSetting("theme", "light");
//            Application.Current.UserAppTheme = savedTheme == "dark" ? AppTheme.Dark : AppTheme.Light;
//        }

//        // Збереження вибраної теми
//        private static void SaveTheme()
//        {
//            var currentTheme = Application.Current?.RequestedTheme == AppTheme.Dark ? "dark" : "light";
//            SettingsService.SaveSetting("theme", currentTheme);
//        }
//    }
//}