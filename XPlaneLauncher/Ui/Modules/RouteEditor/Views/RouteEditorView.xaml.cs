using System.Windows.Controls;
using System.Windows.Input;
using XPlaneLauncher.Model;
using XPlaneLauncher.Ui.Modules.RouteEditor.ViewModels;

namespace XPlaneLauncher.Ui.Modules.RouteEditor.Views {
    /// <summary>
    ///     Interaktionslogik für RouteEditorView.xaml
    /// </summary>
    public partial class RouteEditorView : UserControl {
        public RouteEditorView() {
            InitializeComponent();
        }

        private void RoutePoints_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (DataContext is IRouteEditorViewModel vm && vm.SelectedRoutePoint != null) {
                RoutePoints.ScrollIntoView(vm.SelectedRoutePoint);
            }
        }

        private void UIElement_OnPreviewMouseDown(object sender, MouseButtonEventArgs e) {
            if (sender is Border ctl && ctl.DataContext is RoutePoint rtp) {
                rtp.IsSelected = true;
            }

            e.Handled = false;
        }
    }
}