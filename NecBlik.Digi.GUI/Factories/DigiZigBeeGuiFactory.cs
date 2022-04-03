﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NecBlik.Core.GUI.ViewModels;
using NecBlik.Core.Interfaces;
using NecBlik.Core.Models;
using NecBlik.Digi.Models;
using NecBlik.Digi.GUI.ViewModels;
using NecBlik.Virtual.GUI.Factories;
using System.Windows;
using NecBlik.Digi.GUI.Views.Wizard;
using NecBlik.Digi.GUI.ViewModels.Wizard;
using System.Reflection;
using NecBlik.Core.Factories;
using NecBlik.Virtual.GUI.ViewModels;

namespace NecBlik.Digi.GUI.Factories
{
    public class DigiZigBeeGuiFactory:VirtualDeviceGuiFactory
    {
        public DigiZigBeeGuiFactory()
        {
            this.internalFactoryType = NecBlik.Digi.Resources.Resources.DigiFactoryId;
        }

        public override NetworkViewModel GetNetworkViewModel(Network zigBeeNetwork)
        {
            if (zigBeeNetwork.GetVendorID() != this.internalFactoryType)
                return null;
            return new DigiZigBeeNetworkViewModel(zigBeeNetwork);
        }

        public override DataTemplate GetNetworkDataTemplate(Network zigBeeNetwork)
        {
            if (zigBeeNetwork.GetVendorID() == this.GetVendorID())
            {
                var myResourceDictionary = new ResourceDictionary();
                myResourceDictionary.Source = new Uri("/NecBlik.Digi.GUI;component/Styles/MergedDictionaries.xaml", UriKind.RelativeOrAbsolute);
                var template = myResourceDictionary["DigiNetworkDataTemplate"] as DataTemplate;
                return template;
            }
            return null;
        }

        public override DataTemplate GetNetworkBriefDataTemplate(Network zigBeeNetwork)
        {
            if (zigBeeNetwork.GetVendorID() == this.GetVendorID())
            {
                var myResourceDictionary = new ResourceDictionary();
                myResourceDictionary.Source = new Uri("/NecBlik.Digi.GUI;component/Styles/MergedDictionaries.xaml", UriKind.RelativeOrAbsolute);
                var template = myResourceDictionary["DigiNetworkBriefDataTemplate"] as DataTemplate;
                return template;
            }
            return null;
        }

        public override async Task<NetworkViewModel> NetworkViewModelFromWizard(Network zigBeeNetwork)
        {
            var vm = new DigiNetworkWizardViewModel();
            var rp = new DigiNetworkWizard(vm);
            return await rp.ProvideResponseAsync();
        }

        public override VirtualNetworkViewModel NetworkViewModelBySubType(Network network, string subType)
        {
            if (network.GetVendorID() != this.GetVendorID())
                return base.NetworkViewModelBySubType(network, subType);

            var types = this.GetTypesTInOtherSubAssemblies<DigiZigBeeNetworkViewModel>();
            foreach (var type in types)
            {
                if (type.FullName == subType)
                {
                    network.InternalSubType = subType;
                    return Activator.CreateInstance(type, network) as DigiZigBeeNetworkViewModel;
                }
            }

            var assembly = Assembly.GetAssembly(typeof(VirtualDeviceGuiFactory));
            foreach (var type in assembly.GetExportedTypes())
            {
                if (type.FullName == subType)
                {
                    network.InternalSubType = subType;
                    return Activator.CreateInstance(type, network) as DigiZigBeeNetworkViewModel;
                }
            }

            return new DigiZigBeeNetworkViewModel(network);
        }
        public override VirtualDeviceViewModel DeviceViewModelFromRule(DeviceModel model, NetworkViewModel network, FactoryRule rule)
        {
            if (model.DeviceSource.GetVendorID() != this.GetVendorID())
                return base.DeviceViewModelFromRule(model, network,rule);

            //TODO remove this if after rules are properly assigned from dropdown menu.h
            if (model.DeviceSource is DigiZigBeeUSBCoordinator)
                return new DigiZigBeeCoordinatorViewModel(model, network);

            var types = this.GetTypesTInOtherSubAssemblies<DigiZigBeeViewModel>();
            foreach (var type in types)
            {
                if (type.FullName == rule.Value)
                {
                    network.Model.DeviceCoordinatorSubtypeFactoryRule = rule;
                    return Activator.CreateInstance(type, model, network) as DigiZigBeeViewModel;
                }
            }
            var assembly = Assembly.GetAssembly(typeof(VirtualDeviceGuiFactory));
            foreach (var type in assembly.GetExportedTypes())
            {
                if (type.FullName == rule.Value)
                {
                    network.Model.DeviceCoordinatorSubtypeFactoryRule = rule;
                    return Activator.CreateInstance(type, model, network) as DigiZigBeeViewModel;
                }
            }
            return new DigiZigBeeViewModel(model, network);
        }
        public override VirtualDeviceViewModel DeviceViewModelFromRules(DeviceModel model, NetworkViewModel network, List<FactoryRule> rules)
        {
            if (model.DeviceSource.GetVendorID() != this.GetVendorID())
                return base.DeviceViewModelFromRules(model, network, rules);

            var assembly = Assembly.GetAssembly(typeof(VirtualDeviceGuiFactory));
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
                var types = this.GetTypesTInOtherSubAssemblies<DigiZigBeeViewModel>();
                foreach (var type in types)
                {
                    if (type.FullName == rule.Value)
                    {
                        network.Model.DeviceCoordinatorSubtypeFactoryRule = rule;
                        return Activator.CreateInstance(type, model, network) as DigiZigBeeViewModel;
                    }
                }
                foreach (var type in assembly.GetExportedTypes())
                {
                    if (type.FullName == rule.Value)
                    {
                        return Activator.CreateInstance(type, model, network) as DigiZigBeeViewModel;
                    }
                }
            }
            return new DigiZigBeeViewModel(model, network);
        }
    }
}
