using MapControl;

namespace XPlaneLauncher.Ui.Shell.ViewModels.Design {
    public class RoutePointDesignViewModel : IRoutePointViewModel {
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