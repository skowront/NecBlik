﻿using System;
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
                return this.model.ZigBeeCoordinator.PanId; 
            }
            set { 
                this.model.ZigBeeCoordinator.PanId = value; this.OnPropertyChanged(); 
            }
        }

        public ZigBeeViewModel ZigBeeCoordinator
        {
            get 
            {
                return this.GetZigBeeCoordinatorViewModel();
            }
        }

        public ISelectionSubscriber<ZigBeeViewModel> ZigBeeSelectionSubscriber { get; set; }

        public RelayCommand EditCommand { get; set; }
        
        public IResponseProvider<string,ZigBeeNetworkViewModel> EditResponseProvider { get; set; }

        public ZigBeeNetworkViewModel(ZigBeeNetwork network)
        {
            this.model = network;
            this.BuildCommands();
        }

        public virtual ZigBeeViewModel GetZigBeeCoordinatorViewModel()
        {
            var zvm = new ZigBeeModel(this.Model.ZigBeeCoordinator);
            var vm = new ZigBeeViewModel(zvm);
            vm.PullSelectionSubscriber = ZigBeeSelectionSubscriber;
            return vm;    
        }

        public virtual IEnumerable<ZigBeeViewModel> GetZigBeeViewModels()
        {
            return new List<ZigBeeViewModel>();
        }

        public virtual void Sync()
        {

        }

        private void BuildCommands()
        {
            this.EditCommand = new RelayCommand((o) => {
                this.EditResponseProvider?.ProvideResponse(this);
            });
        }
    }
}
