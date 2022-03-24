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
using ZigBee.Common.WpfElements;
using ZigBee.Common.WpfElements.ResponseProviders;
using ZigBee.Common.WpfExtensions.Interfaces;

namespace ZigBee.Digi.Factories
{
    public class DigiZigBeeFactory:VirtualZigBeeFactory
    {
        public DigiZigBeeFactory()
        {
            this.internalFactoryType = Resources.Resources.DigiFactoryId;
        }

        public override IZigBeeSource BuildNewSource()
        {
            return base.BuildNewSource();
        }

        public override ZigBeeCoordinator BuildCoordinator()
        {
            return new DigiZigBeeUSBCoordinator(this);
        }

        public override async Task<ZigBeeNetwork> BuildNetworkFromDirectory(string pathToDirectory, IUpdatableResponseProvider<int, bool, string> updatableResponseProvider)
        {
            if (pathToDirectory.Split('.').LastOrDefault() != this.GetVendorID())
            {
                return null;
            }

            var path = Path.GetDirectoryName(pathToDirectory);
            var fileName = Path.GetFileName(pathToDirectory);
            var connectionData = JsonConvert.DeserializeObject<DigiZigBeeUSBCoordinator.DigiUSBConnectionData>(File.ReadAllText(pathToDirectory + "\\"+Resources.Resources.CoordinatorFile));
            var network = JsonConvert.DeserializeObject<DigiZigBeeNetwork>(File.ReadAllText(pathToDirectory + "\\"+Resources.Resources.NetworkFile));
            if (network == null)
            {
                return null;
            }
            network.ProgressResponseProvider = updatableResponseProvider;
            await network.Initialize(new DigiZigBeeUSBCoordinator(this, connectionData));
            return network;
        }

        public override IZigBeeSource BuildSourceFromJsonFile(string pathToFile)
        {
            return null;
        }
    }
}
