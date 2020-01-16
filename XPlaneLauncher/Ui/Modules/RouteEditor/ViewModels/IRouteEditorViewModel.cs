using Prism.Commands;
using XPlaneLauncher.Model;

namespace XPlaneLauncher.Ui.Modules.RouteEditor.ViewModels {
    public interface IRouteEditorViewModel {
        Aircraft Aircraft { get; }
        DelegateCommand LeaveEditorCommand { get; }
        RoutePoint SelectedRoutePoint { get; set; }
        DelegateCommand DeleteSelectedRoutePointCommand { get; }
    }
}