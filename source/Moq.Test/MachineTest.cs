using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MoqSut.Test
{
    [TestClass]
    public class MachineTest
    {
        [TestMethod]
        public void Setup_Verify()
        {
            // Mock Objekt instanziieren
            var wc = new Mock<IWheelChanger>();

            // Alternativ: Strict Mock instanziieren (alle verwendeten Methoden muessen gemockt sein)
            //var wc = new Mock<IWheelChanger>(MockBehavior.Strict);

            // Mock Property
            wc.Setup(w => w.NumberOfPorts).Returns(4);

            // SUT instanziieren
            var mc = new Machine(wc.Object);

            // Act
            mc.ExchangeWheel(3);

            // OpenDoor genau einmal aufgerufen
            wc.Verify(w => w.OpenDoor(), Times.Once);
        }

        [TestMethod]
        public void RecursiveMock()
        {
            // kein separater Mock für WheelChanger erforderlich
            var mc = new Mock<IMachine>();
            mc.Setup(m => m.WheelChanger.NumberOfPorts).Returns(3);

            // Act
            Assert.AreEqual(3, mc.Object.WheelChanger.NumberOfPorts);
        }

        [TestMethod]
        public void OutParams()
        {
            var wc = new Mock<IWheelChanger>();
            string wpName = "Scheibensatz";
            // beliebiger int-Parameter, out-Parameter zuvor definierter Rueckgabewert
            wc.Setup(w => w.GetWheelPackName(It.IsAny<int>(), out wpName));

            wc.Object.GetWheelPackName(3, out var actual);
            Assert.AreEqual(wpName, actual, false, CultureInfo.InvariantCulture);
        }

        [TestMethod]
        public void Diverses()
        {
            var wc = new Mock<IWheelChanger>();
            // Mock throwing Exception
            wc.Setup(w => w.IsDoorClosed).Throws(new InvalidOperationException("no door availabale"));
            // Match parameter in Range, Zugriff auf Parameter fuer Rueckgabewert
            wc.Setup(w => w.GetMaxDiameterOfWheelPack(It.IsInRange<int>(1, 12, Range.Exclusive))).Returns((int p) => 50.0 + 2.54 * p);

            //-Stub Property (Werte werden gespeichert)----------------
            var wp = new Mock<IWheelPack>();
            // ohne Property-Stubs
            wp.Object.NumberOfWheels = 3;
            Assert.AreEqual(default(int), wp.Object.NumberOfWheels);

            wp.SetupProperty(w => w.Name);
            // alle Properties automatisch tracken
            wp.SetupAllProperties();

            wp.Object.NumberOfWheels = 3;
            Assert.AreEqual(3, wp.Object.NumberOfWheels);

            // Verify fuer Properties
            wp.VerifySet(w => w.NumberOfWheels = 3, "Number of wheels was set");
            var dummy = wp.Object.Name;
            wp.VerifyGet(w => w.Name, Times.AtLeast(1));

            //-Callbacks-----------------------------------------------
            var evenPortsUsed = new List<int>();
            // nur fuer geradzahlige Argumente
            wc.Setup(w => w.ExchangeNewWheelFromPort(It.Is<int>(i => i % 2 == 0)))
                // fuehre Callback aus vor dem Methodenaufruf
                .Callback((int p) => evenPortsUsed.Add(p))
                // Rueckgabewert abhaengig vom (geradzahligen) Argument
                .Returns((int p) => p <= evenPortsUsed.Count)
                // fuehre Callback nach dem Methodenaufruf aus
                .Callback((int p) => evenPortsUsed.Add(100 + p));

            // Rueckgabewert 4 <= 1, d.h. false
            Assert.IsFalse(wc.Object.ExchangeNewWheelFromPort(4));
            Assert.AreEqual(2, evenPortsUsed.Count);

            // kein Mock, da ungerade
            Assert.IsFalse(wc.Object.ExchangeNewWheelFromPort(1));
            Assert.AreEqual(2, evenPortsUsed.Count);

            // Rueckgabewert 2 <= 3, d.h. true
            Assert.IsTrue(wc.Object.ExchangeNewWheelFromPort(2));
            Assert.AreEqual(4, evenPortsUsed.Count);
        }

        [TestMethod]
        public void MultipleInterfaces()
        {
            var wc = new Mock<IWheelChanger>();
            wc.Setup(m => m.IsDoorClosed).Returns(true);
            // Mock soll gleichzeitig weiteres Interface darstellen
            var wp = wc.As<IWheelPack>().Setup(w => w.NumberOfWheels).Returns(3);

            Assert.AreEqual(3, ((IWheelPack)(wc.Object)).NumberOfWheels);
        }
    }
}
