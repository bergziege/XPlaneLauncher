using System.IO;
using XPlaneLauncher.Dtos;

namespace XPlaneLauncher.Services {
    public interface IAcmiService {
        AcmiDto ParseFile(FileInfo acmiFile);
    }
}