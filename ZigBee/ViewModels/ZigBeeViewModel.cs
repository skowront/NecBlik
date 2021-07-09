using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Common.WpfExtensions.Base;
using ZigBee.Common.WpfExtensions.Interfaces;
using ZigBee.Models;

namespace ZigBee.ViewModels
{
    public class ZigBeeViewModel:BaseViewModel,IDuplicable<ZigBeeViewModel>
    {
        public ZigBeeModel Model;

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

        public ZigBeeViewModel(ZigBeeModel model = null)
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
    }
}
