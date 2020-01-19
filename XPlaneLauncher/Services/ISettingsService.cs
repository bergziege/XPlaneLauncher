using System.Collections.Generic;

namespace XPlaneLauncher.Services {
    public interface ISettingsService {
        bool IsHavingRequiredDirectories(string xplaneRootPath, string dataPath, out IList<string> errors);
    }
}