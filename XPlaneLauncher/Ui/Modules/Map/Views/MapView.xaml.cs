using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MapControl;
using XPlaneLauncher.Ui.Modules.Map.Dtos;
using XPlaneLauncher.Ui.Modules.Map.ViewModels;

namespace XPlaneLauncher.Ui.Modules.Map.Views {
    /// <summary>
    /// Interaktionslogik für MapView.xaml
    /// </summary>
    public partial class MapView : UserControl {
        public MapView() {
            InitializeComponent();
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            if (e.ClickCount == 2 && DataContext is IMapViewModel vm) {
                Location selectedLocation = Map.ViewportPointToLocation(e.GetPosition(Map));
                vm.LocationSelectedCommand.Execute(selectedLocation);
            }
        }

        private void Map_OnViewportChanged(object sender, ViewportChangedEventArgs e) {
            if (DataContext is IMapViewModel mainViewModel) {
                Location topLeft = Map.ViewportPointToLocation(new Point(0, 0));
                Location bottomRight = Map.ViewportPointToLocation(new Point(Map.ActualWidth, Map.ActualHeight));
                mainViewModel.MapBoundariesChangedCommand.Execute(new MapBoundary(topLeft, bottomRight));
            }
        }
    }
}