using XPlaneLauncher.Model;

namespace XPlaneLauncher.Ui.Modules.RouteEditor.NavigationCommands.Parameters {
    public class RouteEditorNavigationParameters {
        public RouteEditorNavigationParameters(Aircraft aircraft) {
            Aircraft = aircraft;
        }

        public Aircraft Aircraft { get; }
    }
}