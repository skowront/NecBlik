using NecBlik.Core.GUI.ViewModels;
using NecBlik.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecBlik.PyDigi.GUI.ViewModels
{
    public class PyDigiZigBeeCoordinatorViewModel:PyDigiZigBeeViewModel
    {
        public PyDigiZigBeeCoordinatorViewModel(DeviceModel model = null, NetworkViewModel networkModel = null):base(model,networkModel)
        {

        }

        public override void Send()
        {
            if(this.SelectedDestinationAddress == this.Address)
            {
                this.AddOutgoingHistoryBufferEntry(this.OutputBuffer, this.Address);
                this.OnDataRecieved(this.OutputBuffer, this.Address);
                return;
            }
            if (this.SelectedDestinationAddress == Core.GUI.Strings.SR.Broadcast)
            {
                var aaddr = this.GetAvailableDestinationAdresses();
                foreach (var item in aaddr)
                {
                    this.Model.DeviceSource.Send(this.OutputBuffer, item);
                    this.AddOutgoingHistoryBufferEntry(this.OutputBuffer, item);
                }
                var sources = this.Network?.GetDeviceViewModels();
                if (sources != null)
                {
                    foreach (var source in sources)
                    {
                        source.OnDataSent(this.OutputBuffer, this.Address);
                    }
                }
            }
            this.Model.DeviceSource.Send(this.OutputBuffer, this.SelectedDestinationAddress);
            var vm = this.Network?.GetDeviceViewModels().Where((x) => { return x.Address == this.SelectedDestinationAddress; }).FirstOrDefault();
            this.AddOutgoingHistoryBufferEntry(this.OutputBuffer, this.SelectedDestinationAddress);
            if (vm != null)
                vm.OnDataSent(this.OutputBuffer, this.Address);
            else
                this.AddOutgoingHistoryBufferEntry(NecBlik.Core.GUI.Strings.SR.GPDeviceUnavailable, this.SelectedDestinationAddress);
            this.OutputBuffer = string.Empty;
        }

        public override void OnDataSent(string data, string sourceAddress)
        {
            base.OnDataRecieved(data, sourceAddress);
        }
    }
}
