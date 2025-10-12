namespace Biblioteca;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute("AltasLibros", typeof(AltasLibros));
        Routing.RegisterRoute("ConsultasLibros", typeof(ConsultasLibros));
    }
}
