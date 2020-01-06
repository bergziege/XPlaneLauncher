using System;
using Prism.Commands;

namespace XPlaneLauncher.Ui.Modules.Settings.ViewModels.Design {
    public class SettingsDesignViewModel : ISettingsViewModel {
        public string XPlaneRootPath { get; set; } = @"c:\test\x-plane\";
        public string DataPath { get; set; } = @"c:\data\launcher\";
        public DelegateCommand FinishCommand { get; } = new DelegateCommand(() => { });
        public DelegateCommand CancelCommand { get; } = new DelegateCommand(() => { });
        public DelegateCommand SelectRootPathCommand { get; } = new DelegateCommand(() => { });
        public DelegateCommand SelectDataPathCommand { get; } = new DelegateCommand(() => { });
        public event EventHandler RequestCloseOnCancel;
        public event EventHandler RequestCloseOnFinish;
        public DelegateCommand CreateLuaScriptCommand { get; } = new DelegateCommand(() => { });

        public string ErrorMessage { get; } = "Ein etwas längerer Fehler mit bissl Text aber ohne Sinn.";
    }
}