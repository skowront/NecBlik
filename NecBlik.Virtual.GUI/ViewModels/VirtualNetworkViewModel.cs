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
using NecBlik.Core.Factories;
using NecBlik.Common.WpfElements;
using NecBlik.Common.WpfElements.ResponseProviders;
using NecBlik.Core.Interfaces;

namespace NecBlik.Virtual.GUI.ViewModels
{
    public class VirtualNetworkViewModel : NetworkViewModel
    {
        public ObservableCollection<VirtualDeviceViewModel> Devices { get; set; } = new ObservableCollection<VirtualDeviceViewModel>();

        public static readonly List<string> CustomizableDeviceProperties = new List<string> { VirtualDeviceGuiFactory.DeviceViewModelRuledProperties.ViewModel,
                                                                                              VirtualDeviceGuiFactory.DeviceViewModelRuledProperties.MapControl};

        public VirtualNetworkViewModel(Network network) : base(network)
        {

            this.EditResponseProvider = new GenericResponseProvider<string, NetworkViewModel>((q) => {
                Window window = new VirtualNetworkWindow(this);
                window.Show();
                return null;
            });

            this.SyncFromModel();
            this.buildCommands();
            this.BuildResponseProviders();
        }

        private void buildCommands()
        {
            this.DiscoverCommand = new RelayCommand((o) =>
            {
                var popup = new SimpleInputPopup(Strings.SR.EnterAddressOrLeaveEmpty, string.Empty , null,null);
                var rp = new InputResponseProvider(popup);
                this.AddNewDevice((new VirtualDevice() { cachedAddress = rp.ProvideResponse() }));
            });

            this.RemoveDeviceCommand = new RelayCommand((o) =>
            {
                this.Devices.Remove(o as VirtualDeviceViewModel);
            });
        }

        private void BuildResponseProviders()
        {
            this.AvailableCacheObjectIDsProvider = new GenericResponseProvider<List<string>, FactoryRule>((rule) =>
            {
                var list = new List<string>(this.Devices.Select((o) => { return o.CacheId; }));
                if (this.model.HasCoordinator)
                    list.Add(this.Coordinator?.GetCacheId());
                return list;
            });

            this.AvailablePropertyProvider =
            new GenericResponseProvider<List<string>, FactoryRule>((rule) =>
            {
                if (this.model.HasCoordinator)
                    if (rule.CacheObjectId == this.Coordinator.GetCacheId())
                    {
                        var list = new List<string>(VirtualNetworkViewModel.CustomizableDeviceProperties);
                        list.Remove(VirtualDeviceGuiFactory.DeviceViewModelRuledProperties.ViewModel);
                        return list;
                    }
                return VirtualNetworkViewModel.CustomizableDeviceProperties;
            });


            this.AvailableValueProvider = new GenericResponseProvider<List<string>, FactoryRule>((rule) =>
            {
            var factory = new VirtualDeviceGuiFactory();
                if (rule.Property == VirtualDeviceGuiFactory.DeviceViewModelRuledProperties.ViewModel)
                {
                    return factory.GetAvailableDeviceViewModels();
                }
                else if(rule.Property== VirtualDeviceGuiFactory.DeviceViewModelRuledProperties.MapControl)
                {
                    return factory.GetAvailableControls();
                }
                else
                {
                    return null;
                }
            });
        }

        public virtual void SyncFromModel()
        {
            this.Devices.Clear();
           
            foreach (var device in this.model.DeviceSources)
            {
                this.AddNewDevice(device);
            }
            this.GetCoordinatorViewModel();

            this.OnPropertyChanged();
        }

        public override void AddNewDevice(IDeviceSource device)
        {
            var factory = new VirtualDeviceGuiFactory();
            var vm = factory.DeviceViewModelFromRules(new DeviceModel(device), this, this.model.DeviceSubtypeFactoryRules.ToList());
            vm.PullSelectionSubscriber = this.DeviceSelectionSubscriber;
            if(!this.model.DeviceSources.Contains(device))
            {
                this.model.DeviceSources.Add(device);
            }
            this.Devices.Add(vm);
            this.DeviceSelectionSubscriber?.NotifyUpdated(vm);
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

        public override void OnFactoryEditClosed()
        {
            base.OnFactoryEditClosed();
            this.Sync();
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
