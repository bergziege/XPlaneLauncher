using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MapControl;
using XPlaneLauncher.Domain;
using XPlaneLauncher.Model;

namespace XPlaneLauncher.Services {
    public interface ILogbookService {
        void CreateManualEntry(
            Guid aircraftId, DateTime startDateTime, DateTime endDateTime, TimeSpan duration, IList<Location> track, double distanceNauticalMiles,
            string notes);

        void DeleteEntry(Guid aircraftId, LogbookEntry entry);

        Task<IList<LogbookEntry>> GetEntriesWithoutTrackAsync(Aircraft aircraft);
    }
}