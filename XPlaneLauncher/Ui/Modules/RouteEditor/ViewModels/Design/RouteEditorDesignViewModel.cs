using Prism.Commands;
using XPlaneLauncher.Domain;
using XPlaneLauncher.Model;

namespace XPlaneLauncher.Ui.Modules.RouteEditor.ViewModels.Design {
    public class RouteEditorDesignViewModel : IRouteEditorViewModel {
        public Aircraft Aircraft { get; } = new Aircraft(new AircraftInformation(), new AircraftLauncherInformation());

        public DelegateCommand LeaveEditorCommand { get; } = new DelegateCommand(()=>{});
        public RoutePoint SelectedRoutePoint { get; set; }
        public DelegateCommand DeleteSelectedRoutePointCommand { get; } = new DelegateCommand(()=>{});
    }
}