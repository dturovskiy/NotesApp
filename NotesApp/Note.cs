namespace NotesApp
{
    public class Note : ObservableObject
    {
        private string? _dayOfWeek;
        private string? _date;
        private string? _topic;

        public string DayOfWeek
        {
            get => _dayOfWeek ?? string.Empty; // Повертаємо порожній рядок, якщо значення null
            set
            {
                if (_dayOfWeek != value)
                {
                    _dayOfWeek = value;
                    OnPropertyChanged(nameof(DayOfWeek));
                }
            }
        }

        public string Date
        {
            get => _date ?? string.Empty; // Повертаємо порожній рядок, якщо значення null
            set
            {
                if (_date != value)
                {
                    _date = value;
                    OnPropertyChanged(nameof(Date));
                }
            }
        }

        public string Topic
        {
            get => _topic ?? string.Empty; // Повертаємо порожній рядок, якщо значення null
            set
            {
                if (_topic != value)
                {
                    _topic = value;
                    OnPropertyChanged(nameof(Topic));
                }
            }
        }
    }
}
