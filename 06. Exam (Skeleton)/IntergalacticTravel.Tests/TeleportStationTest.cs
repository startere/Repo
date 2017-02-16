using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Moq;
using IntergalacticTravel.Contracts;
using System.Collections.Generic;
using IntergalacticTravel.Exceptions;

namespace IntergalacticTravel.Tests
{
    [TestFixture]
    public class TeleportStationTest
    {
        [Test]
        public void ConstructorShouldSetAllProvidedFields()
        {
            //Arrange
            var mockedOwner = new Mock<IBusinessOwner>();
            var mockedGalacticMap = new Mock<IEnumerable<IPath>>();
            var mockedLocation = new Mock<ILocation>();
            var expectedOwner = mockedOwner.Object;
            var expectedGalacticMap = mockedGalacticMap.Object;
            var expectedLocation = mockedLocation.Object;

            //Act
            var teleportStation = new ExtendedTeleportStation(expectedOwner, expectedGalacticMap, expectedLocation);
            var actualOwner = teleportStation.Owner;
            var actualGalacticMap = teleportStation.GalacticMap;
            var actualLocation = teleportStation.Location;
           
            //Assert
            Assert.AreEqual(expectedOwner, actualOwner);
            Assert.AreEqual(expectedGalacticMap, actualGalacticMap);
            Assert.AreEqual(expectedLocation, actualLocation);
        }

        [Test]
        public void TeleportUnitShouldThrowArgumentNullExceptionWhenIUnitUnitToTeleportNull()
        {
            //Arrange
            var location = new Mock<ILocation>();
            var objLocation = location.Object;

            var mockedOwner = new Mock<IBusinessOwner>();
            var mockedGalacticMap = new Mock<IEnumerable<IPath>>();
            var mockedLocation = new Mock<ILocation>();
            var objOwner = mockedOwner.Object;
            var objGalacticMap = mockedGalacticMap.Object;
            var objStationLocation = mockedLocation.Object;
            var teleportStation = new ExtendedTeleportStation(objOwner, objGalacticMap, objStationLocation);
            //Act
            var exception = Assert.Throws<ArgumentNullException>(() => teleportStation.TeleportUnit(null, objLocation));
            var actualMessage = exception.Message;
            var expectedSubstr = "unitToTeleport";

            //Assert
            StringAssert.Contains(expectedSubstr, actualMessage);
        }

        [Test]
        public void TeleportUnitWithNullDestinationshouldThrowMessageShouldContainDestination()
        {
            //Arrange
            var unit = new Mock<IUnit>();
            var objUnit = unit.Object;

            var mockedOwner = new Mock<IBusinessOwner>();
            var mockedGalacticMap = new Mock<IEnumerable<IPath>>();
            var mockedLocation = new Mock<ILocation>();
            var objOwner = mockedOwner.Object;
            var objGalacticMap = mockedGalacticMap.Object;
            var objStationLocation = mockedLocation.Object;
            var teleportStation = new ExtendedTeleportStation(objOwner, objGalacticMap, objStationLocation);
            
            //Act
            var exception = Assert.Throws<ArgumentNullException>(() => teleportStation.TeleportUnit(objUnit, null));
            var actualMessage = exception.Message;
            var expectedSubstr = "destination";

            //Assert
            StringAssert.Contains(expectedSubstr, actualMessage);
        }

        [Test]
        public void TeleportUnitShouldThrowTeleportOutOfRangeExceptionWhenUnitTryingToUseTeleportStationFromDistantLoc()
        {
            //Arrange
            var unit = new Mock<IUnit>();
            var objUnit = unit.Object;
            var destinLoc = new Mock<ILocation>();
            var objDestinLoc = destinLoc.Object;

            var mockedOwner = new Mock<IBusinessOwner>();
            var mockedGalacticMap = new Mock<IEnumerable<IPath>>();
            var mockedLocation = new Mock<ILocation>();
            var objOwner = mockedOwner.Object;
            var objGalacticMap = mockedGalacticMap.Object;
            var objStationLocation = mockedLocation.Object;

            unit.Setup(u => u.CurrentLocation.Planet.Name).Returns("Braxis");
            unit.Setup(u => u.CurrentLocation.Planet.Galaxy.Name).Returns("Outer Galaxy");

            mockedLocation.Setup(loc => loc.Planet.Name).Returns("Mar Sara");
            mockedLocation.Setup(loc => loc.Planet.Galaxy.Name).Returns("Inner Galaxy");

            var teleportStation = new ExtendedTeleportStation(objOwner, objGalacticMap, objStationLocation);

            //Act
            
            var exception = Assert.Throws<TeleportOutOfRangeException>(() => teleportStation.TeleportUnit(objUnit, objDestinLoc));
            var actualMessage = exception.Message;
            var expectedSubstr = "unitToTeleport.CurrentLocation";

            //Assert
            StringAssert.Contains(expectedSubstr, actualMessage);
        }

        [Test]
        public void TeleportUnitShouldThrowInvalidTeleportationLocationWithSubstringUnitsWillOverlap()
        {
            //Arrange
            var unit = new Mock<IUnit>();
            var objUnit = unit.Object;

            var unitAtDestin = new Mock<IUnit>();
            var objUnitAtDestin = unitAtDestin.Object;

            var destinLoc = new Mock<ILocation>();

            var mockedOwner = new Mock<IBusinessOwner>();
            //var mockedGalacticMap = new Mock<IEnumerable<IPath>>();
            var mockedStationLoc = new Mock<ILocation>();
            var objOwner = mockedOwner.Object;

            var objStationLocation = mockedStationLoc.Object;

            //unit.Setup(u => u.CurrentLocation.Planet.Name).Returns("Braxis");
            //unit.Setup(u => u.CurrentLocation.Planet.Galaxy.Name).Returns("Outer Galaxy");

            var destinPlanet = "Char Sara";
            var destinGalaxy = "Inner Galaxy";
            var destinLong = 20.00;
            var destinLat = 10.00;

            unitAtDestin.Setup(u => u.CurrentLocation.Planet.Name).Returns(destinPlanet);
            unitAtDestin.Setup(u => u.CurrentLocation.Planet.Galaxy.Name).Returns(destinGalaxy);
            unitAtDestin.Setup(u => u.CurrentLocation.Coordinates.Longtitude).Returns(destinLong);
            unitAtDestin.Setup(u => u.CurrentLocation.Coordinates.Latitude).Returns(destinLat);

            destinLoc.Setup(loc => loc.Planet.Name).Returns(destinPlanet);
            destinLoc.Setup(loc => loc.Planet.Galaxy.Name).Returns(destinGalaxy);
            destinLoc.Setup(loc => loc.Coordinates.Longtitude).Returns(destinLong);
            destinLoc.Setup(loc => loc.Coordinates.Latitude).Returns(destinLat);

            unit.Setup(u => u.CurrentLocation.Planet.Name).Returns(destinPlanet);
            unit.Setup(u => u.CurrentLocation.Planet.Galaxy.Name).Returns(destinGalaxy);
            unit.Setup(u => u.CurrentLocation.Coordinates.Longtitude).Returns(destinLong);
            unit.Setup(u => u.CurrentLocation.Coordinates.Latitude).Returns(destinLat);

            var path = new Mock<IPath>();
            path.Setup(p => p.TargetLocation.Planet.Name).Returns(destinPlanet);
            path.Setup(p => p.TargetLocation.Planet.Galaxy.Name).Returns(destinGalaxy);
            path.Setup(p => p.Cost.BronzeCoins).Returns(10);
            path.Setup(p => p.Cost.GoldCoins).Returns(20);
            path.Setup(p => p.Cost.SilverCoins).Returns(30);

            var destinPlanetUnitCollection = new List<IUnit> { objUnitAtDestin };
            //var objDestinPlanetUnitCollection = destinPlanetUnitCollection.Object;
            path.Setup(p => p.TargetLocation.Planet.Units).Returns(destinPlanetUnitCollection);

            var objPath = path.Object;

            var mockedGalacticMap = new List<IPath>{ objPath };

            var objDestinLoc = destinLoc.Object;

            var teleportStation = new ExtendedTeleportStation(objOwner, mockedGalacticMap, objStationLocation);

            mockedStationLoc.Setup(l => l.Planet.Name).Returns(destinPlanet);
            mockedStationLoc.Setup(l => l.Planet.Galaxy.Name).Returns(destinGalaxy);

            //Act

            var exception = Assert.Throws<InvalidTeleportationLocationException>(() => teleportStation.TeleportUnit(objUnit, objDestinLoc));
            var actualMessage = exception.Message;
            var expectedSubstr = "units will overlap";

            //Assert
            StringAssert.Contains(expectedSubstr, actualMessage);
        }

        [Test]
        public void TeleportUnitShouldThrowLocationNotFoundWhenGalaxyNotFound()
        {
            //Arrange
            var stationOwner = new Mock<IBusinessOwner>();

            var planet = "Mar Sara";
            var galaxy = "Inner Galaxy";
            var longitude = 20.00;
            var latitude = 10.00;

            //var knownTargetLoc = new Mock<ILocation>();
            //knownTargetLoc.Setup(l => l.Planet.Name).Returns(planet);
            //knownTargetLoc.Setup(l => l.Planet.Galaxy.Name).Returns(galaxy);
            //knownTargetLoc.Setup(l => l.Coordinates.Longtitude).Returns(longitude);
            //knownTargetLoc.Setup(l => l.Coordinates.Latitude).Returns(latitude);
            var path = new Mock<IPath>();

            path.Setup(p => p.Cost.BronzeCoins).Returns(10);
            path.Setup(p => p.Cost.GoldCoins).Returns(20);
            path.Setup(p => p.Cost.SilverCoins).Returns(30);
            //path.Setup(p => p.TargetLocation.Coordinates.Longtitude).Returns(longitude);
            //path.Setup(p => p.TargetLocation.Coordinates.Latitude).Returns(latitude);
            path.Setup(p => p.TargetLocation.Planet.Name).Returns(planet);
            path.Setup(p => p.TargetLocation.Planet.Galaxy.Name).Returns("Solar System");

            var stationMap = new List<IPath> { path.Object };

            var stationLoc = new Mock<ILocation>();


            var station = new ExtendedTeleportStation(stationOwner.Object, stationMap, stationLoc.Object);

            var unit = new Mock<IUnit>();
            unit.Setup(u => u.CurrentLocation.Planet.Name).Returns(planet);
            unit.Setup(u => u.CurrentLocation.Planet.Galaxy.Name).Returns(galaxy);
            unit.Setup(u => u.CurrentLocation.Coordinates.Longtitude).Returns(longitude);
            unit.Setup(u => u.CurrentLocation.Coordinates.Latitude).Returns(latitude);

            var destination = new Mock<ILocation>();
            destination.Setup(l => l.Planet.Name).Returns(planet);
            destination.Setup(l => l.Planet.Galaxy.Name).Returns(galaxy);
            destination.Setup(l => l.Coordinates.Latitude).Returns(latitude);
            destination.Setup(l => l.Coordinates.Longtitude).Returns(longitude);

            stationLoc.Setup(l => l.Planet.Name).Returns(planet);
            stationLoc.Setup(l => l.Planet.Galaxy.Name).Returns(galaxy);

            //Act
            var exception = Assert.Throws<LocationNotFoundException>(() => station.TeleportUnit(unit.Object, destination.Object));
            var message = exception.Message;

            var expectedSubstring = "Galaxy";

            //Assert
            StringAssert.Contains(expectedSubstring, message);      
        }
        [Test]
        public void TeleportUnitShouldThrowLocationNotFoundWhenPlanetNotFound()
        {
            //Arrange
            var stationOwner = new Mock<IBusinessOwner>();

            var planet = "Mar Sara";
            var galaxy = "Inner Galaxy";
            var longitude = 20.00;
            var latitude = 10.00;

            var path = new Mock<IPath>();

            path.Setup(p => p.Cost.BronzeCoins).Returns(10);
            path.Setup(p => p.Cost.GoldCoins).Returns(20);
            path.Setup(p => p.Cost.SilverCoins).Returns(30);
            path.Setup(p => p.TargetLocation.Planet.Name).Returns("Chau Sara");
            path.Setup(p => p.TargetLocation.Planet.Galaxy.Name).Returns(galaxy);

            var stationMap = new List<IPath> { path.Object };

            var stationLoc = new Mock<ILocation>();

            var station = new ExtendedTeleportStation(stationOwner.Object, stationMap, stationLoc.Object);

            var unit = new Mock<IUnit>();
            unit.Setup(u => u.CurrentLocation.Planet.Name).Returns(planet);
            unit.Setup(u => u.CurrentLocation.Planet.Galaxy.Name).Returns(galaxy);
            unit.Setup(u => u.CurrentLocation.Coordinates.Longtitude).Returns(longitude);
            unit.Setup(u => u.CurrentLocation.Coordinates.Latitude).Returns(latitude);

            var destination = new Mock<ILocation>();
            destination.Setup(l => l.Planet.Name).Returns(planet);
            destination.Setup(l => l.Planet.Galaxy.Name).Returns(galaxy);
            destination.Setup(l => l.Coordinates.Latitude).Returns(latitude);
            destination.Setup(l => l.Coordinates.Longtitude).Returns(longitude);

            stationLoc.Setup(l => l.Planet.Name).Returns(planet);
            stationLoc.Setup(l => l.Planet.Galaxy.Name).Returns(galaxy);

            //Act
            var exception = Assert.Throws<LocationNotFoundException>(() => station.TeleportUnit(unit.Object, destination.Object));
            var message = exception.Message;

            var expectedSubstring = "Planet";

            //Assert
            StringAssert.Contains(expectedSubstring, message);
        }
        [Test]
        public void TeleportUnitShouldThrowInsufficientResourcesExceptionButServiceCostsMoreThanUnitResources()
        {
            //Arrange

            var planet = "Mar Sara";
            var galaxy = "Inner Galaxy";
            var longitude = 20.00;
            var latitude = 10.00;

            var stationLoc = new Mock<ILocation>();
            stationLoc.Setup(l => l.Planet.Name).Returns(planet);
            stationLoc.Setup(l => l.Planet.Galaxy.Name).Returns(galaxy);
            //stationLoc.Setup(l => l.Coordinates.Longtitude).Returns(longitude);
            //stationLoc.Setup(l => l.Coordinates.Latitude).Returns(latitude);

            var path = new Mock<IPath>();
            path.Setup(p => p.Cost.SilverCoins).Returns(20);
            path.Setup(p => p.Cost.GoldCoins).Returns(20);
            path.Setup(p => p.Cost.BronzeCoins).Returns(20);
            path.Setup(p => p.TargetLocation.Planet.Name).Returns("Chau Sara");
            path.Setup(p => p.TargetLocation.Planet.Galaxy.Name).Returns("Outer Rim");

            var stationmap = new List<IPath> { path.Object };

            var stationOwner = new Mock<IBusinessOwner>();

            var unit = new Mock<IUnit>();
            unit.Setup(u => u.CurrentLocation.Planet.Name).Returns(planet);
            unit.Setup(u => u.CurrentLocation.Planet.Galaxy.Name).Returns(galaxy);
            unit.Setup(u => u.CurrentLocation.Coordinates.Longtitude).Returns(longitude);
            unit.Setup(u => u.CurrentLocation.Coordinates.Latitude).Returns(latitude);
            unit.Setup(u => u.Resources.BronzeCoins).Returns(10);
            unit.Setup(u => u.Resources.GoldCoins).Returns(10);
            unit.Setup(u => u.Resources.SilverCoins).Returns(10);

            var unitsOnPlanet = new List<IUnit> { unit.Object };
            path.Setup(p => p.TargetLocation.Planet.Units).Returns(unitsOnPlanet);

            var destination = new Mock<ILocation>();
            destination.Setup(l => l.Planet.Name).Returns("Chau Sara");
            destination.Setup(l => l.Planet.Galaxy.Name).Returns("Outer Rim");
            destination.Setup(l => l.Coordinates.Longtitude).Returns(100.00);
            destination.Setup(l => l.Coordinates.Latitude).Returns(200.00);

            var substring = "FREE LUNCH";

            var station = new ExtendedTeleportStation(stationOwner.Object, stationmap, stationLoc.Object);

            //Act

            var exception = Assert.Throws<InsufficientResourcesException>(() => station.TeleportUnit(unit.Object, destination.Object));
            var message = exception.Message;

            //Assert

            StringAssert.Contains(substring, message);
        }
        [Test]
        public void TeleportUnitShouldRequirePaymentFromUnitToTeleportForServicesAfterValidationsBeforeTeleport()
        {
            //Arrange

            var planet = "Chau Sara";
            var galaxy = "Inner Galaxy";
            var longitude = 10.00;
            var latitude = 20.00;

            var stationLoc = new Mock<ILocation>();
            stationLoc.Setup(l => l.Planet.Name).Returns(planet);
            stationLoc.Setup(l => l.Planet.Galaxy.Name).Returns(galaxy);

            var stationOwner = new Mock<IBusinessOwner>();

            var path = new Mock<IPath>();
            path.Setup(p => p.Cost.BronzeCoins).Returns(5);
            path.Setup(p => p.Cost.GoldCoins).Returns(5);
            path.Setup(p => p.Cost.SilverCoins).Returns(5);
            path.Setup(p => p.TargetLocation.Planet.Name).Returns(planet);
            path.Setup(p => p.TargetLocation.Planet.Galaxy.Name).Returns(galaxy);

            var destination = new Mock<ILocation>();
            destination.Setup(l => l.Planet.Name).Returns(planet);
            destination.Setup(l => l.Planet.Galaxy.Name).Returns(galaxy);
            destination.Setup(l => l.Coordinates.Latitude).Returns(latitude);
            destination.Setup(l => l.Coordinates.Longtitude).Returns(longitude);

            var unitAtDestination = new Mock<IUnit>();
            unitAtDestination.Setup(u => u.CurrentLocation.Planet.Name).Returns("Chau Sara");
            unitAtDestination.Setup(u => u.CurrentLocation.Planet.Galaxy.Name).Returns("Outer region");
            unitAtDestination.Setup(u => u.CurrentLocation.Coordinates.Longtitude).Returns(longitude);
            unitAtDestination.Setup(u => u.CurrentLocation.Coordinates.Latitude).Returns(latitude);

            var destinationUnitList = new List<IUnit> { unitAtDestination.Object };
            path.Setup(p => p.TargetLocation.Planet.Units).Returns(destinationUnitList);

            var stationMap = new List<IPath> { path.Object };

            var station = new ExtendedTeleportStation(stationOwner.Object, stationMap, stationLoc.Object);

            var unit = new Mock<IUnit>();
            unit.Setup(u => u.CurrentLocation.Planet.Name).Returns(planet);
            unit.Setup(u => u.CurrentLocation.Planet.Galaxy.Name).Returns(galaxy);
            unit.Setup(u => u.CurrentLocation.Coordinates.Longtitude).Returns(longitude);
            unit.Setup(u => u.CurrentLocation.Coordinates.Latitude).Returns(latitude);
            unit.Setup(u => u.Resources.BronzeCoins).Returns(10);
            unit.Setup(u => u.Resources.GoldCoins).Returns(10);
            unit.Setup(u => u.Resources.SilverCoins).Returns(10);
            unit.Setup(u => u.CanPay(It.IsAny<IResources>())).Returns(true);
            unit.Setup(u => u.Pay(path.Object.Cost)).Returns(path.Object.Cost);

            var initialLocUnits = new List<IUnit> { unit.Object };
            unit.Setup(u => u.CurrentLocation.Planet.Units).Returns(initialLocUnits);
            
            //Act

            station.TeleportUnit(unit.Object, destination.Object);

            //Assert
            unit.Verify(u => u.Pay(unit.Object.Pay(path.Object.Cost)), Times.Once());
        }
        [Test]
        public void TeleportUnitShouldObtainAPaymentFromTheUnitToTeleportForTheProvidedService()
        {
            var unit = new Mock<IUnit>();

        }

    }
}
