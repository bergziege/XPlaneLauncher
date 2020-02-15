using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using MapControl;
using XPlaneLauncher.Domain;

namespace XPlaneLauncher.Persistence.Impl {
    public class LogbookEntryTrackDao : ILogbookEntryTrackDao {
        public void Delete(FileInfo trackFile) {
            if (trackFile.Exists) {
                trackFile.Delete();
            }
        }

        public IList<LogbookTrackItem> GetTrack(FileInfo trackFile) {
            return new List<LogbookTrackItem>();
        }

        public void Save(FileInfo trackFile, IList<LogbookTrackItem> track) {
            using (var writer = new StreamWriter(trackFile.FullName))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)) {
                csv.WriteRecords(track);
            }
        }
    }
}