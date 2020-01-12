using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace XPlaneLauncher.Model.Provider {
    public interface IAircraftModelProvider {
        ObservableCollection<Aircraft> Aircrafts { get; }
    }
}