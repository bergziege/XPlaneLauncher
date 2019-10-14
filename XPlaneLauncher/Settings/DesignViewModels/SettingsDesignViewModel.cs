using System;
using Prism.Commands;

namespace XPlaneLauncher.Settings.DesignViewModels {
    public class SettingsDesignViewModel : ISettingsViewModel {
        private DelegateCommand _createLuaScriptCommand;
        public string XPlaneRootPath { get; set; } = @"c:\test\x-plane\";
        public string DataPath { get; set; } = @"c:\data\launcher\";
        public DelegateCommand FinishCommand { get; }
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand SelectRootPathCommand { get; }
        public DelegateCommand SelectDataPathCommand { get; }
        public event EventHandler RequestCloseOnCancel;
        public event EventHandler RequestCloseOnFinish;

        public DelegateCommand CreateLuaScriptCommand { get; }
    }
}