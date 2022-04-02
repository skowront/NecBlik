using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NecBlik.Core.GUI;
using NecBlik.Core.Models;
using NecBlik.Virtual.GUI.ViewModels;

namespace NecBlik.ZigBeeNet.GUI.ViewModels
{
    public class ZBNetNetworkViewModel: VirtualNetworkViewModel
    {
        public ZBNetNetworkViewModel(Network network) : base(network)
        {
        }

        public override DeviceViewModel GetCoordinatorViewModel()
        {
            if (this.coorinator == null)
            {
                var zvm = new DeviceModel(this.Model.Coordinator);
                this.coorinator = new ZBNetSourceViewModel(zvm, this);
                this.coorinator.PullSelectionSubscriber = DeviceSelectionSubscriber;
            }
            return this.coorinator;
        }

        public override void SyncFromModel()
        {
            this.Devices.Clear();

            foreach (var device in this.model.DeviceSources)
            {
                var vm = new ZBNetSourceViewModel(new DeviceModel(device));
                vm.PullSelectionSubscriber = this.DeviceSelectionSubscriber;
                this.Devices.Add(vm);
            }
        }
    }
}
