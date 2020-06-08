using System.Windows;

namespace Notepad
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
       private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var wnd = new MainWindow();

            if (e.Args.Length == 1) wnd.Open(e.Args[0]);
            
            wnd.Show();
        }
    }
}
