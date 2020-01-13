﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using XPlaneLauncher.Dtos;
using XPlaneLauncher.Model;

namespace XPlaneLauncher.Services {
    public interface IRouteService {
        Polyline GetRouteLine(Aircraft aircraft, ObservableCollection<RoutePoint> aircraftRoute);
    }
}