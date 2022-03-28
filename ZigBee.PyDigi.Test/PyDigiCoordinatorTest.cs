using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZigBee.Core.Interfaces;
using ZigBee.PyDigi.Models;

namespace ZigBee.PyDigi.Test
{
    [TestClass]
    public class PyDigiCoordinatorTest
    {
        [TestMethod]
        public async Task DevicesRetrieval()
        {
            var conData = new PyDigiZigBeeUSBCoordinator.PyDigiUSBConnectionData() { baud = 9600, port = "COM4" };
            var coordinator = new PyDigiZigBeeUSBCoordinator(new Factories.PyDigiZigBeeFactory(), conData);
            var devices = new List<IZigBeeSource>(await coordinator.GetDevices());
            Assert.AreEqual(devices.Count, 1);

        }
    }
}