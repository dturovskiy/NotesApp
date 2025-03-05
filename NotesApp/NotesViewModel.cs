using NotesApp.Resources.Localization;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace NotesApp
{
    public class NotesViewModel : ObservableObject
    {
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
            var loadedNotes = await NoteService.LoadNotesAsync();
            if (loadedNotes is not null)
            {
                Notes = new ObservableCollection<Note>(loadedNotes);
                OnPropertyChanged(nameof(Notes)); // Оповіщаємо UI про зміну колекції
            }
        }

        public void UpdateLocalizedTexts()
        {
            OnPropertyChanged(nameof(EditButtonText));
            OnPropertyChanged(nameof(DeleteButtonText));
            OnPropertyChanged(nameof(TopicLabel));
            OnPropertyChanged(nameof(AddButtonText));
            OnPropertyChanged(nameof(Title));
        }
    }
}
