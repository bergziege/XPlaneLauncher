using System;
using System.IO;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using Prism.Commands;
using Prism.Mvvm;

namespace XPlaneLauncher.Settings.ViewModels {
    public class SettingsViewModel : BindableBase, ISettingsViewModel {
        private string _xPlaneRootPath;
        private string _xPlaneSituationPath;
        private string _dataPath;
        private DelegateCommand _finishCommand;
        private DelegateCommand _cancelCommand;
        private DelegateCommand _selectRootPathCommand;
        private DelegateCommand _selectDataPathCommand;

        public SettingsViewModel() {
            XPlaneRootPath = Properties.Settings.Default.XPlaneRootPath;
            DataPath = Properties.Settings.Default.DataPath;
        }

        public string XPlaneRootPath {
            get => _xPlaneRootPath;
            set => SetProperty(ref _xPlaneRootPath, value, nameof(XPlaneRootPath));
        }

        public string DataPath {
            get => _dataPath;
            set => SetProperty(ref _dataPath, value, nameof(DataPath));
        }

        public DelegateCommand FinishCommand {
            get {
                if (_finishCommand == null) _finishCommand = new DelegateCommand(OnFinish, CanFinish);
                return _finishCommand;
            }
        }

        public DelegateCommand CancelCommand {
            get {
                if (_cancelCommand == null) _cancelCommand = new DelegateCommand(OnCancel, CanCancel);
                return _cancelCommand;
            }
        }

        public DelegateCommand SelectRootPathCommand {
            get {
                if (_selectRootPathCommand == null)
                    _selectRootPathCommand = new DelegateCommand(OnSelectRootPath, CanSelectRootPath);
                return _selectRootPathCommand;
            }
        }

        public DelegateCommand SelectDataPathCommand {
            get {
                if (_selectDataPathCommand == null)
                    _selectDataPathCommand = new DelegateCommand(OnSelectDataPath, CanSelectDataPath);
                return _selectDataPathCommand;
            }
        }

        public event EventHandler RequestCloseOnCancel;
        public event EventHandler RequestCloseOnFinish;

        private bool AllPathsExist() {
            return Directory.Exists(XPlaneRootPath) && Directory.Exists(DataPath);
        }

        private void OnFinish() {
            SaveSettings();
            OnRequestCloseOnFinish();
        }

        private void SaveSettings() {
            Properties.Settings.Default.XPlaneRootPath = XPlaneRootPath;
            Properties.Settings.Default.DataPath = DataPath;
            Properties.Settings.Default.Save();
        }

        private bool CanFinish() {
            return AllPathsExist();
        }

        private bool CanCancel() {
            return AllPathsExist();
        }

        private void OnCancel() {
            OnRequestCloseOnCancel();
        }

        private bool CanSelectRootPath() {
            return true;
        }

        private void OnSelectRootPath() {
            var ofd = new OpenFileDialog();
            ofd.InitialDirectory = XPlaneRootPath;
            ofd.Filter = $"{Properties.Settings.Default.XPlaneExecutableFile} | {Properties.Settings.Default.XPlaneExecutableFile}";
            var showDialog = ofd.ShowDialog();
            if (showDialog.HasValue && showDialog.Value)
            {
                XPlaneRootPath = Path.GetDirectoryName(ofd.FileName);
                CancelCommand.RaiseCanExecuteChanged();
                FinishCommand.RaiseCanExecuteChanged();
            }
        }

        private bool CanSelectDataPath() {
            return true;
        }

        private void OnSelectDataPath() {
            var browserDialog = new VistaFolderBrowserDialog();
            browserDialog.SelectedPath = DataPath;
            var showDialog = browserDialog.ShowDialog();
            if (showDialog.HasValue && showDialog.Value)
            {
                DataPath = browserDialog.SelectedPath;
                CancelCommand.RaiseCanExecuteChanged();
                FinishCommand.RaiseCanExecuteChanged();
            }
        }

        protected virtual void OnRequestCloseOnCancel() {
            RequestCloseOnCancel?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnRequestCloseOnFinish() {
            RequestCloseOnFinish?.Invoke(this, EventArgs.Empty);
        }
    }
}