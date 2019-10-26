using System.Windows;
using XPlaneLauncher.Ui.Shell.ViewModels.Runtime;
using XPlaneLauncher.Ui.Shell.Views;

namespace XPlaneLauncher {
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App {
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);
            MainWindow = new MainWindow();
            MainWindow.DataContext = new MainViewModel();
            MainWindow.Show();
        }
    }
}