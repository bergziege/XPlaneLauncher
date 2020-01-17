﻿using Prism.Commands;
using XPlaneLauncher.Model;

namespace XPlaneLauncher.Ui.Modules.RouteEditor.ViewModels.Design {
    public class RouteEditorDesignViewModel : IRouteEditorViewModel {
        public Aircraft Aircraft { get; }

        public DelegateCommand LeaveEditorCommand { get; } = new DelegateCommand(()=>{});
        public RoutePoint SelectedRoutePoint { get; set; }
        public DelegateCommand DeleteSelectedRoutePointCommand { get; } = new DelegateCommand(()=>{});
    }
}