﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NecBlik.Common.WpfExtensions.Base;
using NecBlik.Core.GUI;
using NecBlik.Core.Models;
using NecBlik.Digi.GUI.Views;
using NecBlik.PyDigi.GUI.Factories;
using NecBlik.Virtual.GUI.ViewModels;

namespace NecBlik.PyDigi.GUI.ViewModels
{
    public class PyDigiZigBeeNetworkViewModel:VirtualNetworkViewModel
    {
        public RelayCommand AddCommand { get; set; }

        public PyDigiZigBeeNetworkViewModel(Network network):base(network)
        {
            this.EditCommand = new RelayCommand((o) =>
            {
                var window = new PyDigiNetworkWindow(this);
                window.Show();
            });

            this.AddCommand = this.DiscoverCommand;
            this.DiscoverCommand = new RelayCommand(async (o) =>
            {
                await this.Discover();
            });
        }

        public override DeviceViewModel GetCoordinatorViewModel()
        {
            if (this.coorinator == null)
            {
                var zvm = new DeviceModel(this.Model.Coordinator);
                var factory = new PyDigiZigBeeGuiFactory();
                this.coorinator = factory.DeviceViewModelFromRule(zvm, this, this.model.DeviceCoordinatorSubtypeFactoryRule);
                if (this.coorinator == null)
                    base.GetCoordinatorViewModel();
            }
            else
                this.coorinator.PullSelectionSubscriber = DeviceSelectionSubscriber;
            this.DeviceSelectionSubscriber?.NotifyUpdated(this.coorinator);
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

            foreach (var device in this.Devices)
                if (device.PullSelectionSubscriber == null)
                    device.PullSelectionSubscriber = this.DeviceSelectionSubscriber;

            this.GetCoordinatorViewModel();

            this.OnPropertyChanged();
        }

        public override bool AddNewDevice(Core.Interfaces.IDeviceSource device)
        {
            var factory = new PyDigiZigBeeGuiFactory();
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
            else
            {
                var existingViewModel = this.Devices.Where((x) => { return x.GetCacheId() == device.GetCacheId(); }).First();
                if (existingViewModel.GetType() != vm.GetType())
                {
                    existingViewModel.Dispose();
                    this.Devices.Remove(existingViewModel);
                    this.Devices.Add(vm);
                    this.DeviceSelectionSubscriber?.NotifyUpdated(vm);
                    this.NotifyDevicesNetworkChanged(vm);
                }
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
