using Prism.Commands;

namespace XPlaneLauncher.Ui.Modules.Settings.ViewModels {
    public interface ISettingsViewModel {
        DelegateCommand BackCommand { get; }

        DelegateCommand CreateLuaScriptCommand { get; }

        string DataPath { get; set; }

        string ErrorMessage { get; }

        DelegateCommand SelectDataPathCommand { get; }

        DelegateCommand SelectRootPathCommand { get; }

        string XPlaneRootPath { get; set; }
    }
}