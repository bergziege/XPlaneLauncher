using Prism.Mvvm;
using XPlaneLauncher.Model;

namespace XPlaneLauncher.Ui.Modules.AircraftList.ViewModels.Runtime {
    public class RemoveConfirmViewModel : BindableBase, IRemoveConfirmViewModel {
        public RemoveConfirmViewModel(Aircraft aircraft) {
            AircraftLivery = aircraft.Livery;
            AircraftName = aircraft.Name;
        }

        public string AircraftLivery { get; }
        public string AircraftName { get; }
    }
}