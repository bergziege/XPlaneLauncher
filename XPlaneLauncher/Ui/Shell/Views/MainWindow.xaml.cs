using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MapControl;
using XPlaneLauncher.Dtos;
using XPlaneLauncher.Ui.Shell.ViewModels;

namespace XPlaneLauncher.Ui.Shell.Views {
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        public MainWindow() {
            InitializeComponent();
            ImageLoader.HttpClient.DefaultRequestHeaders.Add("User-Agent", "Your App");
            TileImageLoader.Cache = new MapControl.Caching.ImageFileCache(TileImageLoader.DefaultCacheFolder);
            TileImageLoader.DefaultCacheExpiration = TimeSpan.FromHours(24);
            //TileImageLoader.Cache = null;
            DataContextChanged += MainWindow_DataContextChanged;
        }

        private void MainWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e) {
            if (DataContext is IMainViewModel vm) {
                vm.PropertyChanged += Vm_PropertyChanged;
            }
        }

        private void Vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            //if (e.PropertyName == nameof(IMainViewModel.SelectedAircraft) && DataContext is IMainViewModel vm && vm.SelectedAircraft != null) {
            //    Aircrafts.ScrollIntoView(vm.SelectedAircraft);
            //}
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            //if (e.ClickCount == 2) {
            //    //map.ZoomMap(e.GetPosition(map), Math.Floor(map.ZoomLevel + 1.5));
            //    //map.ZoomToBounds(new BoundingBox(53, 7, 54, 9));
            //    Location target = Map.ViewportPointToLocation(e.GetPosition(Map));
            //    if (DataContext is IMainViewModel vm && vm.ApplyTargetCommand.CanExecute(target)) {
            //        vm.ApplyTargetCommand.Execute(target);
            //    }
            //}
        }

        private void Map_OnViewportChanged(object sender, ViewportChangedEventArgs e) {
            //if (DataContext is IMainViewModel mainViewModel) {
            //    Location topLeft = Map.ViewportPointToLocation(new Point(0, 0));
            //    Location bottomRight = Map.ViewportPointToLocation(new Point(Map.ActualWidth, Map.ActualHeight));
            //    mainViewModel.MapBoundariesChangedCommand.Execute(new MapBoundary(topLeft, bottomRight));
            //}
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            //RoutePointList.ScrollIntoView(RoutePointList.SelectedItem);
        }
    }
}