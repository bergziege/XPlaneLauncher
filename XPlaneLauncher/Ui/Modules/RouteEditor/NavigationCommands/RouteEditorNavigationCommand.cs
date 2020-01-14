using System;
using Prism.Regions;
using XPlaneLauncher.Model;
using XPlaneLauncher.Ui.Modules.RouteEditor.NavigationCommands.Parameters;
using XPlaneLauncher.Ui.Modules.RouteEditor.Views;
using XPlaneLauncher.Ui.Shell;

namespace XPlaneLauncher.Ui.Modules.RouteEditor.NavigationCommands {
    public class RouteEditorNavigationCommand {
        public const string RouteEditorNavParamKey = "RouteEditorNavParamKey";
        private readonly IRegionManager _regionManager;

        public RouteEditorNavigationCommand(IRegionManager regionManager) {
            _regionManager = regionManager;
        }

        public void Execute(Aircraft aircraft) {
            NavigationParameters navParam = new NavigationParameters();
            navParam.Add(RouteEditorNavParamKey, new RouteEditorNavigationParameters(aircraft));
            _regionManager.RequestNavigate(RegionNames.AppRegion, new Uri(nameof(RouteEditorView), UriKind.Relative), navParam);
        }
    }
}