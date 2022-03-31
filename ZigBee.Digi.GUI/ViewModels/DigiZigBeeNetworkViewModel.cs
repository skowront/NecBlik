using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Core.GUI;
using ZigBee.Core.Models;
using ZigBee.Virtual.GUI.ViewModels;

namespace ZigBee.Digi.GUI.ViewModels
{
    public class DigiZigBeeNetworkViewModel : VirtualZigBeeNetworkViewModel
    {
        public DigiZigBeeNetworkViewModel(ZigBeeNetwork network) : base(network)
        {
        }

        public override ZigBeeViewModel GetZigBeeCoordinatorViewModel()
        {
            if (this.zigBeeCoorinator == null)
            {
                var zvm = new ZigBeeModel(this.Model.ZigBeeCoordinator);
                this.zigBeeCoorinator = new DigiZigBeeViewModel(zvm, this);
                this.zigBeeCoorinator.PullSelectionSubscriber = ZigBeeSelectionSubscriber;
            }
            return this.zigBeeCoorinator;
        }

        public override void SyncFromModel()
        {
            this.ZigBees.Clear();

            foreach (var device in this.model.ZigBeeSources)
            {
                var vm = new DigiZigBeeViewModel(new ZigBeeModel(device), this);
                vm.PullSelectionSubscriber = this.ZigBeeSelectionSubscriber;
                this.ZigBees.Add(vm);
            }
        }
    }
}
