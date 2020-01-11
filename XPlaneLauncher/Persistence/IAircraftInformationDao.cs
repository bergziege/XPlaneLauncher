using System.Collections.Generic;
using System.Threading.Tasks;
using XPlaneLauncher.Domain;

namespace XPlaneLauncher.Persistence {
    public interface IAircraftInformationDao {
        Task<IList<AircraftInformation>> GetAllAsync();
    }
}