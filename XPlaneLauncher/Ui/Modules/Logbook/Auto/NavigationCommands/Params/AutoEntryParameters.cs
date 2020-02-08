using System;
using XPlaneLauncher.Domain;

namespace XPlaneLauncher.Ui.Modules.Logbook.Auto.NavigationCommands.Params {
    public class AutoEntryParameters {
        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public AutoEntryParameters(Guid aircraftId, LogbookEntry logbookEntry) {
            AircraftId = aircraftId;
            LogbookEntry = logbookEntry;
        }

        public Guid AircraftId { get; }
        public LogbookEntry LogbookEntry { get; }
    }
}