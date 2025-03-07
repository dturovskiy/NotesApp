using NotesApp.Resources.Localization;
using NotesApp.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace NotesApp
{
    public class NotesViewModel : ObservableObject
    {
        private bool _isLoading;

        public ObservableCollection<Note> Notes { get; set; } = new();

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

        public NotesViewModel()
        {
            // Ініціалізація команд
            AddDayCommand = new Command(async () => await OnAddDayButtonClicked());
            EditCommand = new Command<Note>(async (note) => await OnEditButtonClicked(note));
            DeleteCommand = new Command<Note>(async (note) => await OnDeleteButtonClicked(note));

            // Завантаження нотаток
            _ = LoadNotesAsync();

            // Оновлення локалізованих текстів при запуску
            UpdateLocalizedTexts();

            // Підписуємось на зміну мови, щоб UI оновлювався
            LocalizationService.LanguageChanged += UpdateLocalizedTexts;
        }

        private async Task OnAddDayButtonClicked()
        {
            var today = DateTime.Now;
            var newNote = new Note
            {
                Date = today, // Зберігаємо дату як DateTime
                Topic = "Введіть тему"
            };

            // Оновлюємо день тижня та форматовану дату після створення нотатки
            newNote.UpdateDayOfWeek(); // Якщо ви хочете оновити день тижня для нової нотатки

            await NoteService.AddNoteAsync(newNote);
            Notes.Add(newNote);
        }

        private async Task OnEditButtonClicked(Note note)
        {
            if (note is null) return;

            string newTopic = await Shell.Current.DisplayPromptAsync("Редагування", "Введіть нову тему:", initialValue: note.Topic);
            if (!string.IsNullOrWhiteSpace(newTopic))
            {
                note.Topic = newTopic;
                await NoteService.SaveNotesAsync(Notes);
            }
        }

        private async Task OnDeleteButtonClicked(Note note)
        {
            if (note is null) return;

            bool confirm = await Shell.Current.DisplayAlert("Підтвердження", "Ви точно хочете видалити цю нотатку?", "Так", "Ні");
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
                    Notes = new ObservableCollection<Note>(loadedNotes);
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
