using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using NecBlik.Core.Factories;
using NecBlik.Core.GUI;
using NecBlik.Core.GUI.Factories;
using NecBlik.Core.GUI.ViewModels;
using NecBlik.Core.Models;
using NecBlik.Virtual.GUI.ViewModels;
using NecBlik.Virtual.GUI.Views.Controls;
using NecBlik.Virtual.GUI.Views.Wizard;

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
            return this.NetworkViewModelBySubType(network,network.InternalSubType);
        }

        public override string GetVendorID()
        {
            return this.internalFactoryType;
        }

        public override UIElement GetDeviceControl(DeviceViewModel deviceViewModel)
        {
            if (deviceViewModel?.Model?.DeviceSource is Virtual.Models.VirtualCoordinator)
            {
                return new VirtualCoordinatorUserControl(deviceViewModel);
            }
            
            var zbc = base.GetDeviceControl(deviceViewModel);
            return zbc;
        }

        public override async Task<NetworkViewModel> NetworkViewModelFromWizard(Network network)
        {
            var rp = new VirtualNetworkWizard(new ViewModels.Wizard.VirtualNetworkWizardViewModel());
            return await rp.ProvideResponse();
        }

        public VirtualNetworkViewModel NetworkViewModelBySubType(Network network, string subType)
        {
            var assembly = Assembly.GetAssembly(typeof(VirtualDeviceGuiFactory));
            foreach (var type in assembly.GetExportedTypes())
            {
                if (type.FullName == subType)
                {
                    network.InternalSubType = subType;
                    return Activator.CreateInstance(type,network) as VirtualNetworkViewModel;
                }
            }
            return new VirtualNetworkViewModel(network);
        }

        public List<string> GetAvailableNetworkViewModels()
        {
            var ret = new List<string>();
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

        public VirtualDeviceViewModel DeviceViewModelFromRule(DeviceModel model, NetworkViewModel network ,FactoryRule rule)
        {
            var assembly = Assembly.GetAssembly(typeof(VirtualDeviceGuiFactory));
            foreach (var type in assembly.GetExportedTypes())
            {
                if (type.FullName == rule.Value)
                {
                    network.Model.DeviceCoordinatorSubtypeFactoryRule = rule;
                    return Activator.CreateInstance(type,model, network) as VirtualDeviceViewModel;
                }
            }
            return new VirtualDeviceViewModel(model,network);
        }

        public VirtualDeviceViewModel DeviceViewModelFromRules(DeviceModel model, NetworkViewModel network, List<FactoryRule> rules)
        {
            var assembly = Assembly.GetAssembly(typeof(VirtualDeviceGuiFactory));
            FactoryRule rule = null;
            foreach(var item in rules)
            {
                if(item.CacheObjectId == model.CacheId)
                {
                    rule = item;
                    break;
                }
            }
            if(rule!=null)
            foreach (var type in assembly.GetExportedTypes())
            {
                if (type.FullName == rule.Value)
                {
                     return Activator.CreateInstance(type, model, network) as VirtualDeviceViewModel;
                }
            }
            return new VirtualDeviceViewModel(model, network);
        }

        public override void Initalize(object args = null)
        {
            base.Initalize(args);

            
        }
    }
}
