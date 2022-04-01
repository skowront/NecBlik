using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Common.WpfExtensions.Base;
using ZigBee.Core.GUI.ViewModels;
using ZigBee.Core.Models;
using ZigBee.Virtual.GUI.Examples.Views.Sources;
using ZigBee.Virtual.GUI.ViewModels;

namespace ZigBee.Virtual.GUI.Examples.ViewModels.Sources
{
    public class VirtualZigBeeViewModelExampleA : VirtualZigBeeViewModel
    {
        public VirtualZigBeeViewModelExampleA(ZigBeeModel model, ZigBeeNetworkViewModel networkModel) : base(model, networkModel)
        {
            this.EditCommand = new RelayCommand((o)=>
            {
                var window = new VirtualZigBeeExampleAWindow(this);
                window.Show();
            });
        }
    }
}
