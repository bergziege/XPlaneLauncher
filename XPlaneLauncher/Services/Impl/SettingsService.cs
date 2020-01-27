using System.Collections.Generic;
using System.IO;

namespace XPlaneLauncher.Services.Impl {
    public class SettingsService : ISettingsService{
        public bool IsHavingRequiredDirectories(string xplaneRootPath, string dataPath, out IList<string> errors) {
            errors = new List<string>();
            bool pathsExists = true;
            if (!Directory.Exists(xplaneRootPath)) {
                pathsExists = false;
                errors.Add("X-Plane root not found.");
            }

            if (!Directory.Exists(dataPath)) {
                pathsExists = false;
                errors.Add("Data directory not found.");
            }
            return pathsExists;
        }
    }
}