using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Common.WpfExtensions.Base;
using ZigBee.Core.GUI.ViewModels;
using ZigBee.Core.Models;
using ZigBee.Virtual.GUI.Examples.Views.Coordinators;
using ZigBee.Virtual.GUI.ViewModels;

namespace ZigBee.Virtual.GUI.Examples.ViewModels.Coordinators
{
    public class VirtualZigBeeCoordinatorViewModelExampleA : VirtualZigBeeViewModel
    {
        public VirtualZigBeeCoordinatorViewModelExampleA(ZigBeeModel model, ZigBeeNetworkViewModel networkModel) : base(model, networkModel)
        {
            
        }

        protected override void BuildCommands()
        {
            base.BuildCommands();
            this.EditCommand = new RelayCommand((o) =>
            {
                var window = new VirtualZigBeeCoordinatorExampleAWindow(this);
                window.Show();
            });
        }
    }
}
