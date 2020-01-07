using System.Collections.Generic;
using XPlaneLauncher.Domain;

namespace XPlaneLauncher.Repositories.Impl {
    public class AircraftRepository : IAircraftRepository{
        public IList<Aircraft> Aircrafts { get; } = new List<Aircraft>();
    }
}