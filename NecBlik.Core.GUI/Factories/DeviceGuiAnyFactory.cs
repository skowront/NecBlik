using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NecBlik.Core.GUI.Interfaces;
using NecBlik.Core.GUI.ViewModels;
using NecBlik.Core.GUI.Views.Controls;
using NecBlik.Core.Models;

namespace NecBlik.Core.GUI.Factories
{
    public class DeviceGuiAnyFactory: IDeviceGuiFactory
    { 

        private static DeviceGuiAnyFactory instance;
        public static DeviceGuiAnyFactory Instance
        {
            get
            {
                if (DeviceGuiAnyFactory.instance == null)
                {
                    DeviceGuiAnyFactory.instance = new DeviceGuiAnyFactory();
                    instance.Refresh();
                }
                return DeviceGuiAnyFactory.instance;
            }
        }

        private IDeviceGuiFactory DefaultFactory = new DeviceGuiDefaultFactory();

        private List<IDeviceGuiFactory> Factories = new List<IDeviceGuiFactory>();
        public IDeviceGuiFactory GetFactory(string factoryId)
        {
            foreach (var item in this.Factories)
            {
                if (item.GetVendorID() == factoryId)
                {
                    return item;
                }
            }
            return null;
        }

        public List<string> GetFactoryIds()
        {
            List<string> list = new List<string>();
            foreach (var item in this.Factories)
            {
                list.Add(item.GetVendorID());
            }
            return list;
        }
        public DeviceGuiAnyFactory()
        {
            
        }

        public void Refresh()
        {
            this.Factories = this.LoadFactories();
        }

        private List<IDeviceGuiFactory> LoadFactories()
        {
            var factories = new List<IDeviceGuiFactory>();
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
                    if (typeof(IDeviceGuiFactory).IsAssignableFrom(type) && !type.IsAbstract)
                    {
                        if (Activator.CreateInstance(type) as DeviceGuiDefaultFactory != null || Activator.CreateInstance(type) as DeviceGuiAnyFactory != null)
                        {
                            continue;
                        }
                        var module = Activator.CreateInstance(type) as IDeviceGuiFactory;
                        module.Initalize();
                        factories.Add(module);
                    }
                }
            }

            var otherFactories = new List<IDeviceGuiFactory>();
            foreach (var factory in factories)
            {
                foreach (var otherFactory in factories)
                {
                    if (factory != otherFactory)
                    {
                        otherFactories.Add(otherFactory);
                    }
                }
                factory.AttachOtherFactories(otherFactories);
                otherFactories = new List<IDeviceGuiFactory>();
            }

            return factories;
        }

        public void AttachOtherFactories(List<IDeviceGuiFactory> deviceFactories)
        {
            return;
        }

        public DataTemplate GetNetworkDataTemplate(Network deviceNetwork)
        {
            foreach(var item in this.Factories)
            {
                var template = item.GetNetworkDataTemplate(deviceNetwork);
                if(template != null)
                {
                    return template;
                }
            }
            return this.DefaultFactory.GetNetworkDataTemplate(deviceNetwork);
        }

        public DataTemplate GetNetworkBriefDataTemplate(Network network)
        {
            foreach (var item in this.Factories)
            {
                var template = item.GetNetworkBriefDataTemplate(network);
                if (template != null)
                {
                    return template;
                }
            }
            return this.DefaultFactory.GetNetworkBriefDataTemplate(network);
        }

        public virtual NetworkViewModel GetNetworkViewModel(Network network)
        {
            foreach(var item in this.Factories)
            {
                var r = item.GetNetworkViewModel(network);
                if(r !=null)
                {
                    return r;
                }
            }
            return this.DefaultFactory.GetNetworkViewModel(network);
        }

        public virtual async Task<NetworkViewModel> NetworkViewModelFromWizard(Network network)
        {
            foreach (var item in this.Factories)
            {
                var r = item.NetworkViewModelFromWizard(network);
                if (r != null)
                {
                    return await r;
                }
            }
            return await this.DefaultFactory.NetworkViewModelFromWizard(network);
        }

        public virtual async Task<NetworkViewModel> NetworkViewModelFromWizard(Network network,string FactoryID)
        {
            var factory = this.Factories.Where((o) => { return o.GetVendorID() == FactoryID; }).FirstOrDefault(this.DefaultFactory);
            return await factory.NetworkViewModelFromWizard(network);
        }

        public string GetVendorID()
        {
            return NecBlik.Core.Resources.Resources.AnyFactoryId;
        }

        public UIElement GetDeviceControl(DeviceViewModel deviceViewModel)
        {
            foreach (var item in this.Factories)
            {
                var r = item.GetDeviceControl(deviceViewModel);
                if (r != null)
                {
                    return r;
                }
            }
            return this.DefaultFactory.GetDeviceControl(deviceViewModel);
        }

        public void Initalize(object args = null)
        {
            
        }
    }
}
