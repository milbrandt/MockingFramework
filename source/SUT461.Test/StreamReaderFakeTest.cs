using System.Collections.Generic;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SUT461.Test
{
    [TestClass]
    public class StreamReaderFakeTest
    {
        [TestMethod]
        public void StreamReaderFake()
        {
            using (ShimsContext.Create()) // sonst bleibt die Shims erhalten
            {
                // Arrange
                var lines = new Queue<string>(new List<string> { "Zeile 1", "Zeile 2", "Zeile 3" });

                // shim the StreamReader constructor
                System.IO.Fakes.ShimStreamReader.ConstructorString =
                    (@this, filename) =>
                    {
                    };

                System.IO.Fakes.ShimStreamReader.AllInstances.EndOfStreamGet = sr => lines.Count == 0;
                System.IO.Fakes.ShimStreamReader.AllInstances.ReadLine = sr => lines.Dequeue();

                // Act
                var text = SUT461.StreamReaderFake.ReadText(null);

                // Assert
                Assert.IsNotNull(text);
                Assert.AreEqual(3, text.Count);
            }
        }
    }
}
