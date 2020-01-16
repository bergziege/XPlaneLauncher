using System;
using MapControl;

namespace XPlaneLauncher.Dtos {
    public class AircraftRouteOnMap {
        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public AircraftRouteOnMap(Guid aircraftId) {
            AircraftId = aircraftId;
        }

        public Guid AircraftId { get; }

        public LocationCollection Locations { get; set; }
    }
}