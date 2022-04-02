using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NecBlik.Core.GUI;
using NecBlik.Core.Models;
using NecBlik.Virtual.GUI.ViewModels;

namespace NecBlik.PyDigi.GUI.ViewModels
{
    public class PyDigiZigBeeNetworkViewModel:VirtualNetworkViewModel
    {
        public PyDigiZigBeeNetworkViewModel(Network network):base(network)
        {

        }

        public override DeviceViewModel GetCoordinatorViewModel()
        {
            if (this.coorinator == null)
            {
                var zvm = new DeviceModel(this.Model.Coordinator);
                this.coorinator = new PyDigiZigBeeViewModel(zvm, this);
                this.coorinator.PullSelectionSubscriber = DeviceSelectionSubscriber;
            }
            return this.coorinator;
        }
    }
}
