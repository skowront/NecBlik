using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Core.GUI;
using ZigBee.Core.Models;
using ZigBee.Virtual.GUI.ViewModels;

namespace ZigBee.ZigBeeNet.GUI.ViewModels
{
    public class ZBNetNetworkViewModel: VirtualZigBeeNetworkViewModel
    {
        public ZBNetNetworkViewModel(ZigBeeNetwork network) : base(network)
        {
        }

        public override ZigBeeViewModel GetZigBeeCoordinatorViewModel()
        {
            var zvm = new ZigBeeModel(this.Model.ZigBeeCoordinator);
            var vm = new ZBNetSourceViewModel(zvm);
            vm.PullSelectionSubscriber = ZigBeeSelectionSubscriber;
            return vm;
        }

        public override void SyncFromModel()
        {
            this.ZigBees.Clear();

            foreach (var device in this.model.ZigBeeSources)
            {
                var vm = new ZBNetSourceViewModel(new ZigBeeModel(device));
                vm.PullSelectionSubscriber = this.ZigBeeSelectionSubscriber;
                this.ZigBees.Add(vm);
            }
        }
    }
}
