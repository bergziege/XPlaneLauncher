using System;
using System.Collections.Generic;
using MapControl;

namespace XPlaneLauncher.Services {
    public interface ILogbookService {
        void CreateManualEntry(
            Guid aircraftId, DateTime startDateTime, DateTime endDateTime, TimeSpan duration, IList<Location> track, double distanceNauticalMiles,
            string notes);
    }
}