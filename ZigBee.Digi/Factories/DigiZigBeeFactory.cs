using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Core.Interfaces;
using ZigBee.Core.Models;
using ZigBee.Virtual.Factories;
using ZigBee.Digi.Models;
using Newtonsoft.Json;

namespace ZigBee.Digi.Factories
{
    public class DigiZigBeeFactory:VirtualZigBeeFactory
    {
        public DigiZigBeeFactory()
        {
            this.internalFactoryType = "Digi";
        }

        public override IZigBeeSource BuildNewSource()
        {
            return base.BuildNewSource();
        }

        public override ZigBeeCoordinator BuildCoordinator()
        {
            return new DigiZigBeeUSBCoordinator(this);
        }

        public override ZigBeeNetwork BuildNetworkFromDirectory(string pathToDirectory)
        {
            if (pathToDirectory.Split('.').LastOrDefault() != this.GetVendorID())
            {
                return null;
            }

            var path = Path.GetDirectoryName(pathToDirectory);
            var fileName = Path.GetFileName(pathToDirectory);
            var connectionData = JsonConvert.DeserializeObject<DigiZigBeeUSBCoordinator.DigiUSBConnectionData>(File.ReadAllText(pathToDirectory + "\\Coordinator.json"));
            var network = JsonConvert.DeserializeObject<DigiZigBeeNetwork>(File.ReadAllText(pathToDirectory + "\\Network.json"));
            network?.SetCoordinator(new DigiZigBeeUSBCoordinator(this, connectionData));
            return network;
        }

        public override IZigBeeSource BuildSourceFromJsonFile(string pathToFile)
        {
            return null;
        }
    }
}
