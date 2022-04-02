using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NecBlik.Core.GUI;
using NecBlik.Core.Models;
using NecBlik.Digi.GUI.Factories;
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
            foreach (var device in this.model.DeviceSources)
            {
                this.AddNewDevice(device);
            }

            if(this.model.HasCoordinator && this.coorinator == null)
            {
                var zvm = new DeviceModel(this.Model.Coordinator);
                this.coorinator = new DigiZigBeeCoordinatorViewModel(zvm, this);
                this.coorinator.PullSelectionSubscriber = DeviceSelectionSubscriber;
                this.DeviceSelectionSubscriber?.NotifyUpdated(this.coorinator);
            }
        }

        public override bool AddNewDevice(Core.Interfaces.IDeviceSource device)
        {
            var factory = new DigiZigBeeGuiFactory();
            var vm = factory.DeviceViewModelFromRules(new DeviceModel(device), this, this.model.DeviceSubtypeFactoryRules.ToList());
            if (vm == null)
                return base.AddNewDevice(device); ;
            vm.PullSelectionSubscriber = this.DeviceSelectionSubscriber;
            if (!this.model.DeviceSources.Contains(device))
            {
                this.model.DeviceSources.Add(device);
            }
            if (!this.Devices.Any((v) => { return v.GetCacheId() == device.GetCacheId(); }))
            {
                this.Devices.Add(vm);
                this.DeviceSelectionSubscriber?.NotifyUpdated(vm);
                this.NotifyDevicesNetworkChanged(vm);
            }
            return true;
        }

        public override async Task Discover()
        {
            await this.model.SyncCoordinator();
            this.SyncFromModel();
        }
    }
}
