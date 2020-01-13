using System;
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
    }
}
