using Prism.Commands;

namespace XPlaneLauncher.Ui.Modules.AircraftList.ViewModels.Design {
    public class AircraftListDesignViewModel : IAircraftListViewModel {
        public DelegateCommand ReloadCommand { get; } = new DelegateCommand(()=>{});
    }
}