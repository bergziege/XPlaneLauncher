using System;
using XPlaneLauncher.Model;

namespace XPlaneLauncher.Ui.Modules.RouteEditor.Events {
    public class RoutePointAddedEvent {
        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public RoutePointAddedEvent(Guid aircraftId, RoutePoint addedRoutePoint) {
            AircraftId = aircraftId;
            AddedRoutePoint = addedRoutePoint;
        }

        public RoutePoint AddedRoutePoint { get; }

        public Guid AircraftId { get; }
    }
}