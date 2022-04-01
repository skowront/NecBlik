using Newtonsoft.Json;
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

namespace NecBlik.Core.GUI.ViewModels
{
    public class NetworkViewModel : BaseViewModel
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
            get { 
                return this.model.PanId; 
            }
            set { 
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
            set {
                this.isOpen = value; 
                if(isOpen == true)
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

        public ObservableCollection<FactoryRuleViewModel> DevicesSubtypeFactoryRules = new ObservableCollection<FactoryRuleViewModel>();

        protected DeviceViewModel coorinator = null;
        public DeviceViewModel Coordinator
        {
            get 
            {
                return this.GetCoordinatorViewModel();
            }
        }

        public ISelectionSubscriber<DeviceViewModel> DeviceSelectionSubscriber { get; set; }

        public RelayCommand EditCommand { get; set; }
        public RelayCommand RefreshCommand { get; set; }
        public RelayCommand DiscoverCommand { get; set; }
        public RelayCommand EditRulesCommand { get; set; }
        
        public IResponseProvider<string,NetworkViewModel> EditResponseProvider { get; set; }

        public NetworkViewModel(Network network)
        {
            this.model = network;
            this.model.CoordinatorChanged = new Action(() =>
            {
                this.GetCoordinatorViewModel();
            });

            foreach(var item in this.model.DeviceSubtypeFactoryRules)
            {
                this.DevicesSubtypeFactoryRules.Add(new FactoryRuleViewModel(item));
            }

            this.BuildCommands();
        }

        public virtual DeviceViewModel GetCoordinatorViewModel()
        {
            if(this.coorinator==null)
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

        public virtual void Sync()
        {
            this.model.DeviceSubtypeFactoryRules.Clear();
            foreach (var item in this.DevicesSubtypeFactoryRules)
            {
                this.model.DeviceSubtypeFactoryRules.Add(item.Model);
            }
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
            this.EditCommand = new RelayCommand((o) => {
                this.EditResponseProvider?.ProvideResponse(this);
            });

            this.RefreshCommand = new RelayCommand((o) =>
            {
                this.Sync();
            });

            this.DiscoverCommand = new RelayCommand(async (o) =>
            {
                this.Discover();
            });

            this.EditRulesCommand = new RelayCommand((o) =>
            {
                var rp = new GenericResponseProvider<ObservableCollection<FactoryRuleViewModel>, object>((o) => { return this.DevicesSubtypeFactoryRules; });
                var window = new FactoryRulesEditor(rp, AvailableCacheObjectIDsProvider,AvailablePropertyProvider,AvailableValueProvider);
                window.Show();
            });
        }
    }
}
