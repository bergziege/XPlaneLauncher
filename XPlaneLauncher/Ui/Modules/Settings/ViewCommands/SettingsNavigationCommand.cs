using System;
using Prism.Regions;
using XPlaneLauncher.Ui.Modules.Settings.ViewModels;
using XPlaneLauncher.Ui.Modules.Settings.ViewModels.Runtime;
using XPlaneLauncher.Ui.Modules.Settings.Views;
using XPlaneLauncher.Ui.Shell;

namespace XPlaneLauncher.Ui.Modules.Settings.ViewCommands {
    public class SettingsNavigationCommand {
        private readonly IRegionManager _regionManager;

        public SettingsNavigationCommand(IRegionManager regionManager) {
            _regionManager = regionManager;
        }

        public void Execute() {
            _regionManager.RequestNavigate(RegionNames.AppRegion, new Uri(nameof(SettingsView), UriKind.Relative));
        }
    }
}