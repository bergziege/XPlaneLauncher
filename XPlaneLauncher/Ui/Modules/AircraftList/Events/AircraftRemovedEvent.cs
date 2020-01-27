using System;

namespace XPlaneLauncher.Ui.Modules.AircraftList.Events {
    public class AircraftRemovedEvent {
        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public AircraftRemovedEvent(Guid aircraftId) {
            AircraftId = aircraftId;
        }

        public Guid AircraftId { get; }
    }
}