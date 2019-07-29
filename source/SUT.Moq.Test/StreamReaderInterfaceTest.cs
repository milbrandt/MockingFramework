using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace SUT.Moq.Test
{
    [TestClass]
    public class StreamReaderInterfaceTest
    {
        [TestMethod]
        public void StreamReaderInterface()
        {
            // Arrange
            var mock = new Mock<IStreamReader>();
            mock.SetupSequence(sr => sr.EndOfStream)
                .Returns(false).Returns(false).Returns(false)
                .Returns(true);
            mock.SetupSequence(sr => sr.ReadLine())
                .Returns("Zeile 1").Returns("Zeile 2").Returns("Zeile 3")
                .Throws<InvalidOperationException>();

            // Act
            var text = MockWithoutInterface.ReadText(mock.Object);

            // Assert
            Assert.IsNotNull(text);
            Assert.AreEqual(3, text.Count);
        }
    }
}
