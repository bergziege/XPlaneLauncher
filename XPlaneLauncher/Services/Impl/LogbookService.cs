using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MapControl;
using XPlaneLauncher.Domain;
using XPlaneLauncher.Model;
using XPlaneLauncher.Model.Provider;
using XPlaneLauncher.Persistence;

namespace XPlaneLauncher.Services.Impl {
    public class LogbookService : ILogbookService {
        private readonly IAircraftModelProvider _aircraftModelProvider;
        private readonly ILogbookEntryDao _logbookEntryDao;
        private readonly ILogbookEntryTrackDao _logbookEntryTrackDao;

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public LogbookService(
            ILogbookEntryDao logbookEntryDao, ILogbookEntryTrackDao logbookEntryTrackDao,
            IAircraftModelProvider aircraftModelProvider) {
            _logbookEntryDao = logbookEntryDao;
            _logbookEntryTrackDao = logbookEntryTrackDao;
            _aircraftModelProvider = aircraftModelProvider;
        }

        public void CreateManualEntry(
            Guid aircraftId, DateTime startDateTime, DateTime endDateTime, TimeSpan duration, IList<Location> track, double distanceNauticalMiles,
            string notes) {
            LogbookEntry newEntry = new LogbookEntry(LogbookEntryType.Manual, startDateTime, endDateTime, duration, track, distanceNauticalMiles);
            newEntry.Update(notes);

            FileInfo logbookEntryFile = GetLogbookEntryFile(aircraftId, startDateTime, "log");
            FileInfo logbookEntryTrackFile = GetLogbookEntryFile(aircraftId, startDateTime, "track");
            /* Persist entry to json*/
            _logbookEntryDao.SaveWithoutTrack(logbookEntryFile, newEntry);

            /* Persist track as GeoJSON */
            _logbookEntryTrackDao.Save(logbookEntryTrackFile, newEntry.Track);
        }

        public void DeleteEntry(Guid aircraftId, LogbookEntry entry) {
            /* Delete log file */
            FileInfo logFile = GetLogbookEntryFile(aircraftId, entry.StartDateTime, "log");
            _logbookEntryDao.Delete(logFile);

            /* Delete track file */
            FileInfo trackFile = GetLogbookEntryFile(aircraftId, entry.StartDateTime, "track");
            _logbookEntryTrackDao.Delete(trackFile);

            if (!logFile.Directory.GetFiles().Any()) {
                logFile.Directory.Delete();
            }
        }

        public LogbookEntry ExpandTrack(Guid aircraftId, LogbookEntry logbookEntry) {
            IList<Location> track = _logbookEntryTrackDao.GetTrack(GetLogbookEntryFile(aircraftId, logbookEntry.StartDateTime, "track"));
            logbookEntry.Update(track);
            return logbookEntry;
        }

        public async Task<IList<LogbookEntry>> GetEntriesWithoutTrackAsync(Aircraft aircraft) {
            DirectoryInfo logbookDirectory = GetLogbookDirectoryByLauncherInfoFile(aircraft.LauncherInfoFile);
            return await _logbookEntryDao.GetEntriesAsync(logbookDirectory);
        }

        public void UpdateManualEntry(
            LogbookEntry oldEntry, Guid aircraftId, DateTime startDateTime, DateTime endDateTime, TimeSpan duration, IList<Location> track,
            double distanceNauticalMiles, string notes) {
            DeleteEntry(aircraftId, oldEntry);
            CreateManualEntry(aircraftId, startDateTime, endDateTime, duration, track, distanceNauticalMiles, notes);
        }

        private static DirectoryInfo GetLogbookDirectoryByLauncherInfoFile(FileInfo launcherInfoFile) {
            DirectoryInfo logbookDir = new DirectoryInfo(
                Path.Combine(launcherInfoFile.DirectoryName, launcherInfoFile.Name.Replace(launcherInfoFile.Extension, "")));
            if (!logbookDir.Exists) {
                logbookDir.Create();
            }

            return logbookDir;
        }

        private string GetLogbookBaseFileName(DateTime startDateTime) {
            return startDateTime.ToString("yyyy-MM-dd-hh-mm-ss");
        }

        private DirectoryInfo GetLogbookDirectoryByAircraftId(Guid aircraftId) {
            FileInfo launcherInfoFile = _aircraftModelProvider.Aircrafts.Single(x => x.Id == aircraftId).LauncherInfoFile;
            return GetLogbookDirectoryByLauncherInfoFile(launcherInfoFile);
        }

        private FileInfo GetLogbookEntryFile(Guid aircraftId, DateTime startDateTime, string discriminator) {
            DirectoryInfo logbookDir = GetLogbookDirectoryByAircraftId(aircraftId);
            string fileName = $"{GetLogbookBaseFileName(startDateTime)}.{discriminator}.json";
            return new FileInfo(Path.Combine(logbookDir.FullName, fileName));
        }
    }
}