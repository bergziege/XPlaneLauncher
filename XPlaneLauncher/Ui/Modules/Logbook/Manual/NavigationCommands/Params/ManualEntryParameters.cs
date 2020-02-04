using System;

namespace XPlaneLauncher.Ui.Modules.Logbook.Manual.NavigationCommands.Params {
    public class ManualEntryParameters {
        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public ManualEntryParameters(Guid aircraftId) {
            AircraftId = aircraftId;
        }

        public Guid AircraftId { get; }
    }
}