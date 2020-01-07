using Prism.Commands;

namespace XPlaneLauncher.Ui.Modules.AircraftList.ViewModels {
    public interface IAircraftListViewModel {
        DelegateCommand ReloadCommand { get; }
    }
}