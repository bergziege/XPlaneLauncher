using System;
using Prism.Commands;

namespace XPlaneLauncher.Ui.Modules.Settings.ViewModels {
    public interface ISettingsViewModel {
        string XPlaneRootPath { get; set; }

        string DataPath { get; set; }

        DelegateCommand FinishCommand { get; }

        DelegateCommand CancelCommand { get; }

        DelegateCommand SelectRootPathCommand { get; }

        DelegateCommand SelectDataPathCommand { get; }

        DelegateCommand CreateLuaScriptCommand { get; }

        string ErrorMessage { get; }
    }
}