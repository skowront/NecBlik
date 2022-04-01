using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NecBlik.Core.GUI;
using NecBlik.Core.Models;
using NecBlik.Virtual.GUI.ViewModels;

namespace NecBlik.Digi.GUI.ViewModels
{
    public class DigiZigBeeNetworkViewModel : VirtualNetworkViewModel
    {
        public DigiZigBeeNetworkViewModel(Network network) : base(network)
        {
        }

        public override DeviceViewModel GetCoordinatorViewModel()
        {
            if (this.coorinator == null)
            {
                var zvm = new DeviceModel(this.Model.Coordinator);
                this.coorinator = new DigiZigBeeCoordinatorViewModel(zvm, this);
                this.coorinator.PullSelectionSubscriber = DeviceSelectionSubscriber;
                this.DeviceSelectionSubscriber?.NotifyUpdated(this.coorinator);
            }
            return this.coorinator;
        }

        public override void Close()
        {
            this.coorinator.Model.DeviceSource.Close();
        }

        public override bool Open()
        {
            return this.coorinator.Model.DeviceSource.Open();
        }

        public override void SyncFromModel()
        {
            this.Devices.Clear();

            foreach (var device in this.model.DeviceSources)
            {
                var vm = new DigiZigBeeViewModel(new DeviceModel(device), this);
                vm.PullSelectionSubscriber = this.DeviceSelectionSubscriber;
                this.DeviceSelectionSubscriber?.NotifyUpdated(vm);
                this.Devices.Add(vm);
            }

            if(this.model.HasCoordinator)
            {
                var zvm = new DeviceModel(this.Model.Coordinator);
                this.coorinator = new DigiZigBeeCoordinatorViewModel(zvm, this);
                this.coorinator.PullSelectionSubscriber = DeviceSelectionSubscriber;
                this.DeviceSelectionSubscriber?.NotifyUpdated(this.coorinator);
            }
        }

        public override async Task Discover()
        {
            await this.model.SyncCoordinator();
            this.SyncFromModel();
        }
    }
}
