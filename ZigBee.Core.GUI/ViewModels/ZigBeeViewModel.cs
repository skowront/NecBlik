using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Common.WpfExtensions.Base;
using ZigBee.Common.WpfExtensions.Interfaces;
using ZigBee.Core.GUI.Interfaces;
using ZigBee.Core.Interfaces;
using ZigBee.Core.Models;

namespace ZigBee.Core.GUI
{
    public class ZigBeeViewModel:BaseViewModel,IDuplicable<ZigBeeViewModel>,ICachable
    {
        public ZigBeeModel Model;

        public Guid Guid
        {
            get { return this.Model.Guid; }
        }

        public string Name
        {
            get { return this.Model.Name; }
            set { this.Model.Name = value; this.OnPropertyChanged(); }
        }

        public string Version
        {
            get { return this.Model.Version; }
            set { this.Model.Version = value; this.OnPropertyChanged(); }
        }

        public string InternalFactoryType
        {
            get 
            {
                if (this.Model.ZigBeeSource != null)
                    return this.Model.ZigBeeSource.GetVendorID();
                return this.Model.InternalFactoryType; 
            }
            set { this.Model.InternalFactoryType = value; this.OnPropertyChanged(); }
        }

        public string Address
        {
            get { return this.Model.AddressName; }
        }

        public ISelectionSubscriber<ZigBeeViewModel> PullSelectionSubscriber { get; set; }

        public RelayCommand EditCommand { get; set; }
        public RelayCommand SelectCommand { get; set; }

        public ZigBeeViewModel(ZigBeeModel model = null)
        {
            this.Model = model;
            if(this.Model == null)
            {
                this.Model = new ZigBeeModel();
                return;
            }
            this.BuildCommands();
        }

        public ZigBeeViewModel Duplicate()
        {
            var zb = new ZigBeeViewModel(this.Model.Duplicate());
            zb.SelectCommand = this.SelectCommand = new RelayCommand((o) => {
                this.PullSelectionSubscriber?.NotifySelected(zb);
            });
            return zb;
        }

        protected virtual void BuildCommands()
        {
            this.EditCommand = new RelayCommand((o) => { });
            this.SelectCommand = new RelayCommand((o) => { 
                this.PullSelectionSubscriber?.NotifySelected(this); 
            });
        }

        public string GetCacheId()
        {
            return this.Guid.ToString();
        }
    }
}
