using NotesApp.Services;
using NotesApp.ViewModels;

namespace NotesApp
{
    public partial class MainPage : ContentPage
    {
        public NotesViewModel ViewModel { get; set; }
        private readonly IThemeService _themeService;
        private readonly ILocalizationService _localizationService;

        public MainPage(NotesViewModel viewModel, IThemeService themeService, ILocalizationService localizationService)
        {
            InitializeComponent();

            // Інжектуємо ViewModel та сервіси через конструктор
            ViewModel = viewModel;
            _themeService = themeService;
            _localizationService = localizationService;

            BindingContext = ViewModel;

            // Підписуємось на події
            _localizationService.LanguageChanged += OnLanguageChanged;
            _themeService.ThemeChanged += OnThemeChanged;

            // Оновлюємо UI
            UpdateLanguageButton();
            UpdateThemeButton();

            // Встановлюємо тему за замовчуванням
            if (Application.Current != null)
            {
                Application.Current.UserAppTheme = _themeService.LoadTheme();
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
            LanguageSwitchButton.Text = _localizationService.GetFlag();
        }

        private void UpdateThemeButton()
        {
            ThemeSwitchButton.Text = _themeService.GetThemeIcon();
        }

        private void OnThemeSwitchClicked(object sender, EventArgs e)
        {
            // Зміна теми через ThemeService
            _themeService.ToggleTheme();
        }

        private void OnLanguageSwitchClicked(object sender, EventArgs e)
        {
            // Зміна мови через LocalizationService
            _localizationService.SwitchLanguage();
        }

        private void UpdateTitle()
        {
            Title = ViewModel.Title;
        }

        protected override void OnDisappearing()
        {
            // Відписуємося від подій при закритті сторінки
            _localizationService.LanguageChanged -= OnLanguageChanged;
            _themeService.ThemeChanged -= OnThemeChanged;
            base.OnDisappearing();
        }
    }
}