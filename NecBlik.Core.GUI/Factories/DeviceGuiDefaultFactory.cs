using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NecBlik.Core.GUI.Interfaces;
using NecBlik.Core.GUI.ViewModels;
using NecBlik.Core.GUI.Views.Controls;
using NecBlik.Core.Models;

namespace NecBlik.Core.GUI.Factories
{
    public class DeviceGuiDefaultFactory: DeviceGuiFactory
    {
        private List<IDeviceGuiFactory> otherFactories;

        public DeviceGuiDefaultFactory()
        {
            this.internalFactoryType = "Default";
        }

        public override void AttachOtherFactories(List<IDeviceGuiFactory> deviceFactories)
        {
            this.otherFactories = deviceFactories;
        }

        public override DataTemplate GetNetworkBriefDataTemplate(Network deviceNetwork)
        {
            return null;
        }

        public override DataTemplate GetNetworkDataTemplate(Network network)
        {
            return null;
        }

        public override NetworkViewModel GetNetworkViewModel(Network network)
        {
            return new NetworkViewModel(network);
        }

        public override string GetVendorID()
        {
            return this.internalFactoryType;
        }

        public override UIElement GetDeviceControl(DeviceViewModel deviceViewModel)
        {
            if (deviceViewModel.GetVendorID() == this.GetVendorID())
                return new DeviceControl(deviceViewModel);
            return null;
        }

        public override async Task<NetworkViewModel> NetworkViewModelFromWizard(Network network)
        {
            return null;
        }
    }
}
