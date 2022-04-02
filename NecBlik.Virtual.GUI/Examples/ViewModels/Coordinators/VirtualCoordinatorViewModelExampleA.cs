using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NecBlik.Common.WpfExtensions.Base;
using NecBlik.Core.GUI.ViewModels;
using NecBlik.Core.Models;
using NecBlik.Virtual.GUI.Examples.Views.Coordinators;
using NecBlik.Virtual.GUI.ViewModels;

namespace NecBlik.Virtual.GUI.Examples.ViewModels.Coordinators
{
    public class VirtualCoordinatorViewModelExampleA : VirtualDeviceViewModel
    {
        public VirtualCoordinatorViewModelExampleA(DeviceModel model, NetworkViewModel networkModel) : base(model, networkModel)
        {
            
        }

        protected override void BuildCommands()
        {
            base.BuildCommands();
            this.EditCommand = new RelayCommand((o) =>
            {
                var window = new VirtualCoordinatorExampleAWindow(this);
                window.Show();
            });
        }
    }
}
