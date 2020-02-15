using System.Collections.Generic;
using System.IO;
using MapControl;
using XPlaneLauncher.Domain;

namespace XPlaneLauncher.Persistence {
    public interface ILogbookEntryTrackDao {
        void Save(FileInfo trackFile, IList<LogbookTrackItem> track);
        void Delete(FileInfo trackFile);
        IList<LogbookTrackItem> GetTrack(FileInfo getLogbookEntryFile);
    }
}