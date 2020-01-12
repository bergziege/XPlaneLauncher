using System.Collections.ObjectModel;
using XPlaneLauncher.Model;

namespace XPlaneLauncher.Ui.Modules.Map.ViewModels {
    public interface IMapViewModel {
        ObservableCollection<Aircraft> Aircrafts { get; }
    }
}