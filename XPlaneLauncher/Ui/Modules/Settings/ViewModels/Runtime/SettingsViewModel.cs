using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using XPlaneLauncher.Services;
using XPlaneLauncher.Ui.Common.Commands;

namespace XPlaneLauncher.Ui.Modules.Settings.ViewModels.Runtime {
    public class SettingsViewModel : BindableBase, ISettingsViewModel {
        private readonly NavigateBackCommand _navigateBackCommand;
        private readonly ISettingsService _settingsService;
        private DelegateCommand _cancelCommand;
        private DelegateCommand _createLuaScriptCommand;
        private string _dataPath;
        private string _errorMessage;
        private DelegateCommand _finishCommand;
        private DelegateCommand _selectDataPathCommand;
        private DelegateCommand _selectRootPathCommand;
        private string _xPlaneRootPath;

        public SettingsViewModel(
            NavigateBackCommand navigateBackCommand,
            ISettingsService settingsService) {
            _navigateBackCommand = navigateBackCommand;
            _settingsService = settingsService;
            XPlaneRootPath = Properties.Settings.Default.XPlaneRootPath;
            DataPath = Properties.Settings.Default.DataPath;
        }

        public DelegateCommand CancelCommand {
            get {
                if (_cancelCommand == null) {
                    _cancelCommand = new DelegateCommand(OnCancel, CanCancel);
                }

                return _cancelCommand;
            }
        }

        public DelegateCommand CreateLuaScriptCommand {
            get {
                if (_createLuaScriptCommand == null) {
                    _createLuaScriptCommand = new DelegateCommand(OnCreateLuaScript, CanCreateLuaScript);
                }

                return _createLuaScriptCommand;
            }
        }

        public string DataPath {
            get => _dataPath;
            set => SetProperty(ref _dataPath, value, nameof(DataPath));
        }

        public string ErrorMessage {
            get { return _errorMessage; }
            private set { SetProperty(ref _errorMessage, value, nameof(ErrorMessage)); }
        }

        public DelegateCommand FinishCommand {
            get {
                if (_finishCommand == null) {
                    _finishCommand = new DelegateCommand(OnFinish, CanFinish);
                }

                return _finishCommand;
            }
        }

        public DelegateCommand SelectDataPathCommand {
            get {
                if (_selectDataPathCommand == null) {
                    _selectDataPathCommand = new DelegateCommand(OnSelectDataPath, CanSelectDataPath);
                }

                return _selectDataPathCommand;
            }
        }

        public DelegateCommand SelectRootPathCommand {
            get {
                if (_selectRootPathCommand == null) {
                    _selectRootPathCommand = new DelegateCommand(OnSelectRootPath, CanSelectRootPath);
                }

                return _selectRootPathCommand;
            }
        }

        public string XPlaneRootPath {
            get => _xPlaneRootPath;
            set => SetProperty(ref _xPlaneRootPath, value, nameof(XPlaneRootPath));
        }

        private bool AllPathsExist() {
            bool pathsExists = _settingsService.IsHavingRequiredDirectories(XPlaneRootPath, DataPath, out IList<string> errors);

            if (errors.Any()) {
                ErrorMessage = string.Join(", ", errors);
            } else {
                ErrorMessage = null;
            }

            return pathsExists;
        }

        private bool CanCancel() {
            return AllPathsExist();
        }

        private bool CanCreateLuaScript() {
            return AllPathsExist() && LuaScriptPathExists();
        }

        private bool CanFinish() {
            return AllPathsExist();
        }

        private bool CanSelectDataPath() {
            return true;
        }

        private bool CanSelectRootPath() {
            return true;
        }

        private void CommandsRaiseCanExecuteChanged() {
            CancelCommand.RaiseCanExecuteChanged();
            FinishCommand.RaiseCanExecuteChanged();
            CreateLuaScriptCommand.RaiseCanExecuteChanged();
        }

        private void CreateLuaScript() {
            string luaScript = File.ReadAllText("./Assets/LuaScript/BasicInfoLogger.lua");
            luaScript = luaScript.Replace(
                "####",
                $"{DataPath.Replace("\\", "/")}/");
            File.WriteAllText(
                Path.Combine(XPlaneRootPath, Properties.Settings.Default.LuaPathRelativeToXPlaneRoot, Properties.Settings.Default.LuaScriptFileName),
                luaScript);
        }

        private bool LuaScriptPathExists() {
            return Directory.Exists(
                Path.Combine(
                    XPlaneRootPath,
                    Properties.Settings.Default.LuaPathRelativeToXPlaneRoot));
        }

        private void OnCancel() {
            _navigateBackCommand.Execute();
        }

        private void OnCreateLuaScript() {
            CreateLuaScript();
        }

        private void OnFinish() {
            SaveSettings();
            _navigateBackCommand.Execute();
        }

        private void OnSelectDataPath() {
            VistaFolderBrowserDialog browserDialog = new VistaFolderBrowserDialog();
            browserDialog.SelectedPath = DataPath;
            bool? showDialog = browserDialog.ShowDialog();
            if (showDialog.HasValue && showDialog.Value) {
                DataPath = browserDialog.SelectedPath;
                CommandsRaiseCanExecuteChanged();
            }
        }

        private void OnSelectRootPath() {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = XPlaneRootPath;
            ofd.Filter =
                $"{Properties.Settings.Default.XPlaneExecutableFile} | {Properties.Settings.Default.XPlaneExecutableFile}";
            bool? showDialog = ofd.ShowDialog();
            if (showDialog.HasValue && showDialog.Value) {
                XPlaneRootPath = Path.GetDirectoryName(ofd.FileName);
                CommandsRaiseCanExecuteChanged();
            }
        }

        private void SaveSettings() {
            Properties.Settings.Default.XPlaneRootPath = XPlaneRootPath;
            Properties.Settings.Default.DataPath = DataPath;
            Properties.Settings.Default.Save();
        }
    }
}