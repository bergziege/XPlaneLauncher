using System.IO;

namespace XPlaneLauncher.Model {
    public class Thumbnail {
        public Thumbnail(bool hasThumbnail, FileInfo thumbnailFile) {
            HasThumbnail = hasThumbnail;
            ThumbnailFile = thumbnailFile;
        }

        public bool HasThumbnail { get; }
        public FileInfo ThumbnailFile { get; }
    }
}