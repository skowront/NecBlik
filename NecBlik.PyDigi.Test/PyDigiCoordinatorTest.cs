using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using NecBlik.Core.Interfaces;
using NecBlik.PyDigi.Models;

namespace NecBlik.PyDigi.Test
{
    [TestClass]
    public class PyDigiCoordinatorTest
    {
        [TestMethod]
        public async Task DevicesRetrieval()
        {
            var conData = new PyDigiZigBeeUSBCoordinator.PyDigiUSBConnectionData() { baud = 9600, port = "COM4" };
            var coordinator = new PyDigiZigBeeUSBCoordinator(new Factories.PyDigiZigBeeFactory(), conData);
            var devices = new List<IDeviceSource>(await coordinator.GetDevices());
            Assert.AreEqual(devices.Count, 1);

        }
    }
}