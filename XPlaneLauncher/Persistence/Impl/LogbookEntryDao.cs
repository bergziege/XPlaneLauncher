using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using XPlaneLauncher.Domain;

namespace XPlaneLauncher.Persistence.Impl {
    public class LogbookEntryDao : ILogbookEntryDao {
        public void Delete(FileInfo logFile) {
            if (logFile.Exists) {
                logFile.Delete();
            }
        }

        public async Task<IList<LogbookEntry>> GetEntriesAsync(DirectoryInfo logbookDirectory) {
            FileInfo[] fileInfos = logbookDirectory.GetFiles("*.log.json");
            IList<LogbookEntry> entries = new List<LogbookEntry>();
            foreach (FileInfo fileInfo in fileInfos) {
                string logbookEntryContent;
                using (StreamReader reader = File.OpenText(fileInfo.FullName)) {
                    logbookEntryContent = await reader.ReadToEndAsync();
                }

                entries.Add(JsonConvert.DeserializeObject<LogbookEntry>(logbookEntryContent));
            }

            return entries.OrderBy(x => x.StartDateTime).ToList();
        }

        public void SaveWithoutTrack(FileInfo logbookFile, LogbookEntry entry) {
            string content = JsonConvert.SerializeObject(entry);
            File.WriteAllText(logbookFile.FullName, content);
        }
    }
}