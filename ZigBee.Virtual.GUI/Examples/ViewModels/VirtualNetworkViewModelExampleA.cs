using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Core.Models;
using ZigBee.Virtual.GUI.Examples.Views;
using ZigBee.Virtual.GUI.ViewModels;

namespace ZigBee.Virtual.GUI.Examples.ViewModels
{
    public class VirtualNetworkViewModelExampleA : VirtualZigBeeNetworkViewModel
    {
        public VirtualNetworkViewModelExampleA(ZigBeeNetwork network) : base(network)
        {
            this.EditCommand = new Common.WpfExtensions.Base.RelayCommand((o) =>
            {
                var window = new VirtualZigBeeNetworkExampleAWindow(this);
                window.Show();
            });
        }
    }
}
