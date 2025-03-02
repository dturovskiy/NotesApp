using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using System.Threading;
using NotesApp.Resources.Localization;
using System.Diagnostics;

namespace NotesApp
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Note> Notes { get; set; } = new ObservableCollection<Note>();

        // Команди
        public ICommand AddDayCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public MainPage()
        {
            InitializeComponent();

            // Ініціалізація команд
            AddDayCommand = new Command(async () => await OnAddDayButtonClicked());
            EditCommand = new Command<Note>(async (note) => await OnEditButtonClicked(note));
            DeleteCommand = new Command<Note>(async (note) => await OnDeleteButtonClicked(note));

            // Встановлення BindingContext до ініціалізації Notes
            BindingContext = this;
            // Завантажуємо нотатки асинхронно
            _ = LoadNotesAsync();

            UpdateThemeButton();
        }

        private void UpdateThemeButton()
        {
            if (Application.Current?.RequestedTheme == AppTheme.Dark)
            {
                ThemeSwitchButton.Text = "☀️"; // Темна тема → показуємо Сонце
            }
            else
            {
                ThemeSwitchButton.Text = "🌙"; // Світла тема → показуємо Місяць
            }
        }

        private async Task OnAddDayButtonClicked()
        {
            var today = DateTime.Now;
            var newNote = new Note
            {
                DayOfWeek = today.ToString("dddd").ToUpper(),
                Date = today.ToString("dd/MM/yyyy"),
                Topic = "Введіть тему"
            };

            await NoteService.AddNoteAsync(newNote);
            Notes.Add(newNote);
        }

        private async Task OnEditButtonClicked(Note note)
        {
            if (note is null) return;

            string newTopic = await DisplayPromptAsync("Редагування", "Введіть нову тему:", initialValue: note.Topic);
            if (!string.IsNullOrWhiteSpace(newTopic))
            {
                note.Topic = newTopic;
                await NoteService.SaveNotesAsync(Notes);
            }
        }

        private async Task OnDeleteButtonClicked(Note note)
        {
            if (note is null) return;

            bool confirm = await DisplayAlert("Підтвердження", "Ви точно хочете видалити цю нотатку?", "Так", "Ні");
            if (!confirm) return;

            if (Notes.Remove(note))
            {
                await NoteService.SaveNotesAsync(Notes);
            }
        }

        private async Task LoadNotesAsync()
        {
            var loadedNotes = await NoteService.LoadNotesAsync();
            if (loadedNotes is not null)
            {
                Notes = new ObservableCollection<Note>(loadedNotes);
                OnPropertyChanged(nameof(Notes)); // Оповіщаємо UI про зміну колекції
            }
        }

        private void OnThemeSwitchClicked(object sender, EventArgs e)
        {
            if (Application.Current is null) return;

            Application.Current.UserAppTheme = Application.Current.UserAppTheme == AppTheme.Light ? AppTheme.Dark : AppTheme.Light;
            UpdateThemeButton();
        }

        private async void OnLanguageSwitchClicked(object sender, EventArgs e)
        {
            // Визначаємо поточну мову пристрою
            string currentLanguage = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;

            // Показуємо діалогове вікно, що мова була змінена
            await DisplayAlert("Зміна мови", "Мова була змінена.", "OK");

            // Змінюємо текст кнопки в залежності від поточного стану
            if (LanguageSwitchButton.Text == "UK")
            {
                // Якщо кнопка показує "UA", змінюємо на англійську
                LanguageSwitchButton.Text = "EN";

                // Змінюємо мову на англійську
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            }
            else if (LanguageSwitchButton.Text == "EN")
            {
                // Якщо кнопка показує "EN", змінюємо на французьку
                LanguageSwitchButton.Text = "FR";

                // Змінюємо мову на французьку
                Thread.CurrentThread.CurrentCulture = new CultureInfo("fr");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr");
            }
            else
            {
                // Якщо кнопка показує "FR", змінюємо на українську
                LanguageSwitchButton.Text = "UK";

                // Змінюємо мову на українську
                Thread.CurrentThread.CurrentCulture = new CultureInfo("uk");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("uk");
            }

            UpdateText();
            UpdateLocalizedTexts();
        }

        private void UpdateText()
        {
            // Оновлення тексту кнопки додавання нового дня
            AddButton.Text = Localization.AddDay;
            Title = Localization.Title;

            foreach (var note in Notes)
            {
                note.DayOfWeek = DateTime.Now.ToString("dddd", CultureInfo.CurrentCulture).ToUpper();
                note.Date = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.CurrentCulture);
            }
        }

        private void UpdateLocalizedTexts()
        {
            foreach (var note in Notes)
            {
                note.UpdateLocalizedTexts();
            }
        }
    }
}