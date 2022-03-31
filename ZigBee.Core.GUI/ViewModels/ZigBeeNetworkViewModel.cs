using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Common.WpfExtensions.Base;
using ZigBee.Common.WpfExtensions.Interfaces;
using ZigBee.Core.GUI.Interfaces;
using ZigBee.Core.Models;

namespace ZigBee.Core.GUI.ViewModels
{
    public class ZigBeeNetworkViewModel : BaseViewModel
    {
       

        protected ZigBeeNetwork model { get; set; }

        public ZigBeeNetwork Model
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

        protected ZigBeeViewModel zigBeeCoorinator = null;
        public ZigBeeViewModel ZigBeeCoordinator
        {
            get 
            {
                return this.GetZigBeeCoordinatorViewModel();
            }
        }

        public ISelectionSubscriber<ZigBeeViewModel> ZigBeeSelectionSubscriber { get; set; }

        public RelayCommand EditCommand { get; set; }
        public RelayCommand RefreshCommand { get; set; }
        public RelayCommand DiscoverCommand { get; set; }
        
        public IResponseProvider<string,ZigBeeNetworkViewModel> EditResponseProvider { get; set; }

        public ZigBeeNetworkViewModel(ZigBeeNetwork network)
        {
            this.model = network;
            this.model.CoordinatorChanged = new Action(() =>
            {
                this.GetZigBeeCoordinatorViewModel();
            });
            this.BuildCommands();
        }

        public virtual ZigBeeViewModel GetZigBeeCoordinatorViewModel()
        {
            if(this.zigBeeCoorinator==null)
            {
                var zvm = new ZigBeeModel(this.Model.ZigBeeCoordinator);
                this.zigBeeCoorinator = new ZigBeeViewModel(zvm, this);
                this.zigBeeCoorinator.PullSelectionSubscriber = ZigBeeSelectionSubscriber;
                ZigBeeSelectionSubscriber?.NotifyUpdated(this.zigBeeCoorinator);
            }
            return this.zigBeeCoorinator;    
        }

        public virtual IEnumerable<ZigBeeViewModel> GetZigBeeViewModels()
        {
            return new List<ZigBeeViewModel>();
        }

        public virtual void Sync()
        {

        }

        public virtual async Task Discover()
        {

        }

        public virtual bool Open()
        {
            return this.ZigBeeCoordinator.Model.ZigBeeSource.Open();
        }

        public virtual void Close()
        {
            this.ZigBeeCoordinator.Model.ZigBeeSource.Close();
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
            this.DiscoverCommand = new RelayCommand((o) =>
            {
                this.Discover();
            });
        }
    }
}
