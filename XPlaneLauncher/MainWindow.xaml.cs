using System;
using System.Windows.Input;
using MapControl;

namespace XPlaneLauncher
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            ImageLoader.HttpClient.DefaultRequestHeaders.Add("User-Agent", "Your App");
            TileImageLoader.Cache = new MapControl.Caching.ImageFileCache(TileImageLoader.DefaultCacheFolder);
            TileImageLoader.DefaultCacheExpiration = TimeSpan.FromHours(24);
            //TileImageLoader.Cache = null;
            DataContextChanged += MainWindow_DataContextChanged;
        }

        private void MainWindow_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is IMainViewModel vm)
            {
                vm.PropertyChanged += Vm_PropertyChanged;
            }
        }

        private void Vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IMainViewModel.SelectedAircraft) && DataContext is IMainViewModel vm && vm.SelectedAircraft != null)
            {
                Aircrafts.ScrollIntoView(vm.SelectedAircraft);
            }
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                //map.ZoomMap(e.GetPosition(map), Math.Floor(map.ZoomLevel + 1.5));
                //map.ZoomToBounds(new BoundingBox(53, 7, 54, 9));
                Location target = Map.ViewportPointToLocation(e.GetPosition(Map));
                if (DataContext is IMainViewModel vm && vm.ApplyTargetCommand.CanExecute(target))
                {
                    vm.ApplyTargetCommand.Execute(target);
                }
            }
        }
    }
}