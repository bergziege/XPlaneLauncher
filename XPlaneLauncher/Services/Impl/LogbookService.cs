using System;
using System.Collections.Generic;
using MapControl;
using XPlaneLauncher.Domain;

namespace XPlaneLauncher.Services.Impl {
    public class LogbookService : ILogbookService {
        public void CreateManualEntry(
            DateTime startDateTime, DateTime endDateTime, TimeSpan duration, IList<Location> track, double distanceNauticalMiles, string notes) {
            LogbookEntry newEntry = new LogbookEntry(LogbookEntryType.Manual, startDateTime, endDateTime,duration,track,distanceNauticalMiles);
            newEntry.Update(notes);

            /* Persist entry to json*/

            /* Persist track as GeoJSON */
        }
    }
}