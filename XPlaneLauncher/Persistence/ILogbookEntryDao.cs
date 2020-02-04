using System.IO;
using XPlaneLauncher.Domain;

namespace XPlaneLauncher.Persistence {
    public interface ILogbookEntryDao {
        void SaveWithoutTrack(FileInfo logbookFile, LogbookEntry entry);
    }
}