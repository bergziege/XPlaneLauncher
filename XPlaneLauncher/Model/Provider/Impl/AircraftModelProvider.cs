using System.Collections.ObjectModel;

namespace XPlaneLauncher.Model.Provider.Impl {
    public class AircraftModelProvider : IAircraftModelProvider {
        public ObservableCollection<Aircraft> Aircrafts { get; } = new ObservableCollection<Aircraft>();
    }
}