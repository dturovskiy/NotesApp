using NotesApp.Services;
using NotesApp.ViewModels;

namespace NotesApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // Реєстрація сервісів
            builder.Services.AddSingleton<ILocalizationService, LocalizationService>(); // Додаємо LocalizationService
            builder.Services.AddSingleton<IThemeService, ThemeService>();
            builder.Services.AddTransient<NotesViewModel>();

            // Реєстрація сторінок
            builder.Services.AddTransient<MainPage>();

            // Налаштування Splash Screen для Android
#if ANDROID
            builder.ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler<Shell, Microsoft.Maui.Controls.Handlers.Compatibility.ShellRenderer>();
            });
#endif

            return builder.Build();
        }
    }
}