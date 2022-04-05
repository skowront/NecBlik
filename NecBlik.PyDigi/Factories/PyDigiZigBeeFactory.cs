using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NecBlik.Common.WpfExtensions.Interfaces;
using NecBlik.Core.Interfaces;
using NecBlik.Core.Models;
using NecBlik.PyDigi.Models;
using NecBlik.Virtual.Factories;

namespace NecBlik.PyDigi.Factories
{
    public class PyDigiZigBeeFactory:VirtualDeviceFactory
    {
        public PyDigiZigBeeFactory()
        {
            this.internalFactoryType = Resources.Resources.PyDigiFactoryId;
        }

        public override IDeviceSource BuildNewSource()
        {
            return base.BuildNewSource();
        }

        public override Coordinator BuildCoordinator()
        {
            return new PyDigiZigBeeUSBCoordinator(this);
        }

        public override async Task<Network> BuildNetworkFromDirectory(string pathToDirectory, IUpdatableResponseProvider<int, bool, string> updatableResponseProvider)
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

            foreach (var file in Directory.EnumerateFiles(pathToDirectory + "\\Sources"))
            {
                IDeviceSource source = this.BuildSourceFromJsonFile(file);
                if (source == null)
                {
                    foreach (var factory in this.OtherFactories)
                    {
                        source = factory.BuildSourceFromJsonFile(file);
                        if (source != null)
                            break;
                    }
                }
                if (source != null)
                {
                    network.AddSource(source);
                }
            }

            network.ProgressResponseProvider = updatableResponseProvider;
            await network.Initialize(new PyDigiZigBeeUSBCoordinator(this, connectionData));
            return network;
        }

        public override IDeviceSource BuildSourceFromJsonFile(string pathToFile)
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
