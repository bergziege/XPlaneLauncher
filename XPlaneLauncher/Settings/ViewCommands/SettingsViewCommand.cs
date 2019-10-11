using MaterialDesignThemes.Wpf;
using XPlaneLauncher.Settings.ViewModels;

namespace XPlaneLauncher.Settings.ViewCommands {
    public class SettingsViewCommand {
        public void Execute() {
            var view = new SettingsView();
            ISettingsViewModel vm = new SettingsViewModel();
            vm.RequestCloseOnCancel += delegate { view.Close(); };
            vm.RequestCloseOnFinish += delegate { view.Close(); };
            view.DataContext = vm;
            view.ShowDialog();
        }
    }
}