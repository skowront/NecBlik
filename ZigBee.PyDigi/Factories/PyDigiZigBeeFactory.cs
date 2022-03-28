using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Common.WpfExtensions.Interfaces;
using ZigBee.Core.Interfaces;
using ZigBee.Core.Models;
using ZigBee.PyDigi.Models;
using ZigBee.Virtual.Factories;

namespace ZigBee.PyDigi.Factories
{
    public class PyDigiZigBeeFactory:VirtualZigBeeFactory
    {
        public PyDigiZigBeeFactory()
        {
            this.internalFactoryType = Resources.Resources.PyDigiFactoryId;
        }

        public override IZigBeeSource BuildNewSource()
        {
            return base.BuildNewSource();
        }

        public override ZigBeeCoordinator BuildCoordinator()
        {
            return new PyDigiZigBeeUSBCoordinator(this);
        }

        public override async Task<ZigBeeNetwork> BuildNetworkFromDirectory(string pathToDirectory, IUpdatableResponseProvider<int, bool, string> updatableResponseProvider)
        {
            if (pathToDirectory.Split('.').LastOrDefault() != this.GetVendorID())
            {
                return null;
            }

            var path = Path.GetDirectoryName(pathToDirectory);
            var fileName = Path.GetFileName(pathToDirectory);
            var connectionData = JsonConvert.DeserializeObject<PyDigiZigBeeUSBCoordinator.PyDigiUSBConnectionData>(File.ReadAllText(pathToDirectory + "\\" + Resources.Resources.CoordinatorFile));
            var network = JsonConvert.DeserializeObject<PyDigiZigBeeNetwork>(File.ReadAllText(pathToDirectory + "\\" + Resources.Resources.NetworkFile));
            if (network == null)
            {
                return null;
            }
            network.ProgressResponseProvider = updatableResponseProvider;
            await network.Initialize(new PyDigiZigBeeUSBCoordinator(this, connectionData));
            return network;
        }

        public override IZigBeeSource BuildSourceFromJsonFile(string pathToFile)
        {
            return null;
        }

        public override void Initalize(object args = null)
        {
            ZigBeePyEnv.Initialize();
            base.Initalize(args);
        }
    }
}
