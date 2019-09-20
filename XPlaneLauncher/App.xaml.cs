using System.Windows;
using XPlaneLauncher.DesignViewModels;
using XPlaneLauncher.ViewModels;

namespace XPlaneLauncher
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindow = new MainWindow();
            MainWindow.DataContext = new MainViewModel();
            MainWindow.Show();
        }
    }
}
