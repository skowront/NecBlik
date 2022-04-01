using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NecBlik.Common.WpfExtensions.Base;
using NecBlik.Core.GUI.ViewModels;
using NecBlik.Core.Models;
using NecBlik.Virtual.GUI.Examples.Views.Sources;
using NecBlik.Virtual.GUI.ViewModels;

namespace NecBlik.Virtual.GUI.Examples.ViewModels.Sources
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
