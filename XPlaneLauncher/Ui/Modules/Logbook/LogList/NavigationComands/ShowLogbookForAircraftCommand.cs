using System;
using Prism.Regions;
using XPlaneLauncher.Model;
using XPlaneLauncher.Ui.Modules.Logbook.LogList.Views;
using XPlaneLauncher.Ui.Shell;

namespace XPlaneLauncher.Ui.Modules.Logbook.LogList.NavigationComands {
    public class ShowLogbookForAircraftCommand {
        public const string PARAM_KEY = "LogbookNavigationCommandNavParams";
        private readonly IRegionManager _regionManager;

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public ShowLogbookForAircraftCommand(IRegionManager regionManager) {
            _regionManager = regionManager;
        }

        public void Execute(Aircraft aircraft) {
            NavigationParameters parameters = new NavigationParameters();
            parameters.Add(PARAM_KEY, aircraft);
            _regionManager.RequestNavigate(RegionNames.AppRegion, new Uri(nameof(LogListView), UriKind.Relative), parameters);
        }
    }
}