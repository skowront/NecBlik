using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZigBee.Core.GUI.Interfaces;
using ZigBee.Core.GUI.ViewModels;
using ZigBee.Core.GUI.Views.Controls;
using ZigBee.Core.Models;

namespace ZigBee.Core.GUI.Factories
{
    public class ZigBeeGuiDefaultFactory: ZigBeeGuiFactory
    {
        private List<IZigBeeGuiFactory> otherFactories;

        public ZigBeeGuiDefaultFactory()
        {
            this.internalFactoryType = "Default";
        }

        public override void AttachOtherFactories(List<IZigBeeGuiFactory> zigBeeFactories)
        {
            this.otherFactories = zigBeeFactories;
        }

        public override DataTemplate GetNetworkBriefDataTemplate(ZigBeeNetwork zigBeeNetwork)
        {
            return null;
        }

        public override DataTemplate GetNetworkDataTemplate(ZigBeeNetwork zigBeeNetwork)
        {
            return null;
        }

        public override ZigBeeNetworkViewModel GetNetworkViewModel(ZigBeeNetwork zigBeeNetwork)
        {
            return new ZigBeeNetworkViewModel(zigBeeNetwork);
        }

        public override string GetVendorID()
        {
            return this.internalFactoryType;
        }

        public override UIElement GetZigBeeControl(ZigBeeViewModel zigBeeViewModel)
        {
            if (zigBeeViewModel.GetVendorID() == this.GetVendorID())
                return new ZigBeeControl(zigBeeViewModel);
            return null;
        }
    }
}
