using NotesApp.Resources.Localization;
using NotesApp.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace NotesApp.ViewModels
{
    public partial class NotesViewModel : ObservableObject
    {
        private bool _isLoading;
        private readonly ILocalizationService _localizationService;

        public ObservableCollection<Note> Notes { get; set; } = [];

        // Локалізовані тексти
        public string EditButtonText => Localization.Edit;
        public string DeleteButtonText => Localization.Delete;
        public string TopicLabel => Localization.Topic;
        public string AddButtonText => Localization.AddDay;
        public string Title => Localization.Title;

        // Команди
        public ICommand AddDayCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public NotesViewModel(ILocalizationService localizationService)
        {
            _localizationService = localizationService;

            // Ініціалізація команд
            AddDayCommand = new Command(async () => await OnAddDayButtonClicked());
            EditCommand = new Command<Note>(async (note) => await OnEditButtonClicked(note));
            DeleteCommand = new Command<Note>(async (note) => await OnDeleteButtonClicked(note));

            // Завантаження нотаток
            _ = LoadNotesAsync();

            // Оновлення локалізованих текстів при запуску
            UpdateLocalizedTexts();

            // Підписуємось на зміну мови, щоб UI оновлювався
            _localizationService.LanguageChanged += UpdateLocalizedTexts;
        }

        private async Task OnAddDayButtonClicked()
        {
            var today = DateTime.Now;
            var newNote = new Note
            {
                Date = today, // Зберігаємо дату як DateTime
                Topic = Localization.EnterTopic
            };

            // Оновлюємо день тижня та форматовану дату після створення нотатки
            newNote.UpdateDayOfWeek(); // Якщо ви хочете оновити день тижня для нової нотатки

            await NoteService.AddNoteAsync(newNote);
            Notes.Add(newNote);
        }

        private async Task OnEditButtonClicked(Note note)
        {
            if (note is null) return;

            string newTopic = await Shell.Current.DisplayPromptAsync(Localization.Editing, Localization.Enter_new_topic_, initialValue: note.Topic);
            if (!string.IsNullOrWhiteSpace(newTopic))
            {
                note.Topic = newTopic;
                await NoteService.SaveNotesAsync(Notes);
            }
        }

        private async Task OnDeleteButtonClicked(Note note)
        {
            if (note is null) return;

            // Використовуємо локалізований текст
            bool confirm = await Shell.Current.DisplayAlert(
                Localization.Confirmation, // Заголовок
                Localization.DeleteConfirm, // Повідомлення
                Localization.Yes, // Кнопка "Так"
                Localization.No // Кнопка "Ні"
            );

            if (!confirm) return;

            if (Notes.Remove(note))
            {
                await NoteService.SaveNotesAsync(Notes);
            }
        }

        private async Task LoadNotesAsync()
        {
            IsLoading = true; // Показуємо індикатор завантаження

            try
            {
                var loadedNotes = await NoteService.LoadNotesAsync();
                if (loadedNotes is not null)
                {
                    Notes = [.. loadedNotes];
                    UpdateDaysOfWeek();
                    OnPropertyChanged(nameof(Notes));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Помилка завантаження нотаток: {ex.Message}");
            }
            finally
            {
                IsLoading = false; // Ховаємо індикатор завантаження
            }
        }

        public void UpdateDaysOfWeek()
        {
            // Оновлюємо день тижня для кожної нотатки
            foreach (var note in Notes)
            {
                note.UpdateDayOfWeek(); // Викликаємо метод оновлення дня тижня
            }
        }

        public void UpdateLocalizedTexts()
        {
            OnPropertyChanged(nameof(EditButtonText));
            OnPropertyChanged(nameof(DeleteButtonText));
            OnPropertyChanged(nameof(TopicLabel));
            OnPropertyChanged(nameof(AddButtonText));
            OnPropertyChanged(nameof(Title));

            // Оновлюємо дні тижня при зміні мови
            UpdateDaysOfWeek();
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
                OnPropertyChanged(nameof(IsNotLoading)); // Оновлюємо IsNotLoading при зміні IsLoading
            }
        }

        public bool IsNotLoading => !IsLoading;

    }
}