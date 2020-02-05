using System;
using System.Collections.Generic;
using MapControl;
using Newtonsoft.Json;

namespace XPlaneLauncher.Domain {
    public class LogbookEntry {
        [JsonProperty("Notes")]
        private string _notes;

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public LogbookEntry(
            LogbookEntryType type, DateTime startDateTime, DateTime endDateTime, TimeSpan duration, IList<Location> track,
            double distanceNauticalMiles) {
            Duration = duration;
            DistanceNauticalMiles = distanceNauticalMiles;
            Type = type;
            StartDateTime = startDateTime;
            EndDateTime = endDateTime;
            Track = track;
        }

        public double DistanceNauticalMiles { get; }

        public TimeSpan Duration { get; }

        public DateTime EndDateTime { get; }

        [JsonIgnore]
        public string Notes {
            get { return _notes; }
            private set { _notes = value; }
        }

        public DateTime StartDateTime { get; }

        [JsonIgnore]
        public IList<Location> Track { get; }

        public LogbookEntryType Type { get; }

        public void Update(string notes) {
            Notes = notes;
        }
    }
}