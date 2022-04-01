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
    public class PyDigiZigBeeNetworkViewModel:VirtualZigBeeNetworkViewModel
    {
        public PyDigiZigBeeNetworkViewModel(ZigBeeNetwork network):base(network)
        {

        }

        public override ZigBeeViewModel GetZigBeeCoordinatorViewModel()
        {
            if (this.zigBeeCoorinator == null)
            {
                var zvm = new ZigBeeModel(this.Model.ZigBeeCoordinator);
                this.zigBeeCoorinator = new PyDigiZigBeeViewModel(zvm, this);
                this.zigBeeCoorinator.PullSelectionSubscriber = ZigBeeSelectionSubscriber;
            }
            return this.zigBeeCoorinator;
        }
    }
}
