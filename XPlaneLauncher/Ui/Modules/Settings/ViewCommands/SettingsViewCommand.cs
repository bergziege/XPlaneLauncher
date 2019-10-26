using XPlaneLauncher.Ui.Modules.Settings.ViewModels;
using XPlaneLauncher.Ui.Modules.Settings.ViewModels.Runtime;
using XPlaneLauncher.Ui.Modules.Settings.Views;

namespace XPlaneLauncher.Ui.Modules.Settings.ViewCommands {
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