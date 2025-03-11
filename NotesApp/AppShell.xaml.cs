namespace NotesApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            FlyoutBehavior = FlyoutBehavior.Disabled;

            // Реєстрація маршрутів
            Routing.RegisterRoute("main", typeof(MainPage));
        }
    }
}