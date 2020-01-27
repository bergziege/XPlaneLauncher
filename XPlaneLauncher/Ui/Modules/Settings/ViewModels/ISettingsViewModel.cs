using Prism.Commands;

namespace XPlaneLauncher.Ui.Modules.Settings.ViewModels {
    public interface ISettingsViewModel {
        DelegateCommand CancelCommand { get; }

        DelegateCommand CreateLuaScriptCommand { get; }

        string DataPath { get; set; }

        string ErrorMessage { get; }

        DelegateCommand FinishCommand { get; }

        DelegateCommand SelectDataPathCommand { get; }

        DelegateCommand SelectRootPathCommand { get; }
        string XPlaneRootPath { get; set; }
    }
}