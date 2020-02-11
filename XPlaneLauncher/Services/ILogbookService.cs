using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MapControl;
using XPlaneLauncher.Domain;
using XPlaneLauncher.Model;

namespace XPlaneLauncher.Services {
    public interface ILogbookService {
        void CreateAcmiZipEntry(
            FileInfo importFile,
            Guid aircraftId, DateTime startDateTime, DateTime endDateTime, TimeSpan duration, IList<Location> track, double distanceNauticalMiles,
            string notes);

        void CreateManualEntry(
            Guid aircraftId, DateTime startDateTime, DateTime endDateTime, TimeSpan duration, IList<Location> track, double distanceNauticalMiles,
            string notes);

        void DeleteEntry(Guid aircraftId, LogbookEntry entry);

        LogbookEntry ExpandTrack(Guid aircraftId, LogbookEntry logbookEntry);

        Task<IList<LogbookEntry>> GetEntriesWithoutTrackAsync(Aircraft aircraft);

        LogbookEntry GetEntryFromAcmiFile(FileInfo acmiFile);

        void UpdateAutoEntry(
            LogbookEntry oldEntry, Guid aircraftId, DateTime startDateTime, DateTime endDateTime, TimeSpan duration, IList<Location> track,
            double distanceNauticalMiles,
            string notes);

        void UpdateManualEntry(
            LogbookEntry oldEntry, Guid aircraftId, DateTime startDateTime, DateTime endDateTime, TimeSpan duration, IList<Location> track,
            double distanceNauticalMiles,
            string notes);
    }
}