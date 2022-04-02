using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NecBlik.Core.GUI.ViewModels;
using NecBlik.Core.Models;
using NecBlik.Virtual.GUI.Factories;
using NecBlik.ZigBeeNet.GUI.ViewModels.Wizard;
using NecBlik.ZigBeeNet.GUI.Views.Wizard;

namespace NecBlik.ZigBeeNet.GUI.Factories
{
    public class ZBNetGuiFactory:VirtualDeviceGuiFactory
    {
        public ZBNetGuiFactory()
        {
            this.internalFactoryType = "ZigBeeNet";
        }

        public override NetworkViewModel GetNetworkViewModel(Network zigBeeNetwork)
        {
            throw new NotImplementedException();
        }

        public override DataTemplate GetNetworkDataTemplate(Network zigBeeNetwork)
        {
            if (zigBeeNetwork.GetVendorID() == this.GetVendorID())
            {
                var myResourceDictionary = new ResourceDictionary();
                myResourceDictionary.Source = new Uri("/NecBlik.ZigBeeNet.GUI;component/Styles/MergedDictionaries.xaml", UriKind.RelativeOrAbsolute);
                var template = myResourceDictionary["ZBNetNetworkDataTemplate"] as DataTemplate;
                return template;
            }
            return null;
        }

        public override DataTemplate GetNetworkBriefDataTemplate(Network zigBeeNetwork)
        {
            if (zigBeeNetwork.GetVendorID() == this.GetVendorID())
            {
                var myResourceDictionary = new ResourceDictionary();
                myResourceDictionary.Source = new Uri("/NecBlik.ZigBeeNet.GUI;component/Styles/MergedDictionaries.xaml", UriKind.RelativeOrAbsolute);
                var template = myResourceDictionary["ZBNetNetworkBriefDataTemplate"] as DataTemplate;
                return template;
            }
            return null;
        }

        public override async Task<NetworkViewModel> NetworkViewModelFromWizard(Network zigBeeNetwork)
        {
            var vm = new ZBNetNetworkWizardViewModel();
            var rp = new ZBNetNetworkWizard(vm);
            return await rp.ProvideResponseAsync();
        }
    }
}
