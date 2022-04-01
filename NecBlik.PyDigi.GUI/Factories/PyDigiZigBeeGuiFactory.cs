using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NecBlik.Core.GUI.ViewModels;
using NecBlik.Core.Models;
using NecBlik.PyDigi.GUI.ViewModels;
using NecBlik.PyDigi.GUI.ViewModels.Wizard;
using NecBlik.PyDigi.GUI.Views.Wizard;
using NecBlik.PyDigi.Models;
using NecBlik.Virtual.GUI.Factories;

namespace NecBlik.PyDigi.GUI.Factories
{
    public class PyDigiZigBeeGuiFactory:VirtualZigBeeGuiFactory
    {
        public PyDigiZigBeeGuiFactory()
        {
            this.internalFactoryType = PyDigi.Resources.Resources.PyDigiFactoryId;
        }

        public override ZigBeeNetworkViewModel GetNetworkViewModel(ZigBeeNetwork zigBeeNetwork)
        {
            if (zigBeeNetwork.GetVendorID() != this.internalFactoryType)
                return null;
            return new PyDigiZigBeeNetworkViewModel(zigBeeNetwork);
        }

        public override DataTemplate GetNetworkDataTemplate(ZigBeeNetwork zigBeeNetwork)
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

        public override DataTemplate GetNetworkBriefDataTemplate(ZigBeeNetwork zigBeeNetwork)
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

        public override async Task<ZigBeeNetworkViewModel> NetworkViewModelFromWizard(ZigBeeNetwork zigBeeNetwork)
        {
            var vm = new PyDigiNetworkWizardViewModel();
            var rp = new PyDigiNetworkWizard(vm);
            return await rp.ProvideResponseAsync();
        }

        public override void Initalize(object args = null)
        {
            ZigBeePyEnv.Initialize();
            base.Initalize(args);
        }
    }
}
