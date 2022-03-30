using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Core.GUI;
using ZigBee.Core.Models;
using ZigBee.Virtual.GUI.ViewModels;

namespace ZigBee.PyDigi.GUI.ViewModels
{
    public class PyDigiZigBeeNetworkViewModel:VirtualZigBeeNetworkViewModel
    {
        public PyDigiZigBeeNetworkViewModel(ZigBeeNetwork network):base(network)
        {

        }

        public override ZigBeeViewModel GetZigBeeCoordinatorViewModel()
        {
            var zvm = new ZigBeeModel(this.Model.ZigBeeCoordinator);
            var vm = new PyDigiZigBeeViewModel(zvm);
            vm.PullSelectionSubscriber = ZigBeeSelectionSubscriber;
            return vm;
        }
    }
}
