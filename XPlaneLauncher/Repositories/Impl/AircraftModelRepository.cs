using System.Collections.Generic;
using XPlaneLauncher.Model;

namespace XPlaneLauncher.Repositories.Impl {
    public class AircraftModelRepository : IAircraftModelRepository {
        public IList<Aircraft> Aircrafts { get; } = new List<Aircraft>();
    }
}