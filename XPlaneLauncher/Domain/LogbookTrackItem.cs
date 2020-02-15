using System;

namespace XPlaneLauncher.Domain {
    public class LogbookTrackItem {
        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public LogbookTrackItem(DateTime timestamp) {
            Timestamp = timestamp;
        }

        public double? Alt { get; set; }
        public double? Hdg { get; set; }
        public double? Ias { get; set; }
        public double? Lat { get; set; }
        public double? Lon { get; set; }
        public DateTime Timestamp { get; }
    }
}