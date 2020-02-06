using System;
using Prism.Regions;
using XPlaneLauncher.Domain;
using XPlaneLauncher.Ui.Modules.Logbook.Manual.NavigationCommands.Params;
using XPlaneLauncher.Ui.Modules.Logbook.Manual.Views;
using XPlaneLauncher.Ui.Shell;

namespace XPlaneLauncher.Ui.Modules.Logbook.Manual.NavigationCommands {
    public class ShowManualEntryCommand {
        public const string NAV_PARAM_KEY = "ShowManualEntryCommandParameters";
        private readonly IRegionManager _regionManager;

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public ShowManualEntryCommand(IRegionManager regionManager) {
            _regionManager = regionManager;
        }

        public void Execute(Guid aircraftId, LogbookEntry logbookEntry) {
            ManualEntryParameters parameters = new ManualEntryParameters(aircraftId, logbookEntry);
            NavigationParameters navParams = new NavigationParameters { { NAV_PARAM_KEY, parameters } };
            _regionManager.RequestNavigate(RegionNames.AppRegion, new Uri(nameof(ManualLogEntryView), UriKind.Relative), navParams);
        }
    }
}