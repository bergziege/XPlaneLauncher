using System.Collections.ObjectModel;
using XPlaneLauncher.Model;
using XPlaneLauncher.Model.Provider;

namespace XPlaneLauncher.Ui.Modules.Map.ViewModels.Runtime {
    public class MapViewModel : IMapViewModel {
        public MapViewModel(IAircraftModelProvider modelProvider) {
            Aircrafts = modelProvider.Aircrafts;
        }

        public ObservableCollection<Aircraft> Aircrafts { get; }
    }
}