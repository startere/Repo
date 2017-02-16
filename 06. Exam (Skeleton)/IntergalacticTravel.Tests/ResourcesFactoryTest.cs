using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace IntergalacticTravel.Tests
{
    [TestFixture]
    public class ResourcesFactoryTest
    {
        [TestCase("create resources gold(20) silver(30) bronze(40)")]
        [TestCase("create resources gold(20) bronze(40) silver(30)")]
        [TestCase("create resources silver(30) bronze(40) gold(20)")]
        [TestCase("create resources silver(30) gold(20) bronze(40)")]
        [TestCase("create resources bronze(40) gold(20) silver(30)")]
        [TestCase("create resources bronze(40) silver(30) gold(20)")]
        public void GetResourcesShouldReturnANewlyCreatedResourceObject(string validCommand)
        {
            //Arrange
            var factory = new ResourcesFactory();

            //Act and capture
            var createdResource = factory.GetResources(validCommand);

            //Assert/check
            Assert.IsInstanceOf<Resources>(createdResource);
        }

        [TestCase("create resources x y z")]
        [TestCase("tansta resources a b")]
        [TestCase("absolutelyRandomStringThatMustNotBeAValidCommand")]
        public void GetResourcesShouldThrowInvalidOperationException(string invalidCommand)
        {
            //Arrange
            var factory = new ResourcesFactory();

            //Act and assert
            var exception = Assert.Throws<InvalidOperationException>(() => factory.GetResources(invalidCommand));
            var exceptionText = exception.Message;
            var stringToLookFor = "command";

            StringAssert.Contains(stringToLookFor, exceptionText);
        }

        [TestCase("create resources silver(10) gold(97853252356623523532) bronze(20)")]
        [TestCase("create resources silver(555555555555555555555555555555555)")]
        //[TestCase("gold(97853252356623523532999999999) bronze(20)")]
        [TestCase("create resources silver(10) gold(20) bronze(4444444444444444444444444444444444444)")]
        public void GetResourcesShouldThrowOverflowExceptionWhenValidCommandButResourceAmountTooMuch(string validCommand)
        {
            //Arrange
            var factory = new ResourcesFactory();

            // and capture
            //var createdResource = factory.GetResources(validCommand);

            //Act and Assert/check
            Assert.Throws<OverflowException>(() => factory.GetResources(validCommand));
        }
    }
}
