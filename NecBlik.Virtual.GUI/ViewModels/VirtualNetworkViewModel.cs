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
using System.ComponentModel;
using System.Threading;
using NecBlik.Common.WpfElements.PopupValuePickers;

namespace NecBlik.Virtual.GUI.ViewModels
{
    public class VirtualNetworkViewModel : NetworkViewModel
    {
        public ObservableCollection<VirtualDeviceViewModel> Devices { get; set; } = new ObservableCollection<VirtualDeviceViewModel>();

        public static readonly List<string> CustomizableDeviceProperties = new List<string> { VirtualDeviceGuiFactory.DeviceViewModelRuledProperties.ViewModel,
                                                                                              VirtualDeviceGuiFactory.DeviceViewModelRuledProperties.MapControl};

        public int PollingInterval
        {
            get { return this.model.PollingInterval; }
            set { this.model.PollingInterval = value < 100 ? 100 : value ; this.OnPropertyChanged(); }
        }

        protected bool cancelPolling = false;
        protected bool holdPolling = false;

        protected BackgroundWorker statusPollingWorker = new BackgroundWorker();

        public VirtualNetworkViewModel(Network network) : base(network)
        {
            this.statusPollingWorker.DoWork += StatusPollingWorker_DoWork;
            this.statusPollingWorker.RunWorkerAsync();

            this.EditResponseProvider = new GenericResponseProvider<string, NetworkViewModel>((q) => {
                Window window = new VirtualNetworkWindow(this);
                window.Show();
                return null;
            });

            this.SyncFromModel();
            this.buildCommands();
            this.BuildResponseProviders();
        }

        ~VirtualNetworkViewModel()
        {
            this.cancelPolling = true; 
        }

        private async void StatusPollingWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while(!this.cancelPolling)
            {
                if(!holdPolling)
                {
                    foreach (var item in this.Devices)
                    {
                        Application.Current?.Dispatcher.Invoke(async () =>
                        {
                            var coord = this.Coordinator?.Model?.DeviceSource as IDeviceCoordinator;
                            if (coord != null)
                            {
                                item.Status = await coord.GetStatusOf(item.Address);
                            }
                        });
                    }
                    Application.Current?.Dispatcher.Invoke(async () =>
                    {
                        var coord = this.Coordinator?.Model?.DeviceSource as IDeviceCoordinator;
                        if (coord != null)
                        {
                            this.coorinator.Status = await coord.GetStatusOf(this.Coordinator.Address);
                        }
                    });
                    Thread.Sleep(this.model.PollingInterval);
                }
            }
        }

        private void buildCommands()
        {
            this.DiscoverCommand = new RelayCommand((o) =>
            {
                var popup = new SimpleInputPopup(Strings.SR.EnterAddressOrLeaveEmpty, string.Empty , null,null);
                var rp = new InputResponseProvider(popup);
                var result = rp.ProvideResponse();
                if (result == string.Empty)
                {
                    var dev = (new VirtualDevice(true));
                    this.AddNewDevice(dev);
                }
                else if (result == null)
                    return;
                else
                {
                    var dev = (new VirtualDevice(false) { Address = result });
                    this.AddNewDevice(dev);
                }
            });

            this.RemoveDeviceCommand = new RelayCommand((o) =>
            {
                if (this.model.DeviceSources.Contains((o as VirtualDeviceViewModel).Model.DeviceSource))
                {
                    this.model.DeviceSources.Remove((o as VirtualDeviceViewModel).Model.DeviceSource);
                }
                (o as VirtualDeviceViewModel).Dispose();
                this.Devices.Remove(o as VirtualDeviceViewModel);
                this.NotifyDevicesNetworkChanged(o as VirtualDeviceViewModel);
            });

            this.EditCoordinatorRuleCommand = new RelayCommand((o) =>
            {
                var rp = new ListInputValuePicker();
                var result = rp.ProvideResponse(new Tuple<string, IEnumerable<string>>(NecBlik.Virtual.GUI.Factories.VirtualDeviceGuiFactory.DeviceViewModelRuledProperties.ViewModel, this.AvailableValueProvider.ProvideResponse(this.model.DeviceCoordinatorSubtypeFactoryRule)));
                if (result == string.Empty || result == null)
                    return;
                if (result != this.model.DeviceCoordinatorSubtypeFactoryRule.Value)
                {
                    this.model.DeviceCoordinatorSubtypeFactoryRule.Value = result;
                    this.coorinator = null;
                    this.Coordinator = this.GetCoordinatorViewModel();
                }
                this.OnFactoryEditClosed();
            });
        }

        protected void NotifyDevicesNetworkChanged(VirtualDeviceViewModel changedDevice)
        {
            foreach(var item in this.Devices)
            {
                item.NetworkChanged();
            }
            this.Coordinator?.NetworkChanged();
            if(changedDevice != null)
            {
                changedDevice.NetworkChanged();
            }
        }

        protected virtual void BuildResponseProviders()
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
            //this.Devices.Clear();
           
            foreach (var device in this.model.DeviceSources)
            {
                this.AddNewDevice(device);
            }
            this.GetCoordinatorViewModel();

            this.OnPropertyChanged();
        }

        public override bool AddNewDevice(IDeviceSource device)
        {
            var factory = new VirtualDeviceGuiFactory();
            var vm = factory.DeviceViewModelFromRules(new DeviceModel(device), this, this.model.DeviceSubtypeFactoryRules.ToList());
            if (vm == null)
                return false;   
            vm.PullSelectionSubscriber = this.DeviceSelectionSubscriber;
            if(!this.model.DeviceSources.Contains(device))
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
                if(existingViewModel.GetType()!=vm.GetType())
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

        public override IEnumerable<Core.Interfaces.IDeviceSource> GetSources()
        {
            return this.model.DeviceSources;
        }

        public override void Sync()
        {
            base.Sync();
            this.SyncFromModel();
        }

        public override void Dispose()
        {
            base.Dispose();
            this.cancelPolling = true;
            foreach (var device in this.Devices)
            {
                device.Dispose();
            }
        }

        public virtual void Hold()
        {
            this.holdPolling = true;
        }

        public virtual void Unhold()
        {
            this.holdPolling = false;   
        }
    }
}
