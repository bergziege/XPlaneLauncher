using System.Collections.Generic;
using System.IO;
using MapControl;

namespace XPlaneLauncher.Persistence.Impl {
    public class LogbookEntryTrackDao : ILogbookEntryTrackDao {
        public void Delete(FileInfo trackFile) {
            if (trackFile.Exists) {
                trackFile.Delete();
            }
        }

        public IList<Location> GetTrack(FileInfo trackFile) {
            return new List<Location>();
        }

        public void Save(FileInfo trackFile, IList<Location> track) {
            
        }
    }
}