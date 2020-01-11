using System.IO;

namespace XPlaneLauncher.Model {
    public class Situation {
        public Situation(bool hasSit, FileInfo sitFile) {
            HasSit = hasSit;
            SitFile = sitFile;
        }

        public bool HasSit { get; }
        public FileInfo SitFile { get; }
    }
}