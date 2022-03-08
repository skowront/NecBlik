using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Core.Factories;
using ZigBee.Core.Interfaces;
using ZigBee.Core.Models;
using ZigBee.Virtual.Models;

namespace ZigBee.Virtual.Factories
{
    public class VirtualZigBeeFactory : ZigBeeFactory
    {
        public VirtualZigBeeFactory()
        {
            this.internalFactoryType = "Virtual";
        }

        public override string GetVendorID()
        {
            return this.internalFactoryType;
        }

        public override IZigBeeSource BuildNewSource()
        {
            return new VirtualZigBeeSource();
        }

        public override ZigBeeCoordinator BuildCoordinator()
        {
            return new VirtualZigBeeCoordinator(this);
        }

        public override ZigBeeCoordinator BuildCoordinatorFromJsonFile(string pathToFile)
        {
            var path = Path.GetDirectoryName(pathToFile);
            var fileName = Path.GetFileName(pathToFile);
            var type = fileName.Split('.')[1];
            if (type != this.GetVendorID())
            {
                return null;
            }
            else
            {
                var virtualzb = new VirtualZigBeeCoordinator(this);
                JsonConvert.PopulateObject(File.ReadAllText(pathToFile), virtualzb);
                return virtualzb;
            }
        }

        public override IZigBeeSource BuildSourceFromJsonFile(string pathToFile)
        {
            var path = Path.GetDirectoryName(pathToFile);
            var fileName = Path.GetFileName(pathToFile);
            var type = fileName.Split('.')[1];
            if (type != this.GetVendorID())
            {
                return null;
            }
            else
            {
                var source = new VirtualZigBeeSource();
                JsonConvert.PopulateObject(File.ReadAllText(pathToFile), source);
                return source;
            }
        }

        public override ZigBeeNetwork BuildNetworkFromDirectory(string pathToDirectory)
        {
            if (pathToDirectory.Split('.').LastOrDefault() != this.GetVendorID())
            {
                return null;
            }

            var path = Path.GetDirectoryName(pathToDirectory);
            var fileName = Path.GetFileName(pathToDirectory);
            var network = JsonConvert.DeserializeObject<VirtualZigBeeNetwork>(File.ReadAllText(pathToDirectory + "\\Network.json"));

            var files = new List<string>(Directory.EnumerateFiles(pathToDirectory));
            if (files.Count() < 1)
                return null;
            if (network.HasCoordinator)
            {
                var zigBeeCoordinator = this.BuildCoordinatorFromJsonFile(files[0]);
                if (zigBeeCoordinator == null)
                {
                    foreach (var factory in this.OtherFactories)
                    {
                        zigBeeCoordinator = factory.BuildCoordinatorFromJsonFile(files[0]);
                        if (zigBeeCoordinator != null)
                            break;
                    }
                }
                if (zigBeeCoordinator == null)
                    return null;
                network.SetCoordinator(zigBeeCoordinator);
            }
            foreach (var file in Directory.EnumerateFiles(pathToDirectory + "\\Sources"))
            {
                IZigBeeSource source = this.BuildSourceFromJsonFile(file);
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
