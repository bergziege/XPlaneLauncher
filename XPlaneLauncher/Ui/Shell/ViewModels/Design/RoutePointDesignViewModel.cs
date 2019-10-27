using System;
using MapControl;
using Prism.Mvvm;

namespace XPlaneLauncher.Ui.Shell.ViewModels.Design {
    public class RoutePointDesignViewModel : BindableBase, IRoutePointViewModel {
        public string Comment { get; set; } = $"Testcomment{Environment.NewLine}Multilined";
        public bool IsSelected { get; set; } = true;

        public Location Location { get; } = new Location(51, 13);

        public string Name { get; set; } = "Testname";

        public void Deselect() {
            throw new NotImplementedException();
        }

        public void Select() {
            throw new NotImplementedException();
        }
    }
}