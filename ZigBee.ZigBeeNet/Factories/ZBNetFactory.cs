using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Common.WpfExtensions.Interfaces;
using ZigBee.Core.Interfaces;
using ZigBee.Core.Models;
using ZigBee.Virtual.Factories;
using ZigBee.ZigBeeNet.Models;

namespace ZigBee.ZigBeeNet.Factories
{
    public class ZBNetFactory:VirtualZigBeeFactory
    {
        public ZBNetFactory()
        {
            this.internalFactoryType = "ZigBeeNet";
        }

        public override IZigBeeSource BuildNewSource()
        {
            return base.BuildNewSource();
        }

        public override ZigBeeCoordinator BuildCoordinator()
        {
            return new ZBNetCoordinator(this,null);
        }

        public override async Task<ZigBeeNetwork> BuildNetworkFromDirectory(string pathToDirectory, IUpdatableResponseProvider<int, bool, string> updatableResponseProvider)
        {
            if (pathToDirectory.Split('.').LastOrDefault() != this.GetVendorID())
            {
                return null;
            }

            var path = Path.GetDirectoryName(pathToDirectory);
            var fileName = Path.GetFileName(pathToDirectory);
            var connectionData = JsonConvert.DeserializeObject<ZBNetCoordinator.ZBNetConnectionData>(File.ReadAllText(pathToDirectory + "\\Coordinator.json"));
            var network = JsonConvert.DeserializeObject<ZBNetNetwork>(File.ReadAllText(pathToDirectory + "\\Network.json"));
            if (network == null)
            {
                return null;
            }
            network.ProgressResponseProvider = updatableResponseProvider;
            await network.Initialize(new ZBNetCoordinator(this, connectionData));
            return network;
        }

        public override IZigBeeSource BuildSourceFromJsonFile(string pathToFile)
        {
            return null;
        }
    }
}
