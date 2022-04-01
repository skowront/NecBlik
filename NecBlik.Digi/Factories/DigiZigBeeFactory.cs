using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NecBlik.Core.Interfaces;
using NecBlik.Core.Models;
using NecBlik.Virtual.Factories;
using NecBlik.Digi.Models;
using Newtonsoft.Json;
using NecBlik.Common.WpfElements;
using NecBlik.Common.WpfElements.ResponseProviders;
using NecBlik.Common.WpfExtensions.Interfaces;

namespace NecBlik.Digi.Factories
{
    public class DigiZigBeeFactory:VirtualDeviceFactory
    {
        public DigiZigBeeFactory()
        {
            this.internalFactoryType = Resources.Resources.DigiFactoryId;
        }

        public override IDeviceSource BuildNewSource()
        {
            return base.BuildNewSource();
        }

        public override Coordinator BuildCoordinator()
        {
            return new DigiZigBeeUSBCoordinator(this);
        }

        public override async Task<Network> BuildNetworkFromDirectory(string pathToDirectory, IUpdatableResponseProvider<int, bool, string> updatableResponseProvider)
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

        public override IDeviceSource BuildSourceFromJsonFile(string pathToFile)
        {
            return null;
        }
    }
}
