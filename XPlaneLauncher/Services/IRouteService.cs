﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using XPlaneLauncher.Dtos;
using XPlaneLauncher.Model;

namespace XPlaneLauncher.Services {
    public interface IRouteService {
        AircraftRouteOnMap GetRouteLine(Aircraft aircraft);
        double GetRouteLenght(Aircraft aircraft);
    }
}