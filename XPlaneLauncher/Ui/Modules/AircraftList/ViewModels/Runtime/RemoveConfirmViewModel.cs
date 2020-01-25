using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;

namespace XPlaneLauncher.Ui.Modules.AircraftList.ViewModels.Runtime {
    public class RemoveConfirmViewModel : BindableBase, IRemoveConfirmViewModel {
        public string AircraftLivery { get; }
        public string AircraftName { get; }
    }
}