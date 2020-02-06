using System;
using XPlaneLauncher.Domain;

namespace XPlaneLauncher.Ui.Modules.Logbook.Manual.NavigationCommands.Params {
    public class ManualEntryParameters {
        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public ManualEntryParameters(Guid aircraftId, LogbookEntry logbookEntry) {
            AircraftId = aircraftId;
            LogbookEntry = logbookEntry;
        }

        public Guid AircraftId { get; }
        public LogbookEntry LogbookEntry { get; }
    }
}