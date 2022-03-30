﻿using System;
using System.Threading.Tasks;
using System.Windows;
using ZigBee.Core.GUI;
using ZigBee.Core.GUI.Factories;
using ZigBee.Core.GUI.ViewModels;
using ZigBee.Core.Models;
using ZigBee.Virtual.GUI.ViewModels;
using ZigBee.Virtual.GUI.Views.Controls;
using ZigBee.Virtual.GUI.Views.Wizard;

namespace ZigBee.Virtual.GUI.Factories
{
    public class VirtualZigBeeGuiFactory : ZigBeeGuiFactory
    {
        public VirtualZigBeeGuiFactory()
        {
            this.internalFactoryType = ZigBee.Virtual.Resources.Resources.VirtualFactoryId;
        }


        public override DataTemplate GetNetworkDataTemplate(ZigBeeNetwork zigBeeNetwork)
        {
            if(zigBeeNetwork.GetVendorID()==this.GetVendorID())
            {
                var myResourceDictionary = new ResourceDictionary();
                myResourceDictionary.Source = new Uri("/ZigBee.Virtual.GUI;component/Styles/MergedDictionaries.xaml", UriKind.RelativeOrAbsolute);
                var template = myResourceDictionary["VirtualNetworkDataTemplate"] as DataTemplate;
                return template;
            }
            return null;
        }

        public override DataTemplate GetNetworkBriefDataTemplate(ZigBeeNetwork zigBeeNetwork)
        {
            if (zigBeeNetwork.GetVendorID() == this.GetVendorID())
            {
                var myResourceDictionary = new ResourceDictionary();
                myResourceDictionary.Source = new Uri("/ZigBee.Virtual.GUI;component/Styles/MergedDictionaries.xaml", UriKind.RelativeOrAbsolute);
                var template = myResourceDictionary["VirtualNetworkBriefDataTemplate"] as DataTemplate;
                return template;
            }
            return null;
        }

        public override ZigBeeNetworkViewModel GetNetworkViewModel(ZigBeeNetwork zigBeeNetwork)
        {
            return new VirtualZigBeeNetworkViewModel(zigBeeNetwork);
        }

        public override string GetVendorID()
        {
            return this.internalFactoryType;
        }

        public override UIElement GetZigBeeControl(ZigBeeViewModel zigBeeViewModel)
        {
            var zbc = base.GetZigBeeControl(zigBeeViewModel);
            if(zigBeeViewModel?.Model?.ZigBeeSource is Virtual.Models.VirtualZigBeeCoordinator)
            {
                return new VirtualZigBeeCoordinatorUserControl(zigBeeViewModel);
            }
            return zbc;
        }

        public override async Task<ZigBeeNetworkViewModel> NetworkViewModelFromWizard(ZigBeeNetwork zigBeeNetwork)
        {
            var rp = new VirtualNetworkWizard(new ViewModels.Wizard.VirtualNetworkWizardViewModel());
            return rp.ProvideResponse();
        }
    }
}
