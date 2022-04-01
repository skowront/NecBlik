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
    public interface IZigBeeGuiFactory:IVendable, IInitializable
    {
        void AttachOtherFactories(List<IZigBeeGuiFactory> zigBeeFactories);

        DataTemplate GetNetworkDataTemplate(ZigBeeNetwork zigBeeNetwork);

        DataTemplate GetNetworkBriefDataTemplate(ZigBeeNetwork zigBeeNetwork);

        ZigBeeNetworkViewModel GetNetworkViewModel(ZigBeeNetwork zigBeeNetwork);
        
        Task<ZigBeeNetworkViewModel> NetworkViewModelFromWizard(ZigBeeNetwork zigBeeNetwork);

        UIElement GetZigBeeControl(ZigBeeViewModel zigBeeViewModel);
    }
}
