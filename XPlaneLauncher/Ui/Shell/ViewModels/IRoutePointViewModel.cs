using System.ComponentModel;
using MapControl;

namespace XPlaneLauncher.Ui.Shell.ViewModels {
    public interface IRoutePointViewModel : INotifyPropertyChanged {
        bool IsSelected { get; set; }
        Location Location { get; }
        void Deselect();
        void Select();
    }
}