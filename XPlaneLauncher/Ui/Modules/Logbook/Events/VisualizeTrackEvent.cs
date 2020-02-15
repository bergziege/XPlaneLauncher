using System.Collections.Generic;
using XPlaneLauncher.Model;

namespace XPlaneLauncher.Ui.Modules.Logbook.Events {
    public class VisualizeTrackEvent {
        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public VisualizeTrackEvent(IList<LogbookLocation> track) {
            Track = track;
        }

        public IList<LogbookLocation> Track { get; }
    }
}