using System.Collections.Generic;
using MapControl;

namespace XPlaneLauncher.Ui.Modules.Logbook.Events {
    public class VisualizeTrackEvent {
        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public VisualizeTrackEvent(IList<Location> track) {
            Track = track;
        }

        public IList<Location> Track { get; }
    }
}