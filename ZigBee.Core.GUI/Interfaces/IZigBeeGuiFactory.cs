using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZigBee.Core.Interfaces;
using ZigBee.Core.Models;

namespace ZigBee.Core.GUI.Interfaces
{
    public interface IZigBeeGuiFactory:IVendable
    {
        void AttachOtherFactories(List<IZigBeeGuiFactory> zigBeeFactories);

        DataTemplate GetNetworkDataTemplate(ZigBeeNetwork zigBeeNetwork);

        DataTemplate GetNetworkBriefDataTemplate(ZigBeeNetwork zigBeeNetwork);
    }
}
