﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NecBlik.Core.Factories;
using NecBlik.Core.GUI.Interfaces;
using NecBlik.Core.GUI.ViewModels;
using NecBlik.Core.Models;
using NecBlik.PyDigi.GUI.ViewModels;
using NecBlik.PyDigi.GUI.ViewModels.Wizard;
using NecBlik.PyDigi.GUI.Views.Wizard;
using NecBlik.PyDigi.Models;
using NecBlik.Virtual.GUI.Factories;
using NecBlik.Virtual.GUI.ViewModels;

namespace NecBlik.PyDigi.GUI.Factories
{
    public class PyDigiZigBeeGuiFactory:VirtualDeviceGuiFactory
    {
        public PyDigiZigBeeGuiFactory()
        {
            this.internalFactoryType = PyDigi.Resources.Resources.PyDigiFactoryId;
        }

        public override NetworkViewModel GetNetworkViewModel(Network zigBeeNetwork)
        {
            if (zigBeeNetwork.GetVendorID() != this.internalFactoryType)
                return null;
            return new PyDigiZigBeeNetworkViewModel(zigBeeNetwork);
        }

        public override DataTemplate GetNetworkDataTemplate(Network zigBeeNetwork)
        {
            if (zigBeeNetwork.GetVendorID() == this.GetVendorID())
            {
                var myResourceDictionary = new ResourceDictionary();
                myResourceDictionary.Source = new Uri("/NecBlik.PyDigi.GUI;component/Styles/MergedDictionaries.xaml", UriKind.RelativeOrAbsolute);
                var template = myResourceDictionary["PyDigiNetworkDataTemplate"] as DataTemplate;
                return template;
            }
            return null;
        }

        public override DataTemplate GetNetworkBriefDataTemplate(Network zigBeeNetwork)
        {
            if (zigBeeNetwork.GetVendorID() == this.GetVendorID())
            {
                var myResourceDictionary = new ResourceDictionary();
                myResourceDictionary.Source = new Uri("/NecBlik.PyDigi.GUI;component/Styles/MergedDictionaries.xaml", UriKind.RelativeOrAbsolute);
                var template = myResourceDictionary["PyDigiNetworkBriefDataTemplate"] as DataTemplate;
                return template;
            }
            return null;
        }

        public override async Task<NetworkViewModel> NetworkViewModelFromWizard(Network zigBeeNetwork)
        {
            var vm = new PyDigiNetworkWizardViewModel();
            var rp = new PyDigiNetworkWizard(vm);
            return await rp.ProvideResponseAsync();
        }

        public override VirtualNetworkViewModel NetworkViewModelBySubType(Network network, string subType)
        {
            if(network.GetVendorID()!= this.GetVendorID())
                return base.NetworkViewModelBySubType(network, subType);

            var types = this.GetTypesTInOtherSubAssemblies<PyDigiZigBeeNetworkViewModel>();
            foreach(var type in types)
            {
                if (type.FullName == subType)
                {
                    network.InternalSubType = subType;
                    return Activator.CreateInstance(type, network) as PyDigiZigBeeNetworkViewModel;
                }
            }

            var assembly = Assembly.GetAssembly(typeof(PyDigiZigBeeGuiFactory));
            foreach (var type in assembly.GetExportedTypes())
            {
                if (type.FullName == subType)
                {
                    network.InternalSubType = subType;
                    return Activator.CreateInstance(type, network) as PyDigiZigBeeNetworkViewModel;
                }
            }

            var fromBase = base.NetworkViewModelBySubType(network, subType);
            if (fromBase != null)
                if (network.GetVendorID() != this.GetVendorID())
                    return fromBase;

            return new PyDigiZigBeeNetworkViewModel(network);
        }

        public override VirtualDeviceViewModel DeviceViewModelFromRule(DeviceModel model, NetworkViewModel network, FactoryRule rule)
        {
            var types = this.GetTypesTInOtherSubAssemblies<PyDigiZigBeeViewModel>();
            foreach (var type in types)
            {
                if (type.FullName == rule.Value)
                {
                    network.Model.DeviceCoordinatorSubtypeFactoryRule = rule;
                    return Activator.CreateInstance(type, model, network) as PyDigiZigBeeViewModel;
                }
            }
            var assembly = Assembly.GetAssembly(typeof(PyDigiZigBeeGuiFactory));
            foreach (var type in assembly.GetExportedTypes())
            {
                if (type.FullName == rule.Value)
                {
                    network.Model.DeviceCoordinatorSubtypeFactoryRule = rule;
                    return Activator.CreateInstance(type, model, network) as PyDigiZigBeeViewModel;
                }
            }
            var fromBase = base.DeviceViewModelFromRule(model, network, rule);
            if (fromBase != null)
                if (model.DeviceSource.GetVendorID() != this.GetVendorID())
                    return fromBase;

            if (model.DeviceSource is PyDigiZigBeeUSBCoordinator)
                return new PyDigiZigBeeCoordinatorViewModel(model, network);
            return new PyDigiZigBeeViewModel(model, network);
        }

        public override VirtualDeviceViewModel DeviceViewModelFromRules(DeviceModel model, NetworkViewModel network, List<FactoryRule> rules)
        {
            if (model.DeviceSource.GetVendorID() != this.GetVendorID())
                return base.DeviceViewModelFromRules(model, network, rules);

            var assembly = Assembly.GetAssembly(typeof(PyDigiZigBeeGuiFactory));
            FactoryRule rule = rules.Find((f) => { return f.Property == VirtualDeviceGuiFactory.DeviceViewModelRuledProperties.ViewModel && f.CacheObjectId == model.DeviceSource.GetCacheId(); });
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
                var types = this.GetTypesTInOtherSubAssemblies<PyDigiZigBeeViewModel>();
                foreach (var type in types)
                {
                    if (type.FullName == rule.Value)
                    {
                        network.Model.DeviceCoordinatorSubtypeFactoryRule = rule;
                        return Activator.CreateInstance(type, model, network) as PyDigiZigBeeViewModel;
                    }
                }
                foreach (var type in assembly.GetExportedTypes())
                {
                    if (type.FullName == rule.Value)
                    {
                        return Activator.CreateInstance(type, model, network) as PyDigiZigBeeViewModel;
                    }
                }
            }
            return new PyDigiZigBeeViewModel(model, network);
        }

        public override List<string> GetAvailableControls()
        {
            var fromBase = base.GetAvailableControls();
            var ret = new List<string>();
            if (fromBase != null)
                foreach (var fromBaseItem in fromBase)
                    ret.Add(fromBaseItem);

            var types = this.GetTypesTInOtherSubAssemblies<IDeviceControl>();
            foreach (var type in types)
            {
                ret.Add(type.FullName);
            }

            var assembly = Assembly.GetAssembly(typeof(PyDigiZigBeeGuiFactory));
            foreach (var type in assembly.GetExportedTypes())
            {
                if (typeof(IDeviceControl).IsAssignableFrom(type) && !type.IsAbstract)
                {
                    ret.Add(type.FullName);
                }
            }

            return ret;
        }

        public override List<string> GetAvailableNetworkViewModels()
        {
            var fromBase = base.GetAvailableNetworkViewModels();
            var ret = new List<string>();
            if (fromBase != null)
                foreach (var fromBaseItem in fromBase)
                    ret.Add(fromBaseItem);

            var types = this.GetTypesTInOtherSubAssemblies<VirtualNetworkViewModel>();
            foreach (var type in types)
            {
                ret.Add(type.FullName);
            }
            var assembly = Assembly.GetAssembly(typeof(PyDigiZigBeeGuiFactory));
            foreach (var type in assembly.GetExportedTypes())
            {
                if (typeof(VirtualNetworkViewModel).IsAssignableFrom(type) && !type.IsAbstract)
                {
                    ret.Add(type.FullName);
                }
            }
            return ret;
        }

        public override List<string> GetAvailableDeviceViewModels()
        {
            var fromBase = base.GetAvailableDeviceViewModels();
            var ret = new List<string>();
            if (fromBase != null)
                foreach (var fromBaseItem in fromBase)
                    ret.Add(fromBaseItem);

            var types = this.GetTypesTInOtherSubAssemblies<PyDigiZigBeeViewModel>();
            foreach (var type in types)
            {
                ret.Add(type.FullName);
            }

            var assembly = Assembly.GetAssembly(typeof(PyDigiZigBeeGuiFactory));
            foreach (var type in assembly.GetExportedTypes())
            {
                if (typeof(PyDigiZigBeeViewModel).IsAssignableFrom(type) && !type.IsAbstract)
                {
                    ret.Add(type.FullName);
                }
            }
            return ret;
        }

        public override void Initalize(object args = null)
        {
            ZigBeePyEnv.Initialize();
            base.Initalize(args);
        }
    }
}
