using System.Windows.Controls;
using System.Windows.Input;
using MapControl;

namespace XPlaneLauncher.Ui.Modules.Map.Views {
    /// <summary>
    /// Interaktionslogik für MapView.xaml
    /// </summary>
    public partial class MapView : UserControl {
        public MapView() {
            InitializeComponent();
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
        }

        private void Map_OnViewportChanged(object sender, ViewportChangedEventArgs e) {
        }
    }
}