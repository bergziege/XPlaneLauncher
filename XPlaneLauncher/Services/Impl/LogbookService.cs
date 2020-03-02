using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using MapControl;
using UnitsNet;
using XPlaneLauncher.Domain;
using XPlaneLauncher.Dtos;
using XPlaneLauncher.Model;
using XPlaneLauncher.Model.Provider;
using XPlaneLauncher.Persistence;

namespace XPlaneLauncher.Services.Impl {
    public class LogbookService : ILogbookService {
        private readonly IAcmiService _acmiService;
        private readonly IAircraftModelProvider _aircraftModelProvider;
        private readonly ILogbookEntryDao _logbookEntryDao;
        private readonly ILogbookEntryTrackDao _logbookEntryTrackDao;

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public LogbookService(
            ILogbookEntryDao logbookEntryDao, ILogbookEntryTrackDao logbookEntryTrackDao,
            IAircraftModelProvider aircraftModelProvider, IAcmiService acmiService) {
            _logbookEntryDao = logbookEntryDao;
            _logbookEntryTrackDao = logbookEntryTrackDao;
            _aircraftModelProvider = aircraftModelProvider;
            _acmiService = acmiService;
        }

        public void CreateAcmiZipEntry(
            FileInfo importFile, Guid aircraftId, DateTime startDateTime, DateTime endDateTime, TimeSpan duration, IList<LogbookLocation> track,
            double distanceNauticalMiles,
            string notes) {
            CreateEntry(LogbookEntryType.AcmiZip, aircraftId, startDateTime, endDateTime, duration, track, distanceNauticalMiles, notes);
            if (importFile.Exists) {
                DirectoryInfo logbookDir = GetLogbookDirectoryByAircraftId(aircraftId);
                FileInfo acmiFile = GetLogbookEntryFile(aircraftId, startDateTime, "acmi", "zip");
                importFile.MoveTo(acmiFile.FullName);
            }
        }

        public void CreateManualEntry(
            Guid aircraftId, DateTime startDateTime, DateTime endDateTime, TimeSpan duration, IList<LogbookLocation> track,
            double distanceNauticalMiles,
            string notes) {
            CreateEntry(LogbookEntryType.Manual, aircraftId, startDateTime, endDateTime, duration, track, distanceNauticalMiles, notes);
        }

        public void DeleteEntry(Guid aircraftId, LogbookEntry entry) {
            /* Delete log file */
            FileInfo logFile = GetLogbookEntryFile(aircraftId, entry.StartDateTime, "log", "json");
            _logbookEntryDao.Delete(logFile);

            /* Delete track file */
            FileInfo trackFile = GetLogbookEntryFile(aircraftId, entry.StartDateTime, "track", "csv");
            _logbookEntryTrackDao.Delete(trackFile);

            if (entry.Type == LogbookEntryType.AcmiZip) {
                /* Delete imported file */
                FileInfo acmiFile = GetLogbookEntryFile(aircraftId, entry.StartDateTime, "acmi", "zip");
                if (acmiFile.Exists) {
                    acmiFile.Delete();
                }
            }

            if (!logFile.Directory.GetFiles().Any()) {
                logFile.Directory.Delete();
            }
        }

        public LogbookEntry ExpandTrack(Guid aircraftId, LogbookEntry logbookEntry) {
            IList<LogbookLocation> track = ToLogbookLocations(
                _logbookEntryTrackDao.GetTrack(GetLogbookEntryFile(aircraftId, logbookEntry.StartDateTime, "track", "csv")));
            logbookEntry.Update(track);
            return logbookEntry;
        }

        public async Task<IList<LogbookEntry>> GetEntriesWithoutTrackAsync(Aircraft aircraft) {
            if (aircraft.LauncherInfoFile != null) {
                DirectoryInfo logbookDirectory = GetLogbookDirectoryByLauncherInfoFile(aircraft.LauncherInfoFile);
                return await _logbookEntryDao.GetEntriesAsync(logbookDirectory); 
            }
            return new List<LogbookEntry>();
        }

        public LogbookEntry GetEntryFromAcmiFile(FileInfo acmiFile) {
            /* copy to temp */
            FileInfo tmpFile = acmiFile.CopyTo(Path.GetTempFileName(), true);

            /* unzip */
            string unzipDir = Path.Combine(tmpFile.DirectoryName, "XPlaneLauncher", Path.GetRandomFileName());
            ZipFile.ExtractToDirectory(tmpFile.FullName, unzipDir);
            FileInfo unzippedFile = new DirectoryInfo(unzipDir).GetFiles().First();

            /* parse */
            AcmiDto acmiDto = _acmiService.ParseFile(unzippedFile);

            /* remove tmp file */
            tmpFile.Delete();
            unzippedFile.Directory?.Delete(true);

            double trackLength = 0;
            for (int index = 0; index < acmiDto.Track.Count; index++) {
                Location location = acmiDto.Track[index];
                Location nextLocation = acmiDto.Track.ElementAtOrDefault(index + 1);
                if (nextLocation != null) {
                    trackLength += location.GreatCircleDistance(nextLocation);
                }
            }

            trackLength = Length.FromMeters(trackLength).NauticalMiles;

            /* Take year from recording time as year for reference time  - currently ignore february 29th problems*/
            acmiDto.ReferenceTime = new DateTime(
                acmiDto.RecordingTime.Year,
                acmiDto.ReferenceTime.Month,
                acmiDto.ReferenceTime.Day,
                acmiDto.ReferenceTime.Hour,
                acmiDto.ReferenceTime.Minute,
                acmiDto.ReferenceTime.Second);

            LogbookEntry autoLogEntry = new LogbookEntry(
                LogbookEntryType.AcmiZip,
                acmiDto.RecordingTime,
                acmiDto.RecordingTime.Add(acmiDto.Duration),
                acmiDto.Duration,
                acmiDto.Track,
                trackLength);
            autoLogEntry.Update(acmiFile.Name);

            return autoLogEntry;
        }

        public void UpdateAutoEntry(
            LogbookEntry oldEntry, Guid aircraftId, DateTime startDateTime, DateTime endDateTime, TimeSpan duration, IList<LogbookLocation> track,
            double distanceNauticalMiles, string notes) {
            CreateEntry(LogbookEntryType.AcmiZip, aircraftId, startDateTime, endDateTime, duration, track, distanceNauticalMiles, notes);
        }

        public void UpdateManualEntry(
            LogbookEntry oldEntry, Guid aircraftId, DateTime startDateTime, DateTime endDateTime, TimeSpan duration, IList<LogbookLocation> track,
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

        private void CreateEntry(
            LogbookEntryType entryType, Guid aircraftId, DateTime startDateTime, DateTime endDateTime, TimeSpan duration, IList<LogbookLocation> track,
            double distanceNauticalMiles,
            string notes) {
            LogbookEntry newEntry = new LogbookEntry(entryType, startDateTime, endDateTime, duration, track, distanceNauticalMiles);
            newEntry.Update(notes);

            FileInfo logbookEntryFile = GetLogbookEntryFile(aircraftId, startDateTime, "log", "json");
            FileInfo logbookEntryTrackFile = GetLogbookEntryFile(aircraftId, startDateTime, "track", "csv");
            /* Persist entry to json*/
            _logbookEntryDao.SaveWithoutTrack(logbookEntryFile, newEntry);

            /* Persist track as GeoJSON */
            _logbookEntryTrackDao.Save(logbookEntryTrackFile, LocationsToEntries(newEntry.Track));
        }

        private IList<LogbookLocation> ToLogbookLocations(IList<LogbookTrackItem> trackItems) {
            return trackItems.Select(x => new LogbookLocation(x.Time, x.Lat, x.Lon) {
                Alt = x.Alt,
                Hdg = x.Hdg,
                Ias = x.Ias
            }).ToList();
        }

        private string GetLogbookBaseFileName(DateTime startDateTime) {
            return startDateTime.ToString("yyyy-MM-dd-hh-mm-ss");
        }

        private DirectoryInfo GetLogbookDirectoryByAircraftId(Guid aircraftId) {
            FileInfo launcherInfoFile = _aircraftModelProvider.Aircrafts.Single(x => x.Id == aircraftId).LauncherInfoFile;
            return GetLogbookDirectoryByLauncherInfoFile(launcherInfoFile);
        }

        private FileInfo GetLogbookEntryFile(Guid aircraftId, DateTime startDateTime, string discriminator, string extension) {
            DirectoryInfo logbookDir = GetLogbookDirectoryByAircraftId(aircraftId);
            string fileName = $"{GetLogbookBaseFileName(startDateTime)}.{discriminator}.{extension}";
            return new FileInfo(Path.Combine(logbookDir.FullName, fileName));
        }

        private IList<LogbookTrackItem> LocationsToEntries(IList<LogbookLocation> trackLocations) {
            return trackLocations.Select(
                x => new LogbookTrackItem() {
                    Time = x.DateTime,
                    Lat = x.Latitude,
                    Lon = x.Longitude,
                    Alt = x.Alt,
                    Hdg = x.Hdg,
                    Ias = x.Ias
                }).ToList();
        }
    }
}