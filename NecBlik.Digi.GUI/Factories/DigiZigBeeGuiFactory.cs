using System;
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

namespace NecBlik.Digi.GUI.Factories
{
    public class DigiZigBeeGuiFactory:VirtualZigBeeGuiFactory
    {
        public DigiZigBeeGuiFactory()
        {
            this.internalFactoryType = NecBlik.Digi.Resources.Resources.DigiFactoryId;
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
                myResourceDictionary.Source = new Uri("/NecBlik.Digi.GUI;component/Styles/MergedDictionaries.xaml", UriKind.RelativeOrAbsolute);
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
                myResourceDictionary.Source = new Uri("/NecBlik.Digi.GUI;component/Styles/MergedDictionaries.xaml", UriKind.RelativeOrAbsolute);
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
