using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NecBlik.Common.WpfExtensions.Base;
using NecBlik.Core.GUI;
using NecBlik.Core.GUI.Interfaces;
using NecBlik.Core.GUI.ViewModels;
using NecBlik.Core.Models;
using NecBlik.Virtual.GUI.Factories;
using NecBlik.Virtual.GUI.Views;
using NecBlik.Virtual.Models;

namespace NecBlik.Virtual.GUI.ViewModels
{
    public class VirtualNetworkViewModel : NetworkViewModel
    {
        public ObservableCollection<VirtualDeviceViewModel> Devices { get; set; } = new ObservableCollection<VirtualDeviceViewModel>();

        public VirtualNetworkViewModel(Network network) : base(network)
        {

            this.EditResponseProvider = new GenericResponseProvider<string, NetworkViewModel>((q) => {
                Window window = new VirtualNetworkWindow(this);
                window.Show();
                return null;
            });

            this.SyncFromModel();
        }

        public virtual void SyncFromModel()
        {
            this.Devices.Clear();
           
            foreach (var device in this.model.DeviceSources)
            {
                var factory = new VirtualDeviceGuiFactory();
                var vm = factory.DeviceViewModelFromRules(new DeviceModel(device), this, this.model.DeviceSubtypeFactoryRules.ToList());
                vm.PullSelectionSubscriber = this.DeviceSelectionSubscriber;
                this.Devices.Add(vm);
            }
            this.GetCoordinatorViewModel();

            this.OnPropertyChanged();
        }

        public override DeviceViewModel GetCoordinatorViewModel()
        {
            if (this.coorinator == null)
            {
                var zvm = new DeviceModel(this.Model.Coordinator);
                var factory = new VirtualDeviceGuiFactory();
                var vm = factory.DeviceViewModelFromRule(zvm, this, this.model.DeviceCoordinatorSubtypeFactoryRule);
                this.coorinator = vm;
            }
            this.coorinator.PullSelectionSubscriber = DeviceSelectionSubscriber;
            this.DeviceSelectionSubscriber?.NotifyUpdated(this.coorinator);
            return this.coorinator;
        }

        public override IEnumerable<DeviceViewModel> GetDeviceViewModels()
        {
            return this.Devices;
        }

        public override void Sync()
        {
            base.Sync();
            this.SyncFromModel();
        }
    }
}
