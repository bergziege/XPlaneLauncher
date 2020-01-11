using System.IO;

namespace XPlaneLauncher.Model {
    public class Thumbnail {
        public Thumbnail(FileInfo thumbnailFile) {
            ThumbnailFile = thumbnailFile;
        }

        public FileInfo ThumbnailFile { get; }
    }
}