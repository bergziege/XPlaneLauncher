using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;
using XPlaneLauncher.Model;
using XPlaneLauncher.Ui.Modules.AircraftList.ViewModels.Runtime;
using XPlaneLauncher.Ui.Modules.AircraftList.Views;

namespace XPlaneLauncher.Ui.Modules.AircraftList.DialogCommands {
    public class ShowRemoveConfirmationDialogCommand {
        public async Task<bool> Execute(Aircraft aircraft) {
            RemoveConfirmView view = new RemoveConfirmView();
            RemoveConfirmViewModel vm = new RemoveConfirmViewModel(aircraft);
            view.DataContext = vm;
            return (bool)await DialogHost.Show(view);
        }
    }
}