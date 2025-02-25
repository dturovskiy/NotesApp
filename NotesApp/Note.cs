namespace NotesApp
{
    public class Note
    {
        // Статичні поля для імен властивостей (якщо потрібно для прив'язок)
        public static readonly string DayOfWeekPropertyName = nameof(DayOfWeek);
        public static readonly string DatePropertyName = nameof(Date);
        public static readonly string TopicPropertyName = nameof(Topic);

        public string DayOfWeek { get; set; } // День тижня (наприклад, "ПОНЕДІЛОК")
        public string Date { get; set; } // Дата у форматі "день/місяць/рік"
        public string Topic { get; set; } // Тема

        // Конструктор для ініціалізації властивостей
        public Note()
        {
            DayOfWeek = string.Empty;
            Date = string.Empty;
            Topic = string.Empty;
        }
    }
}