using System.IO;

namespace XPlaneLauncher.Extensions {
    public static class FileInfoExtension {
        public static string NameWithoutExtension(this FileInfo fileInfo) {
            return fileInfo.Name.Replace(fileInfo.Extension, "");
        }
    }
}