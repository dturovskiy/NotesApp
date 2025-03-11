namespace NotesApp.Services
{
    public interface ILocalizationService
    {
        event Action? LanguageChanged;
        void SwitchLanguage();
        string GetFlag();
        string GetString(string key);
    }
}
