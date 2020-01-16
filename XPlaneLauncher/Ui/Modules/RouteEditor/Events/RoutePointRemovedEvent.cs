using System;

namespace XPlaneLauncher.Ui.Modules.RouteEditor.Events {
    public class RoutePointRemovedEvent {
        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public RoutePointRemovedEvent(Guid aircraftId, Guid routePointId) {
            AircraftId = aircraftId;
            RoutePointId = routePointId;
        }

        public Guid AircraftId { get; }
        public Guid RoutePointId { get; }
    }
}