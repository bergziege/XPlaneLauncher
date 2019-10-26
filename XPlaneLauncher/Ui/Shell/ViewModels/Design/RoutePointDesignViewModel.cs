using MapControl;
using Prism.Mvvm;

namespace XPlaneLauncher.Ui.Shell.ViewModels.Design {
    public class RoutePointDesignViewModel : BindableBase, IRoutePointViewModel {
        public bool IsSelected { get; set; } = true;

        public Location Location { get; } = new Location(51, 13);
        public void Deselect() {
            throw new System.NotImplementedException();
        }

        public void Select() {
            throw new System.NotImplementedException();
        }
    }
}