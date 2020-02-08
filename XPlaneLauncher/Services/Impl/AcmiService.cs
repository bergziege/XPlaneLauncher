﻿using System;
using System.Globalization;
using System.IO;
using System.Linq;
using MapControl;
using XPlaneLauncher.Dtos;

namespace XPlaneLauncher.Services.Impl {
    public class AcmiService : IAcmiService {
        public AcmiDto ParseFile(FileInfo acmiFile) {
            AcmiDto dto = new AcmiDto();
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
                        lastTimeOffset = line;
                    }else if (line != null) {
                        string[] lineParts = line.Split(',');
                        string transformation = lineParts.FirstOrDefault(x => x.StartsWith("T="));
                        if (transformation != null) {
                            string[] transformationParts = transformation.Replace("T=", "").Split('|');
                            if (HasLocation(transformationParts)) {
                                dto.Track.Add(GetLocation(dto, transformationParts));
                            }
                        }
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(lastTimeOffset)) {
                double.TryParse(lastTimeOffset.Replace("#", ""), out double durationInSeconds);
                if (durationInSeconds > 0) {
                    dto.Duration = TimeSpan.FromSeconds(durationInSeconds);
                }
            }

            return dto;
        }

        private Location GetLocation(AcmiDto dto, string[] transformationParts) {
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

            return new Location(latitude, longitude);
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