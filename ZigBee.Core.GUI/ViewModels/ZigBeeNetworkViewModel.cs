using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Common.WpfExtensions.Base;
using ZigBee.Common.WpfExtensions.Interfaces;
using ZigBee.Core.Models;

namespace ZigBee.Core.GUI.ViewModels
{
    public class ZigBeeNetworkViewModel : BaseViewModel
    {
        private ZigBeeNetwork model { get; set; }

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

        public RelayCommand EditCommand { get; set; }
        
        public IResponseProvider<string,ZigBeeNetworkViewModel> EditResponseProvider { get; set; }

        public ZigBeeNetworkViewModel(ZigBeeNetwork network)
        {
            this.model = network;
            this.BuildCommands();
        }

        private void BuildCommands()
        {
            this.EditCommand = new RelayCommand((o) => {
                this.EditResponseProvider?.ProvideResponse(this);
            });
        }
    }
}
