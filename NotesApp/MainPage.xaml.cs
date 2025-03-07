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

            UpdateLanguageButton();
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

        private void OnThemeSwitchClicked(object sender, EventArgs e)
        {
            if (Application.Current is null) return;

            Application.Current.UserAppTheme = Application.Current.UserAppTheme == AppTheme.Light ? AppTheme.Dark : AppTheme.Light;
            UpdateThemeButton();
        }

        private void UpdateThemeButton()
        {
            if (Application.Current?.RequestedTheme == AppTheme.Dark)
            {
                ThemeSwitchButton.Text = "☀️";
            }
            else
            {
                ThemeSwitchButton.Text = "🌙";
            }
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
            // Відписуємося від події при закритті сторінки
            LocalizationService.LanguageChanged -= OnLanguageChanged;
            base.OnDisappearing();
        }
    }
}