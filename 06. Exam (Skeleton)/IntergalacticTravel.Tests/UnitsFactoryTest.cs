using IntergalacticTravel.Exceptions;
using NUnit.Framework;

namespace IntergalacticTravel.Tests
{
    [TestFixture]
    public class UnitsFactoryTest
    {
        [Test]
        public void GetUnitShouldReturnNewProcyonUnit()
        {
            //Arrange
            var command = "create unit Procyon Gosho 1";
            var factory = new UnitsFactory();

            //Act and capture
            var createdUnit = factory.GetUnit(command);

            //Assert/check
            Assert.IsInstanceOf<Procyon>(createdUnit);
        }

        [Test]
        public void GetUnitShouldReturnNewLuytenUnit()
        {
            //Arrange
            var command = "create unit Luyten Pesho 2";
            var factory = new UnitsFactory();

            //Act and capture
            var createdUnit = factory.GetUnit(command);

            //Assert/check
            Assert.IsInstanceOf<Luyten>(createdUnit);
        }

        [Test]
        public void GetUnitShouldReturnNewLacailleUnit()
        {
            //Arrange
            var command = "create unit Lacaille Pesho 2";
            var factory = new UnitsFactory();

            //Act and capture
            var createdUnit = factory.GetUnit(command);

            //Assert/check
            Assert.IsInstanceOf<Lacaille>(createdUnit);
        }

        [TestCase("create unit invalidType Name 1")]
        [TestCase("create unit Lutyen Name InvalidId")]
        public void GetUnitShouldThrowInvalidCommandException(string invalidCommand)
        {
            //Arrange
            var factory = new UnitsFactory();

            //Act and capture
            //var createdUnit = factory.GetUnit(invalidCommand);

            //Assert/check
            Assert.Throws<InvalidUnitCreationCommandException>(() => factory.GetUnit(invalidCommand));
        }
    }
}
