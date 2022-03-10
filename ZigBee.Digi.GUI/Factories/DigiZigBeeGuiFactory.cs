﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Core.GUI.ViewModels;
using ZigBee.Core.Interfaces;
using ZigBee.Core.Models;
using ZigBee.Digi.Models;
using ZigBee.Digi.GUI.ViewModels;
using ZigBee.Virtual.GUI.Factories;
using System.Windows;
using ZigBee.Digi.GUI.Views.Wizard;
using ZigBee.Digi.GUI.ViewModels.Wizard;

namespace ZigBee.Digi.GUI.Factories
{
    public class DigiZigBeeGuiFactory:VirtualZigBeeGuiFactory
    {
        public DigiZigBeeGuiFactory()
        {
            this.internalFactoryType = "Digi";
        }

        public override ZigBeeNetworkViewModel GetNetworkViewModel(ZigBeeNetwork zigBeeNetwork)
        {
            if (zigBeeNetwork.GetVendorID() != this.internalFactoryType)
                return null;
            return new DigiZigBeeNetworkViewModel(zigBeeNetwork);
        }

        public override DataTemplate GetNetworkDataTemplate(ZigBeeNetwork zigBeeNetwork)
        {
            if (zigBeeNetwork.GetVendorID() == this.GetVendorID())
            {
                var myResourceDictionary = new ResourceDictionary();
                myResourceDictionary.Source = new Uri("/ZigBee.Digi.GUI;component/Styles/MergedDictionaries.xaml", UriKind.RelativeOrAbsolute);
                var template = myResourceDictionary["DigiNetworkDataTemplate"] as DataTemplate;
                return template;
            }
            return null;
        }

        public override DataTemplate GetNetworkBriefDataTemplate(ZigBeeNetwork zigBeeNetwork)
        {
            if (zigBeeNetwork.GetVendorID() == this.GetVendorID())
            {
                var myResourceDictionary = new ResourceDictionary();
                myResourceDictionary.Source = new Uri("/ZigBee.Digi.GUI;component/Styles/MergedDictionaries.xaml", UriKind.RelativeOrAbsolute);
                var template = myResourceDictionary["DigiNetworkBriefDataTemplate"] as DataTemplate;
                return template;
            }
            return null;
        }

        public override async Task<ZigBeeNetworkViewModel> NetworkViewModelFromWizard(ZigBeeNetwork zigBeeNetwork)
        {
            var vm = new DigiNetworkWizardViewModel();
            var rp = new DigiNetworkWizard(vm);
            return await rp.ProvideResponseAsync();
        }
    }
}