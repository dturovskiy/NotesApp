using NotesApp.Resources.Localization;
using System.ComponentModel;

namespace NotesApp
{
    public class Note : INotifyPropertyChanged
    {
        private string _dayOfWeek;
        private string _date;
        private string _topic;

        public string EditButtonText => Localization.Edit;
        public string DeleteButtonText => Localization.Delete;
        public string TopicLabel => Localization.Topic;

        public string DayOfWeek
        {
            get => _dayOfWeek;
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
            get => _date;
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
            get => _topic;
            set
            {
                if (_topic != value)
                {
                    _topic = value;
                    OnPropertyChanged(nameof(Topic));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Note()
        {
            DayOfWeek = string.Empty;
            Date = string.Empty;
            Topic = string.Empty;
        }

        public void UpdateLocalizedTexts()
        {
            OnPropertyChanged(nameof(EditButtonText));
            OnPropertyChanged(nameof(DeleteButtonText));
            OnPropertyChanged(nameof(TopicLabel));
        }
    }
}
