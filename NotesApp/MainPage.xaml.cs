using NotesApp.Resources.Localization;
using System.Globalization;

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

        private async void OnLanguageSwitchClicked(object sender, EventArgs e)
        {
            // Зміна мови
            if (LanguageSwitchButton.Text == "UK")
            {
                LanguageSwitchButton.Text = "EN";
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            }
            else if (LanguageSwitchButton.Text == "EN")
            {
                LanguageSwitchButton.Text = "FR";
                Thread.CurrentThread.CurrentCulture = new CultureInfo("fr");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr");
            }
            else
            {
                LanguageSwitchButton.Text = "UK";
                Thread.CurrentThread.CurrentCulture = new CultureInfo("uk");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("uk");
            }

            // Оновлення локалізованих текстів
            ViewModel.UpdateLocalizedTexts();
            UpdateTitle();
            await DisplayAlert("Зміна мови", "Мова була змінена.", "OK");
        }

        private void UpdateTitle()
        {
            Title = Localization.Title;
        }
    }
}