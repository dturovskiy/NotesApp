using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;

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
        }

        private async Task OnAddDayButtonClicked()
        {
            Console.WriteLine("Add button clicked");
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



        // Редагувати нотатку
        private async Task OnEditButtonClicked(Note note)
        {
            if (note != null)
            {
                // Показуємо повідомлення про редагування
                await DisplayAlert("Редагування", "Тут повинна бути функція редагування нотатки", "ОК");
            }
        }

        // Видалити нотатку
        private async Task OnDeleteButtonClicked(Note note)
        {
            if (note != null)
            {
                bool confirm = await DisplayAlert("Підтвердження", "Ви точно хочете видалити цю нотатку?", "Так", "Ні");
                if (!confirm) return;

                Notes.Remove(note);
                await NoteService.SaveNotesAsync(Notes);
            }
        }

        private async Task LoadNotesAsync()
        {
            Notes = new ObservableCollection<Note>(await NoteService.LoadNotesAsync());
            NotesCollectionView.ItemsSource = Notes;
        }
    }
}