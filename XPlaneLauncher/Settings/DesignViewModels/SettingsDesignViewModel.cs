using Prism.Commands;

namespace XPlaneLauncher.Settings.DesignViewModels
{
    public class SettingsDesignViewModel : ISettingsViewModel
    {
        public string XPlaneRootPath { get; set; } = @"c:\test\x-plane\";
        public string XPlaneSituationPath { get; } = @"output\situations\";
        public string DataPath { get; set; } = @"c:\data\launcher\";
        public DelegateCommand FinishCommand { get; }
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand SelectRootPathCommand { get; }
        public DelegateCommand SelectDataPathCommand { get; }
    }
}