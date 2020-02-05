using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using XPlaneLauncher.Domain;

namespace XPlaneLauncher.Persistence {
    public interface ILogbookEntryDao {
        void SaveWithoutTrack(FileInfo logbookFile, LogbookEntry entry);
        Task<IList<LogbookEntry>> GetEntriesAsync(DirectoryInfo logbookDirectory);
    }
}