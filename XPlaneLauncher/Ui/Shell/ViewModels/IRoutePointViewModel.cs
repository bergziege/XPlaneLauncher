using MapControl;

namespace XPlaneLauncher.Ui.Shell.ViewModels {
    public interface IRoutePointViewModel {
        bool IsSelected { get; set; }
        Location Location { get; }
        void Deselect();
        void Select();
    }
}