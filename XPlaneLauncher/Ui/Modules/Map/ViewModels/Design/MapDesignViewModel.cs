using System.Collections.ObjectModel;
using XPlaneLauncher.Model;

namespace XPlaneLauncher.Ui.Modules.Map.ViewModels.Design {
    public class MapDesignViewModel : IMapViewModel {
        public ObservableCollection<Aircraft> Aircrafts { get; } = new ObservableCollection<Aircraft>();
    }
}