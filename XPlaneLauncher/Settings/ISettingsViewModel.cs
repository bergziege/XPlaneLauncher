using Prism.Commands;

namespace XPlaneLauncher.Settings
{
    public interface ISettingsViewModel
    {
        string XPlaneRootPath { get; set; }
        string XPlaneSituationPath { get; }

        string DataPath { get; set; }

        DelegateCommand FinishCommand { get; }

        DelegateCommand CancelCommand { get; }

        DelegateCommand SelectRootPathCommand { get; }

        DelegateCommand SelectDataPathCommand { get; }
    }
}