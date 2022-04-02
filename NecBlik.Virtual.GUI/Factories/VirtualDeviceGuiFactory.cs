using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using NecBlik.Core.Factories;
using NecBlik.Core.GUI;
using NecBlik.Core.GUI.Factories;
using NecBlik.Core.GUI.Factories.ViewModels;
using NecBlik.Core.GUI.Interfaces;
using NecBlik.Core.GUI.ViewModels;
using NecBlik.Core.Models;
using NecBlik.Virtual.GUI.ViewModels;
using NecBlik.Virtual.GUI.Views.Controls;
using NecBlik.Virtual.GUI.Views.Wizard;
using System.Windows.Controls;
using System.IO;

namespace NecBlik.Virtual.GUI.Factories
{
    public class VirtualDeviceGuiFactory : DeviceGuiFactory
    {
        public VirtualDeviceGuiFactory()
        {
            this.internalFactoryType = NecBlik.Virtual.Resources.Resources.VirtualFactoryId;
        }
        
        public override DataTemplate GetNetworkDataTemplate(Network network)
        {
            if (network.GetVendorID() == this.GetVendorID())
            {
                var myResourceDictionary = new ResourceDictionary();
                myResourceDictionary.Source = new Uri("/NecBlik.Virtual.GUI;component/Styles/MergedDictionaries.xaml", UriKind.RelativeOrAbsolute);
                var template = myResourceDictionary["VirtualNetworkDataTemplate"] as DataTemplate;
                return template;
            }
            return null;
        }

        public override DataTemplate GetNetworkBriefDataTemplate(Network network)
        {
            if (network.GetVendorID() == this.GetVendorID())
            {
                var myResourceDictionary = new ResourceDictionary();
                myResourceDictionary.Source = new Uri("/NecBlik.Virtual.GUI;component/Styles/MergedDictionaries.xaml", UriKind.RelativeOrAbsolute);
                var template = myResourceDictionary["VirtualNetworkBriefDataTemplate"] as DataTemplate;
                return template;
            }
            return null;
        }

        public override NetworkViewModel GetNetworkViewModel(Network network)
        {
            return this.NetworkViewModelBySubType(network, network.InternalSubType);
        }

        public override string GetVendorID()
        {
            return this.internalFactoryType;
        }

        public override async Task<NetworkViewModel> NetworkViewModelFromWizard(Network network)
        {
            var rp = new VirtualNetworkWizard(new ViewModels.Wizard.VirtualNetworkWizardViewModel());
            return await rp.ProvideResponse();
        }

        public override UIElement GetDeviceControl(DeviceViewModel deviceViewModel)
        {
            var rule = deviceViewModel.Network.FactoryRules.Where((r) => { return r.CacheObjectId == deviceViewModel.GetCacheId() && r.Property == VirtualDeviceGuiFactory.DeviceViewModelRuledProperties.MapControl; }).FirstOrDefault();
            if (deviceViewModel?.Model?.DeviceSource is Virtual.Models.VirtualCoordinator)
            {
                if (rule == null)
                {
                    return new VirtualCoordinatorUserControl(deviceViewModel);
                }
            }

            if (rule != null)
            {
                var types = this.GetTypesTInOtherSubAssemblies<IDeviceControl>();
                foreach (var type in types)
                {
                    if (typeof(IDeviceControl).IsAssignableFrom(type) && !type.IsAbstract && type.FullName == rule.Value)
                    {
                        return Activator.CreateInstance(type, deviceViewModel) as UserControl;
                    }
                }
                var currentAssembly = Assembly.GetAssembly(typeof(VirtualDeviceGuiFactory));
                foreach (var type in currentAssembly.GetExportedTypes())
                {
                    if (typeof(IDeviceControl).IsAssignableFrom(type) && !type.IsAbstract && type.FullName == rule.Value)
                    {
                        return Activator.CreateInstance(type, deviceViewModel) as UserControl;
                    }
                }
            }

            var zbc = base.GetDeviceControl(deviceViewModel);
            return zbc;
        }
        public List<string> GetAvailableControls()
        {
            var ret = new List<string>();

            var types = this.GetTypesTInOtherSubAssemblies<IDeviceControl>();
            foreach(var type in types)
            {
                ret.Add(type.FullName);
            }

            var assembly = Assembly.GetAssembly(typeof(VirtualDeviceGuiFactory));
            foreach (var type in assembly.GetExportedTypes())
            {
                if (typeof(IDeviceControl).IsAssignableFrom(type) && !type.IsAbstract)
                {
                    ret.Add(type.FullName);
                }
            }

            return ret;
        }
        public List<string> GetAvailableNetworkViewModels()
        {
            var ret = new List<string>();
            var types = this.GetTypesTInOtherSubAssemblies<VirtualNetworkViewModel>();
            foreach (var type in types)
            {
                ret.Add(type.FullName);
            }
            var assembly = Assembly.GetAssembly(typeof(VirtualDeviceGuiFactory));
            foreach (var type in assembly.GetExportedTypes())
            {
                if (typeof(VirtualNetworkViewModel).IsAssignableFrom(type) && !type.IsAbstract)
                {
                    ret.Add(type.FullName);
                }
            }
            return ret;
        }
        public List<string> GetAvailableDeviceViewModels()
        {
            var ret = new List<string>();

            var types = this.GetTypesTInOtherSubAssemblies<VirtualDeviceViewModel>();
            foreach (var type in types)
            {
                ret.Add(type.FullName);
            }

            var assembly = Assembly.GetAssembly(typeof(VirtualDeviceGuiFactory));
            foreach (var type in assembly.GetExportedTypes())
            {
                if (typeof(VirtualDeviceViewModel).IsAssignableFrom(type) && !type.IsAbstract)
                {
                    ret.Add(type.FullName);
                }
            }
            return ret;
        }
        public virtual VirtualNetworkViewModel NetworkViewModelBySubType(Network network, string subType)
        {
            var types = this.GetTypesTInOtherSubAssemblies<VirtualNetworkViewModel>();
            foreach (var type in types)
            {
                if (type.FullName == subType)
                {
                    network.InternalSubType = subType;
                    return Activator.CreateInstance(type, network) as VirtualNetworkViewModel;
                }
            }

            var assembly = Assembly.GetAssembly(typeof(VirtualDeviceGuiFactory));
            foreach (var type in assembly.GetExportedTypes())
            {
                if (type.FullName == subType)
                {
                    network.InternalSubType = subType;
                    return Activator.CreateInstance(type, network) as VirtualNetworkViewModel;
                }
            }
            return new VirtualNetworkViewModel(network);
        }
        public virtual VirtualDeviceViewModel DeviceViewModelFromRule(DeviceModel model, NetworkViewModel network, FactoryRule rule)
        {
            var types = this.GetTypesTInOtherSubAssemblies<VirtualDeviceViewModel>();
            foreach (var type in types)
            {
                if (type.FullName == rule.Value)
                {
                    network.Model.DeviceCoordinatorSubtypeFactoryRule = rule;
                    return Activator.CreateInstance(type, model, network) as VirtualDeviceViewModel;
                }
            }
            var assembly = Assembly.GetAssembly(typeof(VirtualDeviceGuiFactory));
            foreach (var type in assembly.GetExportedTypes())
            {
                if (type.FullName == rule.Value)
                {
                    network.Model.DeviceCoordinatorSubtypeFactoryRule = rule;
                    return Activator.CreateInstance(type, model, network) as VirtualDeviceViewModel;
                }
            }
            return new VirtualDeviceViewModel(model, network);
        }
        public virtual VirtualDeviceViewModel DeviceViewModelFromRules(DeviceModel model, NetworkViewModel network, List<FactoryRule> rules)
        {
            var assembly = Assembly.GetAssembly(typeof(VirtualDeviceGuiFactory));
            FactoryRule rule = rules.Find((f) => { return f.Property == VirtualDeviceGuiFactory.DeviceViewModelRuledProperties.ViewModel && f.CacheObjectId == model.CacheId; });
            foreach (var item in rules)
            {
                if (item.CacheObjectId == model.CacheId)
                {
                    rule = item;
                    break;
                }
            }
            if (rule != null)
            {
                var types = this.GetTypesTInOtherSubAssemblies<VirtualDeviceViewModel>();
                foreach (var type in types)
                {
                    if (type.FullName == rule.Value)
                    {
                        network.Model.DeviceCoordinatorSubtypeFactoryRule = rule;
                        return Activator.CreateInstance(type, model, network) as VirtualDeviceViewModel;
                    }
                }
                foreach (var type in assembly.GetExportedTypes())
                {
                    if (type.FullName == rule.Value)
                    {
                        return Activator.CreateInstance(type, model, network) as VirtualDeviceViewModel;
                    }
                }
            }
            return new VirtualDeviceViewModel(model, network);
        }
        protected List<Type> GetTypesTInOtherSubAssemblies<T>()
        {
            List<Type> types = new();
            var dir = string.Format(NecBlik.Core.Resources.Resources.LibraryFolderFormattableString,this.internalFactoryType);
            if (Directory.Exists(dir))
            {
                var files = Directory.EnumerateFiles(dir);
                foreach (var file in files)
                {
                    var dllfiles = Directory.EnumerateFiles(dir, "*.dll");
                    foreach (var dll in dllfiles)
                    {
                        var absPath = Path.Combine(dir, dll);
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
                            if (typeof(T).IsAssignableFrom(type) && !type.IsAbstract)
                            {
                                types.Add(type);
                            }
                        }
                    }
                }
            }
            return types;
        }
        public override void Initalize(object args = null)
        {
            base.Initalize(args);
            
        }
        public class DeviceViewModelRuledProperties
        {
            public const string ViewModel = nameof(ViewModel);
            public const string MapControl = nameof(MapControl);
        }
    }
}
