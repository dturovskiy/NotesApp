using System.Globalization;

namespace NotesApp
{
    public partial class Note : ObservableObject
    {
        private DateTime _date;
        private string? _topic;

        public DateTime Date
        {
            get => _date;
            set
            {
                if (_date != value)
                {
                    _date = value;
                    OnPropertyChanged(nameof(Date));
                    OnPropertyChanged(nameof(DayOfWeek)); // Оновлюємо DayOfWeek при зміні дати
                    OnPropertyChanged(nameof(FormattedDate));
                }
            }
        }

        public string Topic
        {
            get => _topic ?? string.Empty;
            set
            {
                if (_topic != value)
                {
                    _topic = value;
                    OnPropertyChanged(nameof(Topic));
                }
            }
        }

        // Обчислювана властивість для дня тижня
        public string DayOfWeek => Date.ToString("dddd", CultureInfo.CurrentCulture).ToUpper();
        public string FormattedDate => Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

        // Метод для оновлення дня тижня
        public void UpdateDayOfWeek()
        {
            OnPropertyChanged(nameof(DayOfWeek));
        }

        public void FormateDate()
        {
            OnPropertyChanged(nameof(FormattedDate));
        }
    }
}