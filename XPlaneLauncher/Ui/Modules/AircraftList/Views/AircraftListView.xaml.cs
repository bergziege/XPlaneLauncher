using System;
using System.Windows;
using System.Windows.Controls;
using XPlaneLauncher.Ui.Modules.AircraftList.ViewModels;

namespace XPlaneLauncher.Ui.Modules.AircraftList.Views {
    /// <summary>
    /// Interaktionslogik für AircraftListView.xaml
    /// </summary>
    public partial class AircraftListView : UserControl {
        public AircraftListView() {
            InitializeComponent();
        }

        private void Aircrafts_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (DataContext is IAircraftListViewModel vm && vm.SelectedAircraft != null) {
                Aircrafts.ScrollIntoView(vm.SelectedAircraft);
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e) {
            if (DataContext is IAircraftListViewModel vm && vm.EditSelectedAircraftRoute.CanExecute()) {
                vm.EditSelectedAircraftRoute.Execute();
            }
        }
    }
}
