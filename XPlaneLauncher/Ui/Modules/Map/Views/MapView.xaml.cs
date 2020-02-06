using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using MapControl;
using Prism.Mvvm;
using XPlaneLauncher.Ui.Modules.Map.Dtos;
using XPlaneLauncher.Ui.Modules.Map.ViewModels;
using XPlaneLauncher.Ui.Modules.Map.ViewModels.Runtime;

namespace XPlaneLauncher.Ui.Modules.Map.Views {
    /// <summary>
    ///     Interaktionslogik für MapView.xaml
    /// </summary>
    public partial class MapView {
        private bool boundingBoxChangeRegistered = false;

        public MapView() {
            InitializeComponent();
        }

        private void Map_OnViewportChanged(object sender, ViewportChangedEventArgs e) {
            if (DataContext is MapViewModel mainViewModel) {
                Location topLeft = Map.ViewportPointToLocation(new Point(0, 0));
                Location bottomRight = Map.ViewportPointToLocation(new Point(Map.ActualWidth, Map.ActualHeight));
                mainViewModel.MapBoundariesChangedCommand.Execute(new MapBoundary(topLeft, bottomRight));
                if (!boundingBoxChangeRegistered) {
                    mainViewModel.PropertyChanged += Vm_PropertyChanged;
                    boundingBoxChangeRegistered = true;
                }
            }
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            if (e.ClickCount == 2 && DataContext is IMapViewModel vm) {
                Location selectedLocation = Map.ViewportPointToLocation(e.GetPosition(Map));
                vm.LocationSelectedCommand.Execute(selectedLocation);
            }
        }

        private void Vm_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (DataContext is IMapViewModel vm && vm.BoundingBox != null && e.PropertyName == nameof(IMapViewModel.BoundingBox)) {
                Map.ZoomToBounds(vm.BoundingBox);
            }
        }
    }
}