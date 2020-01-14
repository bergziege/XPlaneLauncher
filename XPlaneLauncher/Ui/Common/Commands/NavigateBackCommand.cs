using Prism.Regions;
using XPlaneLauncher.Ui.Shell;

namespace XPlaneLauncher.Ui.Common.Commands {
    public class NavigateBackCommand {
        private readonly IRegionManager _journal;

        public NavigateBackCommand(IRegionManager journal) {
            _journal = journal;
        }

        public void Execute() {
            _journal.Regions[RegionNames.AppRegion].NavigationService.Journal.GoBack();
        }
    }
}