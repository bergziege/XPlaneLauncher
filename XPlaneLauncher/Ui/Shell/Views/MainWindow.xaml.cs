using System;
using MapControl;
using MapControl.Caching;

namespace XPlaneLauncher.Ui.Shell.Views {
    /// <summary>
    ///     Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        public MainWindow() {
            InitializeComponent();
            ImageLoader.HttpClient.DefaultRequestHeaders.Add("User-Agent", "Your App");
            TileImageLoader.Cache = new ImageFileCache(TileImageLoader.DefaultCacheFolder);
            TileImageLoader.DefaultCacheExpiration = TimeSpan.FromDays(7);
        }
    }
}