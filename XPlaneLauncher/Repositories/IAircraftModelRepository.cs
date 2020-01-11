using System.Collections.Generic;
using XPlaneLauncher.Model;

namespace XPlaneLauncher.Repositories {
    public interface IAircraftModelRepository {
        IList<Aircraft> Aircrafts { get; }
    }
}