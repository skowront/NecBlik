using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Common.WpfExtensions.Base;
using ZigBee.Common.WpfExtensions.Interfaces;
using ZigBee.Core.Interfaces;
using ZigBee.Core.Models;

namespace ZigBee.Core.GUI
{
    public class ZigBeeViewModel:BaseViewModel,IDuplicable<ZigBeeViewModel>
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

        public RelayCommand EditCommand { get; set; }

        public ZigBeeViewModel( ZigBeeModel model = null)
        {
            this.Model = model;
            if(this.Model == null)
            {
                this.Model = new ZigBeeModel();
                return;
            }
        }

        public ZigBeeViewModel Duplicate()
        {
            return new ZigBeeViewModel(this.Model.Duplicate());
        }

        protected virtual void BuildCommands()
        {
            this.EditCommand = new RelayCommand((o) => { });
        }
    }
}
