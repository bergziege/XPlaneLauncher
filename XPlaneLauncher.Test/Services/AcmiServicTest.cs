using System;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using XPlaneLauncher.Dtos;
using XPlaneLauncher.Services;
using XPlaneLauncher.Services.Impl;

namespace XPlaneLauncher.Test.Services {
    public class AcmiServiceTest {
        /// <summary>
        /// </summary>
        [Test]
        public void TestParsedLocations() {
            // Given: 
            FileInfo acmiFile = new FileInfo("../../TestData/Tacview-20190924-225048-XPL.txt.acmi");
            IAcmiService acmiService = new AcmiService();

            // When: 
            AcmiDto acmiContent = acmiService.ParseFile(acmiFile);

            // Then: 

            acmiContent.Track.Count.Should().Be(12);

            acmiContent.Track[0].Latitude.Should().Be(39.184346);
            acmiContent.Track[0].Longitude.Should().Be(-76.6584197);

            acmiContent.Track[1].Latitude.Should().Be(39.184346);
            acmiContent.Track[1].Longitude.Should().Be(-76.6584196);

            acmiContent.Track[2].Latitude.Should().Be(39.1843459);
            acmiContent.Track[2].Longitude.Should().Be(-76.6584196);

            acmiContent.Track[3].Latitude.Should().Be(39.1843459);
            acmiContent.Track[3].Longitude.Should().Be(-76.6584197);

            acmiContent.Track[4].Latitude.Should().Be(39.1843458);
            acmiContent.Track[4].Longitude.Should().Be(-76.6584197);

            acmiContent.Track[5].Latitude.Should().Be(39.1843457);
            acmiContent.Track[5].Longitude.Should().Be(-76.6584198);

            acmiContent.Track[6].Latitude.Should().Be(39.1843457);
            acmiContent.Track[6].Longitude.Should().Be(-76.6584199);

            acmiContent.Track[7].Latitude.Should().Be(39.1843456);
            acmiContent.Track[7].Longitude.Should().Be(-76.6584201);

            acmiContent.Track[8].Latitude.Should().Be(39.1843455);
            acmiContent.Track[8].Longitude.Should().Be(-76.6584201);

            acmiContent.Track[9].Latitude.Should().Be(39.184345300000004);
            acmiContent.Track[9].Longitude.Should().Be(-76.6584202);

            acmiContent.Track[10].Latitude.Should().Be(39.184345300000004);
            acmiContent.Track[10].Longitude.Should().Be(-76.6584203);

            acmiContent.Track[11].Latitude.Should().Be(39.1843452);
            acmiContent.Track[11].Longitude.Should().Be(-76.6584203);
        }

        /// <summary>
        /// </summary>
        [Test]
        public void TestParsedRecordingTime() {
            // Given: 
            FileInfo acmiFile = new FileInfo("../../TestData/Tacview-20190924-225048-XPL.txt.acmi");
            IAcmiService acmiService = new AcmiService();

            // When: 
            AcmiDto acmiContent = acmiService.ParseFile(acmiFile);

            // Then: 
            acmiContent.RecordingTime.Should().Be(new DateTime(2019, 9, 24, 20, 50, 48, DateTimeKind.Utc).AddMilliseconds(772));
        }

        /// <summary>
        /// </summary>
        [Test]
        public void TestParsedReferenceLat() {
            // Given: 
            FileInfo acmiFile = new FileInfo("../../TestData/Tacview-20190924-225048-XPL.txt.acmi");
            IAcmiService acmiService = new AcmiService();

            // When: 
            AcmiDto acmiContent = acmiService.ParseFile(acmiFile);

            // Then: 
            acmiContent.ReferenceLatitude.Should().Be(34);
        }

        /// <summary>
        /// </summary>
        [Test]
        public void TestParsedReferenceLon() {
            // Given: 
            FileInfo acmiFile = new FileInfo("../../TestData/Tacview-20190924-225048-XPL.txt.acmi");
            IAcmiService acmiService = new AcmiService();

            // When: 
            AcmiDto acmiContent = acmiService.ParseFile(acmiFile);

            // Then: 
            acmiContent.ReferenceLongitude.Should().Be(-82);
        }

        /// <summary>
        /// </summary>
        [Test]
        public void TestParsedReferenceTime() {
            // Given: 
            FileInfo acmiFile = new FileInfo("../../TestData/Tacview-20190924-225048-XPL.txt.acmi");
            IAcmiService acmiService = new AcmiService();

            // When: 
            AcmiDto acmiContent = acmiService.ParseFile(acmiFile);

            // Then: 
            acmiContent.ReferenceTime.Should().Be(new DateTime(2017, 7, 24, 22, 02, 04, DateTimeKind.Utc));
        }

        /// <summary>
        /// </summary>
        [Test]
        public void TestDuration() {
            // Given: 
            FileInfo acmiFile = new FileInfo("../../TestData/Tacview-20190924-225048-XPL.txt.acmi");
            IAcmiService acmiService = new AcmiService();

            // When: 
            AcmiDto acmiContent = acmiService.ParseFile(acmiFile);

            // Then: 
            acmiContent.Duration.Should().Be(new TimeSpan(0,0,3,12,180));
        }
    }
}