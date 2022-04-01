using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NecBlik.Common.WpfExtensions.Interfaces;
using NecBlik.Core.Interfaces;
using NecBlik.Core.Models;

namespace NecBlik.Core.Factories
{
    public class DeviceAnyFactory:IDeviceFactory
    {
        private static DeviceAnyFactory instance;
        public static DeviceAnyFactory Instance
        {
            get
            {
                if(DeviceAnyFactory.instance==null)
                {
                    DeviceAnyFactory.instance = new DeviceAnyFactory();
                    instance.Refresh();
                }
                return DeviceAnyFactory.instance;
            }
        }

        private IDeviceFactory DefaultFactory = new DeviceDefaultFactory();

        private List<IDeviceFactory> Factories = new List<IDeviceFactory>();

        public DeviceAnyFactory()
        {
           
        }

        public void Refresh()
        {
            this.Factories = this.LoadFactories();
        }

        private List<IDeviceFactory> LoadFactories()
        {
            var factories = new List<IDeviceFactory>();
            var path = System.IO.Path.GetDirectoryName(System.AppContext.BaseDirectory);
            var dllfiles = Directory.EnumerateFiles(path, "*.dll");
            foreach (var dll in dllfiles)
            {
                if (!dll.Contains("NecBlik"))
                {
                    continue;
                }
                var absPath = Path.Combine(path, dll);
                Assembly assembly;
                try
                {
                    assembly = Assembly.LoadFrom(absPath);
                }
                catch
                {
                    continue;
                }
                foreach (var type in assembly.GetExportedTypes())
                {
                    if (typeof(IDeviceFactory).IsAssignableFrom(type) && !type.IsAbstract)
                    {
                        if(Activator.CreateInstance(type) as DeviceDefaultFactory != null || Activator.CreateInstance(type) as DeviceAnyFactory != null)
                        {
                            continue;
                        }
                        var module = Activator.CreateInstance(type) as IDeviceFactory;
                        module.Initalize();
                        factories.Add(module);
                    }
                }
            }

            var otherFactories = new List<IDeviceFactory>();
            foreach(var factory in factories)
            {
                foreach(var otherFactory in factories)
                {
                    if(factory != otherFactory)
                    {
                        otherFactories.Add(otherFactory);
                    }
                }
                factory.AttachOtherFactories(otherFactories);
                otherFactories = new List<IDeviceFactory>();
            }

            return factories;
        }

        public string GetVendorID()
        {
            return Resources.Resources.AnyFactoryId;
        }

        public void AttachOtherFactories(List<IDeviceFactory> deviceFactories)
        {
            return;
        }

        public IDeviceSource BuildNewSource()
        {
            foreach (var factory in this.Factories)
            {
                var product = factory.BuildNewSource();
                if(product !=null)
                {
                    return product;
                }
            }
            return this.DefaultFactory.BuildNewSource();
        }

        public IDeviceSource BuildSourceFromJsonFile(string pathToFile)
        {
            foreach (var factory in this.Factories)
            {
                var product = factory.BuildSourceFromJsonFile(pathToFile);
                if (product != null)
                {
                    return product;
                }
            }
            return this.DefaultFactory.BuildSourceFromJsonFile(pathToFile);
        }

        public Coordinator BuildCoordinator()
        {
            foreach (var factory in this.Factories)
            {
                var product = factory.BuildCoordinator();
                if (product != null)
                {
                    return product;
                }
            }
            return this.DefaultFactory.BuildCoordinator();
        }

        public Coordinator BuildCoordinatorFromJsonFile(string pathToFile)
        {
            foreach (var factory in this.Factories)
            {
                var product = factory.BuildCoordinatorFromJsonFile(pathToFile);
                if (product != null)
                {
                    return product;
                }
            }
            return this.DefaultFactory.BuildCoordinatorFromJsonFile(pathToFile);
        }

        public Network BuildNetwork()
        {
            foreach (var factory in this.Factories)
            {
                var product = factory.BuildNetwork();
                if (product != null)
                {
                    return product;
                }
            }
            return this.DefaultFactory.BuildNetwork();
        }

        public async Task<Network> BuildNetworkFromDirectory(string pathToDirectory, IUpdatableResponseProvider<int, bool, string> updatableResponseProvider)
        {
            foreach (var factory in this.Factories)
            {
                var product = await factory.BuildNetworkFromDirectory(pathToDirectory, updatableResponseProvider);
                if (product != null)
                {
                    return product;
                }
            }
            return await this.DefaultFactory.BuildNetworkFromDirectory(pathToDirectory, updatableResponseProvider);
        }

        public void Initalize(object args = null)
        {
            
        }
    }
}
