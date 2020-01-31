using System;
using Prism.Regions;
using XPlaneLauncher.Ui.Modules.Logbook.Manual.Views;
using XPlaneLauncher.Ui.Shell;

namespace XPlaneLauncher.Ui.Modules.Logbook.Manual.NavigationCommands {
    public class ShowManualEntryCommand {
        private readonly IRegionManager _regionManager;

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public ShowManualEntryCommand(IRegionManager regionManager) {
            _regionManager = regionManager;
        }

        public void Execute() {
            _regionManager.RequestNavigate(RegionNames.AppRegion, new Uri(nameof(ManualLogEntryView), UriKind.Relative));
        }
    }
}