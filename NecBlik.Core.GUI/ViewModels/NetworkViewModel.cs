﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NecBlik.Common.WpfExtensions.Base;
using NecBlik.Common.WpfExtensions.Interfaces;
using NecBlik.Core.Factories;
using NecBlik.Core.GUI.Factories.ViewModels;
using NecBlik.Core.GUI.Interfaces;
using NecBlik.Core.Models;
using NecBlik.Core.GUI.Views;
using NecBlik.Core.Interfaces;
using NecBlik.Common.WpfElements.PopupValuePickers;

namespace NecBlik.Core.GUI.ViewModels
{
    public class NetworkViewModel : BaseViewModel, IDisposable
    {
        protected Network model { get; set; }

        public Network Model
        {
            get { return this.model; }
            set { this.model = value; this.OnPropertyChanged(); }
        }

        public string Name
        {
            get { return this.model.Name; }
            set { this.model.Name = value; this.OnPropertyChanged(); }
        }

        public Guid Guid
        {
            get { return this.model.Guid; }
        }

        public string PanId
        {
            get
            {
                return this.model.PanId;
            }
            set
            {
                this.model.PanId = value; this.OnPropertyChanged();
            }
        }

        public string InternalSubType
        {
            get { return this.model.InternalSubType; }
            set { this.model.InternalSubType = value; this.OnPropertyChanged(); }
        }

        private bool isOpen = false;
        public bool IsOpen
        {
            get { return this.isOpen; }
            set
            {
                this.isOpen = value;
                if (isOpen == true)
                {
                    if (!this.Open())
                        this.isOpen = false;
                    else
                        this.isOpen = true;
                }
                else
                {
                    this.Close();
                }
                this.OnPropertyChanged();
            }
        }

        protected IResponseProvider<List<string>, FactoryRule> AvailableCacheObjectIDsProvider =
                new GenericResponseProvider<List<string>, FactoryRule>((rule) =>
                {
                    return null;
                });

        protected IResponseProvider<List<string>, FactoryRule> AvailablePropertyProvider =
                new GenericResponseProvider<List<string>, FactoryRule>((rule) =>
                {
                    return null;
                });

        protected IResponseProvider<List<string>, FactoryRule> AvailableValueProvider =
                new GenericResponseProvider<List<string>, FactoryRule>((rule) =>
                {
                    return null;
                });

        public ObservableCollection<FactoryRuleViewModel> FactoryRules = new ObservableCollection<FactoryRuleViewModel>();

        protected DeviceViewModel coorinator = null;
        public DeviceViewModel Coordinator
        {
            get
            {
                return this.GetCoordinatorViewModel();
            }
            set
            {
                this.coorinator = value; this.OnPropertyChanged();
            }
        }

        public ISelectionSubscriber<DeviceViewModel> DeviceSelectionSubscriber { get; set; }

        public RelayCommand EditCommand { get; set; }
        public RelayCommand RefreshCommand { get; set; }
        public RelayCommand DiscoverCommand { get; set; }
        public RelayCommand EditRulesCommand { get; set; }
        public RelayCommand EditCoordinatorRuleCommand { get; set; }
        public RelayCommand RemoveDeviceCommand { get; set; }
        public RelayCommand PingCommand { get; set; }
        public RelayCommand PollDevicesCommand { get; set; }

        public IResponseProvider<string, NetworkViewModel> EditResponseProvider { get; set; }

        public NetworkViewModel(Network network)
        {
            this.model = network;
            this.model.CoordinatorChanged = new Action(() =>
            {
                this.GetCoordinatorViewModel();
            });

            foreach (var item in this.model.DeviceSubtypeFactoryRules)
            {
                this.FactoryRules.Add(new FactoryRuleViewModel(item));
            }

            this.BuildCommands();
        }

        public virtual DeviceViewModel GetCoordinatorViewModel()
        {
            if (this.coorinator == null)
            {
                var zvm = new DeviceModel(this.Model.Coordinator);
                this.coorinator = new DeviceViewModel(zvm, this);
                this.coorinator.PullSelectionSubscriber = DeviceSelectionSubscriber;
                DeviceSelectionSubscriber?.NotifyUpdated(this.coorinator);
            }
            return this.coorinator;
        }

        public virtual IEnumerable<DeviceViewModel> GetDeviceViewModels()
        {
            return new List<DeviceViewModel>();
        }

        public virtual IEnumerable<IDeviceSource> GetSources()
        {
            return new List<GhostDevice>();
        }

        public virtual void Sync()
        {
            this.model.DeviceSubtypeFactoryRules.Clear();
            foreach (var item in this.FactoryRules)
            {
                this.model.DeviceSubtypeFactoryRules.Add(item.Model);
            }
        }

        public virtual bool AddNewDevice(IDeviceSource device)
        {
            return false;
        }

        public virtual async Task Discover()
        {

        }

        public virtual bool Open()
        {
            return this.Coordinator.Model.DeviceSource.Open();
        }

        public virtual void Close()
        {
            this.Coordinator.Model.DeviceSource.Close();
        }

        private void BuildCommands()
        {
            this.EditCommand = new RelayCommand((o) =>
            {
                this.EditResponseProvider?.ProvideResponse(this);
            });

            this.RefreshCommand = new RelayCommand((o) =>
            {
                this.Sync();
            });

            this.DiscoverCommand = new RelayCommand(async (o) =>
            {
                await this.Discover();
            });

            this.EditRulesCommand = new RelayCommand((o) =>
            {
                var rp = new GenericResponseProvider<ObservableCollection<FactoryRuleViewModel>, object>((o) => { return this.FactoryRules; });
                var window = new FactoryRulesEditor(rp, AvailableCacheObjectIDsProvider, AvailablePropertyProvider, AvailableValueProvider, new Action(() => { this.OnFactoryEditClosed(); }));
                window.Show();
            });

            this.EditCoordinatorRuleCommand = new RelayCommand((o) =>
            {
                
            });

            this.RemoveDeviceCommand = new RelayCommand((o) =>
            {

            });

            this.PingCommand = new RelayCommand((o) =>
            {
                if (this.coorinator == null)
                    return;
                if (this.coorinator.Model == null)
                    return;
                if (this.coorinator.Model.DeviceSource is IDeviceCoordinator)
                {
                    var window = new PingCoordinatorWindow(new PingViewModel(this.coorinator.Model.DeviceSource as IDeviceCoordinator, this.GetDeviceViewModels().Select((o) => { return o.Address; }).ToList()));
                    window.Show();
                }
            });

            this.PollDevicesCommand = new RelayCommand((o) =>
            {

            });
        }

        public virtual void OnFactoryEditClosed()
        {

        }

        public virtual void Dispose()
        {
            this.Coordinator?.Dispose();
        }
    }
}
