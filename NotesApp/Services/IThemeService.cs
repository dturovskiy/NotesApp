namespace NotesApp.Services
{
    public interface IThemeService
    {
        event Action? ThemeChanged;
        void ToggleTheme();
        AppTheme LoadTheme();
        string GetThemeIcon();
    }
}