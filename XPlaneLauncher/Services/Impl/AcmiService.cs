using System;
using System.Globalization;
using System.IO;
using System.Linq;
using XPlaneLauncher.Dtos;
using XPlaneLauncher.Model;

namespace XPlaneLauncher.Services.Impl {
    public class AcmiService : IAcmiService {
        public AcmiDto ParseFile(FileInfo acmiFile) {
            AcmiDto dto = new AcmiDto();
            string firstTimeOffset = null;
            string lastTimeOffset = null;
            using (StreamReader reader = File.OpenText(acmiFile.FullName)) {
                while (!reader.EndOfStream) {
                    string line = reader.ReadLine();
                    if (line != null && line.StartsWith("0")) {
                        string[] lineParts = line.Replace("0,", "").Split('=');
                        switch (lineParts[0]) {
                            case "ReferenceTime":
                                dto.ReferenceTime = ParseToDateTime(lineParts[1]);
                                break;
                            case "RecordingTime":
                                dto.RecordingTime = ParseToDateTime(lineParts[1]);
                                break;
                            case "ReferenceLongitude":
                                dto.ReferenceLongitude = double.Parse(lineParts[1]);
                                break;
                            case "ReferenceLatitude":
                                dto.ReferenceLatitude = double.Parse(lineParts[1]);
                                break;
                        }
                    } else if (line != null && line.StartsWith("#")) {
                        if (firstTimeOffset == null) {
                            firstTimeOffset = line;
                        }

                        lastTimeOffset = line;
                    } else if (line != null) {
                        string[] lineParts = line.Split(',');
                        string transformation = lineParts.FirstOrDefault(x => x.StartsWith("T="));
                        if (transformation != null) {
                            string[] transformationParts = transformation.Replace("T=", "").Split('|');
                            if (HasLocation(transformationParts)) {
                                TimeSpan? durationToThisLocation = GetDurationOrDefault(firstTimeOffset, lastTimeOffset);
                                DateTime timestamp = dto.ReferenceTime;
                                if (durationToThisLocation.HasValue) {
                                    timestamp.Add(durationToThisLocation.Value);
                                }

                                dto.Track.Add(GetLocation(dto, transformationParts, timestamp));
                            }
                        }
                    }
                }
            }

            TimeSpan? duration = GetDurationOrDefault(firstTimeOffset, lastTimeOffset);

            if (duration.HasValue) {
                dto.Duration = duration.Value;
            }

            return dto;
        }

        private TimeSpan? GetDurationOrDefault(string firstTimeOffset, string lastTimeOffset) {
            if (!string.IsNullOrWhiteSpace(firstTimeOffset) && !string.IsNullOrWhiteSpace(lastTimeOffset)) {
                double.TryParse(
                    firstTimeOffset.Replace("#", ""),
                    NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture,
                    out double firstTimeOffsetSeconds);
                double.TryParse(
                    lastTimeOffset.Replace("#", ""),
                    NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture,
                    out double lastTimeOffsetSeconds);
                double durationSeconds = lastTimeOffsetSeconds - firstTimeOffsetSeconds;
                if (durationSeconds > 0) {
                    return TimeSpan.FromSeconds(durationSeconds);
                }
            }

            return null;
        }

        private LogbookLocation GetLocation(AcmiDto dto, string[] transformationParts, DateTime timestamp) {
            double.TryParse(transformationParts[0], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double lonOffset);
            double longitude;
            if (lonOffset == 0 && dto.Track.Any()) {
                longitude = dto.Track.Last().Longitude;
            } else if (lonOffset == 0 && !dto.Track.Any()) {
                longitude = dto.ReferenceLongitude;
            } else {
                longitude = dto.ReferenceLongitude + lonOffset;
            }

            double.TryParse(transformationParts[1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double latOffset);
            double latitude;
            if (latOffset == 0 && dto.Track.Any()) {
                latitude = dto.Track.Last().Latitude;
            } else if (latOffset == 0 && !dto.Track.Any()) {
                latitude = dto.ReferenceLatitude;
            } else {
                latitude = dto.ReferenceLatitude + latOffset;
            }

            return new LogbookLocation(timestamp, latitude, longitude);
        }

        private bool HasLocation(string[] transformationParts) {
            return !string.IsNullOrWhiteSpace(transformationParts[0]) ||
                   !string.IsNullOrWhiteSpace(transformationParts[1]);
        }

        private DateTime ParseToDateTime(string dateTime) {
            return DateTime.Parse(dateTime, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
        }
    }
}