using MapControl;

namespace XPlaneLauncher.Ui.Modules.Map.Events {
    public class LocationSelectedEvent {
        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public LocationSelectedEvent(Location location) {
            Location = location;
        }

        public Location Location { get; }
    }
}