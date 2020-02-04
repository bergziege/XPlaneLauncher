using System.Collections.Generic;
using System.IO;
using MapControl;

namespace XPlaneLauncher.Persistence {
    public interface ILogbookEntryTrackDao {
        void Save(FileInfo trackFile, IList<Location> track);
    }
}