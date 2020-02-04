using System.IO;
using Newtonsoft.Json;
using XPlaneLauncher.Domain;

namespace XPlaneLauncher.Persistence.Impl {
    public class LogbookEntryDao : ILogbookEntryDao {
        public void SaveWithoutTrack(FileInfo logbookFile, LogbookEntry entry) {
            string content = JsonConvert.SerializeObject(entry);
            File.WriteAllText(logbookFile.FullName, content);
        }
    }
}