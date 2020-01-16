using System;

namespace XPlaneLauncher.Ui.Modules.RouteEditor.Events {
    public class RoutePointRemovedEvent {
        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public RoutePointRemovedEvent(Guid aircraftId) {
            AircraftId = aircraftId;
        }

        public Guid AircraftId { get; }
    }
}