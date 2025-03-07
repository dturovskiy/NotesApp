using NotesApp.Services;

namespace NotesApp
{
    public partial class MainPage : ContentPage
    {
        public NotesViewModel ViewModel { get; set; }

        public MainPage()
        {
            InitializeComponent();

            // Створюємо екземпляр ViewModel
            ViewModel = new NotesViewModel();

            // Встановлюємо BindingContext
            BindingContext = ViewModel;

            LocalizationService.LanguageChanged += OnLanguageChanged;
            ThemeService.ThemeChanged += OnThemeChanged;

            UpdateLanguageButton();
            UpdateThemeButton();

            if (Application.Current != null)
            {
                Application.Current.UserAppTheme = ThemeService.LoadTheme();
            }
        }

        private void OnThemeChanged()
        {
            // Оновлення кнопки теми
            UpdateThemeButton();
        }

        private void OnLanguageChanged()
        {
            // Оновлення локалізованих текстів
            ViewModel.UpdateLocalizedTexts();

            // Оновлення дня тижня при зміні мови
            ViewModel.UpdateDaysOfWeek();

            // Оновлення заголовка
            UpdateTitle();

            // Оновлення прапорця
            UpdateLanguageButton();
        }

        private void UpdateLanguageButton()
        {
            LanguageSwitchButton.Text = LocalizationService.GetFlag();
        }

        private void UpdateThemeButton()
        {
            ThemeSwitchButton.Text = ThemeService.GetThemeIcon();
        }

        private void OnThemeSwitchClicked(object sender, EventArgs e)
        {
            // Зміна теми через ThemeService
            ThemeService.ToggleTheme();
        }

        private void OnLanguageSwitchClicked(object sender, EventArgs e)
        {
            // Зміна мови
            LocalizationService.SwitchLanguage();
        }

        private void UpdateTitle()
        {
            Title = ViewModel.Title;
        }

        protected override void OnDisappearing()
        {
            // Відписуємося від подій при закритті сторінки
            LocalizationService.LanguageChanged -= OnLanguageChanged;
            ThemeService.ThemeChanged -= OnThemeChanged;
            base.OnDisappearing();
        }
    }
}