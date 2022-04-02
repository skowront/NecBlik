using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NecBlik.Core.GUI.ViewModels;
using NecBlik.Core.Interfaces;
using NecBlik.Core.Models;

namespace NecBlik.Core.GUI.Interfaces
{
    public interface IDeviceGuiFactory:IVendable, IInitializable
    {
        void AttachOtherFactories(List<IDeviceGuiFactory> deviceFactories);

        DataTemplate GetNetworkDataTemplate(Network network);

        DataTemplate GetNetworkBriefDataTemplate(Network network);

        NetworkViewModel GetNetworkViewModel(Network network);
        
        Task<NetworkViewModel> NetworkViewModelFromWizard(Network network);

        UIElement GetDeviceControl(DeviceViewModel network);
    }
}
