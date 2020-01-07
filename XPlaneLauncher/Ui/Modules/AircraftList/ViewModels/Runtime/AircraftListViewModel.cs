using Prism.Commands;
using Prism.Mvvm;
using XPlaneLauncher.Services;

namespace XPlaneLauncher.Ui.Modules.AircraftList.ViewModels.Runtime {
    public class AircraftListViewModel : BindableBase, IAircraftListViewModel {
        private readonly IAircraftService _aircraftService;
        private DelegateCommand _reloadCommand;

        public AircraftListViewModel(IAircraftService aircraftService) {
            _aircraftService = aircraftService;
        }

        public DelegateCommand ReloadCommand {
            get { return _reloadCommand ?? (_reloadCommand = new DelegateCommand(Reload, CanReload)); }
        }

        private bool CanReload() {
            return true;
        }

        private async void Reload() {
            await _aircraftService.ReloadAsync();
        }
    }
}