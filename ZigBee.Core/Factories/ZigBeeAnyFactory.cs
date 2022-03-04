using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Core.Interfaces;
using ZigBee.Core.Models;

namespace ZigBee.Core.Factories
{
    public class ZigBeeAnyFactory:IZigBeeFactory
    {
        private static ZigBeeAnyFactory instance;
        public static ZigBeeAnyFactory Instance
        {
            get
            {
                if(ZigBeeAnyFactory.instance==null)
                {
                    ZigBeeAnyFactory.instance = new ZigBeeAnyFactory();
                    instance.Refresh();
                }
                return ZigBeeAnyFactory.instance;
            }
        }

        private IZigBeeFactory DefaultFactory = new ZigBeeDefaultFactory();

        private List<IZigBeeFactory> Factories = new List<IZigBeeFactory>();

        public ZigBeeAnyFactory()
        {
           
        }

        public void Refresh()
        {
            this.Factories = this.LoadFactories();
        }

        private List<IZigBeeFactory> LoadFactories()
        {
            var factories = new List<IZigBeeFactory>();
            var path = System.IO.Path.GetDirectoryName(System.AppContext.BaseDirectory);
            var dllfiles = Directory.EnumerateFiles(path, "*.dll");
            foreach (var dll in dllfiles)
            {
                if (!dll.Contains("ZigBee"))
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
                    if (typeof(IZigBeeFactory).IsAssignableFrom(type) && !type.IsAbstract)
                    {
                        if(Activator.CreateInstance(type) as ZigBeeDefaultFactory != null || Activator.CreateInstance(type) as ZigBeeAnyFactory != null)
                        {
                            continue;
                        }
                        var module = Activator.CreateInstance(type) as IZigBeeFactory;
                        factories.Add(module);
                    }
                }
            }

            var otherFactories = new List<IZigBeeFactory>();
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
                otherFactories = new List<IZigBeeFactory>();
            }

            return factories;
        }

        public string GetVendorID()
        {
            return "Internal";
        }

        public void AttachOtherFactories(List<IZigBeeFactory> zigBeeFactories)
        {
            return;
        }

        public IZigBeeSource BuildNewSource()
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

        public IZigBeeSource BuildSourceFromJsonFile(string pathToFile)
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

        public ZigBeeCoordinator BuildCoordinator()
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

        public ZigBeeCoordinator BuildCoordinatorFromJsonFile(string pathToFile)
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

        public ZigBeeNetwork BuildNetwork()
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

        public ZigBeeNetwork BuildNetworkFromDirectory(string pathToDirectory)
        {
            foreach (var factory in this.Factories)
            {
                var product = factory.BuildNetworkFromDirectory(pathToDirectory);
                if (product != null)
                {
                    return product;
                }
            }
            return this.DefaultFactory.BuildNetworkFromDirectory(pathToDirectory);
        }
    }
}
