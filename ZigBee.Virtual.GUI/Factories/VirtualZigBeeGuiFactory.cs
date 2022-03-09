using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZigBee.Core.GUI;
using ZigBee.Core.GUI.Factories;
using ZigBee.Core.GUI.Interfaces;
using ZigBee.Core.GUI.ViewModels;
using ZigBee.Core.Models;
using ZigBee.Virtual.GUI.ViewModels;
using ZigBee.Virtual.GUI.Views.Controls;

namespace ZigBee.Virtual.GUI.Factories
{
    public class VirtualZigBeeGuiFactory : ZigBeeGuiFactory
    {
        public VirtualZigBeeGuiFactory()
        {
            this.internalFactoryType = "Virtual";
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

        public override ZigBeeNetworkViewModel NetworkViewModelFromWizard(ZigBeeNetwork zigBeeNetwork)
        {
            throw new NotImplementedException();
        }
    }
}
