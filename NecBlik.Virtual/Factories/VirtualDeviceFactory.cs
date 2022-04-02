using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NecBlik.Common.WpfExtensions.Interfaces;
using NecBlik.Core.Factories;
using NecBlik.Core.Interfaces;
using NecBlik.Core.Models;
using NecBlik.Virtual.Models;

namespace NecBlik.Virtual.Factories
{
    public class VirtualDeviceFactory : DeviceFactory
    {
        public VirtualDeviceFactory()
        {
            this.internalFactoryType = Resources.Resources.VirtualFactoryId;
        }

        public override string GetVendorID()
        {
            return this.internalFactoryType;
        }

        public override IDeviceSource BuildNewSource()
        {
            return new VirtualDevice();
        }

        public override Coordinator BuildCoordinator()
        {
            return new VirtualCoordinator(this);
        }

        public override Coordinator BuildCoordinatorFromJsonFile(string pathToFile)
        {
            var path = Path.GetDirectoryName(pathToFile);
            var fileName = Path.GetFileName(pathToFile);
            var type = fileName.Split('.')[1];
            if (type != this.GetVendorID())
            {
                return null;
            }
            var virtualzb = new VirtualCoordinator(this);
            JsonConvert.PopulateObject(File.ReadAllText(pathToFile), virtualzb);
            return virtualzb;
        }

        public override IDeviceSource BuildSourceFromJsonFile(string pathToFile)
        {
            var path = Path.GetDirectoryName(pathToFile);
            var fileName = Path.GetFileName(pathToFile);
            var type = fileName.Split('.')[1];
            if (type != this.GetVendorID())
            {
                return null;
            }
            var source = new VirtualDevice();
            JsonConvert.PopulateObject(File.ReadAllText(pathToFile), source);
            return source;
        }

        public override async Task<Network> BuildNetworkFromDirectory(string pathToDirectory, IUpdatableResponseProvider<int, bool, string> updatableResponseProvider)
        {
            if (pathToDirectory.Split('.').LastOrDefault() != this.GetVendorID())
            {
                return null;
            }

            var path = Path.GetDirectoryName(pathToDirectory);
            var fileName = Path.GetFileName(pathToDirectory);
            var network = JsonConvert.DeserializeObject<VirtualNetwork>(File.ReadAllText(pathToDirectory + "\\Network.json"));

            var files = new List<string>(Directory.EnumerateFiles(pathToDirectory));
            if (files.Count() < 1)
                return null;
            if (network.HasCoordinator)
            {
                var coordinator = this.BuildCoordinatorFromJsonFile(files[0]);
                if (coordinator == null)
                {
                    foreach (var factory in this.OtherFactories)
                    {
                        coordinator = factory.BuildCoordinatorFromJsonFile(files[0]);
                        if (coordinator != null)
                            break;
                    }
                }
                if (coordinator == null)
                    return null;
                Task.Run(async () => { await network.SetCoordinator(coordinator); }).Wait();
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
            return network;
        }
    }
}
