using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoqSut;

namespace Moq.Test
{
    [TestClass]
    [Ignore]
    public class StreamReaderInstanceTest
    {
        [TestMethod]
        //[Ignore]
        public void StreamReaderMock()
        {
            // Arrange
            var mock = new Mock<StreamReader>();
            // Konfiguration mehrer Rueckgabewerte
            mock.SetupSequence(sr => sr.EndOfStream)
                .Returns(false).Returns(false).Returns(false)
                .Returns(true);
            mock.SetupSequence(sr => sr.ReadLine())
                .Returns("Zeile 1").Returns("Zeile 2").Returns("Zeile 3")
                .Throws<InvalidOperationException>();

            // Act
            var text = StreamReaderInstance.ReadText(mock.Object);

            // Assert
            Assert.IsNotNull(text);
            Assert.AreEqual(3, text.Count);
        }
    }
}
