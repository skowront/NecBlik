using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZigBee.Core.GUI.Interfaces;
using ZigBee.Core.GUI.ViewModels;
using ZigBee.Core.GUI.Views.Controls;
using ZigBee.Core.Models;

namespace ZigBee.Core.GUI.Factories
{
    public class ZigBeeGuiAnyFactory: IZigBeeGuiFactory
    { 

        private static ZigBeeGuiAnyFactory instance;
        public static ZigBeeGuiAnyFactory Instance
        {
            get
            {
                if (ZigBeeGuiAnyFactory.instance == null)
                {
                    ZigBeeGuiAnyFactory.instance = new ZigBeeGuiAnyFactory();
                    instance.Refresh();
                }
                return ZigBeeGuiAnyFactory.instance;
            }
        }

        private IZigBeeGuiFactory DefaultFactory = new ZigBeeGuiDefaultFactory();

        private List<IZigBeeGuiFactory> Factories = new List<IZigBeeGuiFactory>();
        public IZigBeeGuiFactory GetFactory(string factoryId)
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
        public ZigBeeGuiAnyFactory()
        {
            
        }

        public void Refresh()
        {
            this.Factories = this.LoadFactories();
        }

        private List<IZigBeeGuiFactory> LoadFactories()
        {
            var factories = new List<IZigBeeGuiFactory>();
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
                    if (typeof(IZigBeeGuiFactory).IsAssignableFrom(type) && !type.IsAbstract)
                    {
                        if (Activator.CreateInstance(type) as ZigBeeGuiDefaultFactory != null || Activator.CreateInstance(type) as ZigBeeGuiAnyFactory != null)
                        {
                            continue;
                        }
                        var module = Activator.CreateInstance(type) as IZigBeeGuiFactory;
                        module.Initalize();
                        factories.Add(module);
                    }
                }
            }

            var otherFactories = new List<IZigBeeGuiFactory>();
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
                otherFactories = new List<IZigBeeGuiFactory>();
            }

            return factories;
        }

        public void AttachOtherFactories(List<IZigBeeGuiFactory> zigBeeFactories)
        {
            return;
        }

        public DataTemplate GetNetworkDataTemplate(ZigBeeNetwork zigBeeNetwork)
        {
            foreach(var item in this.Factories)
            {
                var template = item.GetNetworkDataTemplate(zigBeeNetwork);
                if(template != null)
                {
                    return template;
                }
            }
            return this.DefaultFactory.GetNetworkDataTemplate(zigBeeNetwork);
        }

        public DataTemplate GetNetworkBriefDataTemplate(ZigBeeNetwork zigBeeNetwork)
        {
            foreach (var item in this.Factories)
            {
                var template = item.GetNetworkBriefDataTemplate(zigBeeNetwork);
                if (template != null)
                {
                    return template;
                }
            }
            return this.DefaultFactory.GetNetworkBriefDataTemplate(zigBeeNetwork);
        }

        public virtual ZigBeeNetworkViewModel GetNetworkViewModel(ZigBeeNetwork zigBeeNetwork)
        {
            foreach(var item in this.Factories)
            {
                var r = item.GetNetworkViewModel(zigBeeNetwork);
                if(r !=null)
                {
                    return r;
                }
            }
            return this.DefaultFactory.GetNetworkViewModel(zigBeeNetwork);
        }

        public virtual async Task<ZigBeeNetworkViewModel> NetworkViewModelFromWizard(ZigBeeNetwork zigBeeNetwork)
        {
            foreach (var item in this.Factories)
            {
                var r = item.NetworkViewModelFromWizard(zigBeeNetwork);
                if (r != null)
                {
                    return await r;
                }
            }
            return await this.DefaultFactory.NetworkViewModelFromWizard(zigBeeNetwork);
        }

        public virtual async Task<ZigBeeNetworkViewModel> NetworkViewModelFromWizard(ZigBeeNetwork zigBeeNetwork,string FactoryID)
        {
            var factory = this.Factories.Where((o) => { return o.GetVendorID() == FactoryID; }).FirstOrDefault(this.DefaultFactory);
            return await factory.NetworkViewModelFromWizard(zigBeeNetwork);
        }

        public string GetVendorID()
        {
            return ZigBee.Core.Resources.Resources.AnyFactoryId;
        }

        public UIElement GetZigBeeControl(ZigBeeViewModel zigBeeViewModel)
        {
            foreach (var item in this.Factories)
            {
                var r = item.GetZigBeeControl(zigBeeViewModel);
                if (r != null)
                {
                    return r;
                }
            }
            return this.DefaultFactory.GetZigBeeControl(zigBeeViewModel);
        }

        public void Initalize(object args = null)
        {
            
        }
    }
}
