using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NecBlik.Core.GUI.ViewModels;
using NecBlik.Core.Models;

namespace NecBlik.Digi.GUI.ViewModels
{
    public class DigiZigBeeCoordinatorViewModel:DigiZigBeeViewModel
    {
        public DigiZigBeeCoordinatorViewModel(ZigBeeModel model = null, ZigBeeNetworkViewModel networkModel = null) : base(model, networkModel)
        {

        }

        public override void Send()
        {
            if (this.SelectedDestinationAddress == this.Address)
            {
                this.OnDataRecieved(this.OutputBuffer, this.Address);
            }
            if (this.SelectedDestinationAddress == Core.GUI.Strings.SR.Broadcast)
            {
                var aaddr = this.GetAvailableDestinationAdresses();
                foreach (var item in aaddr)
                {
                    this.Model.ZigBeeSource.Send(this.OutputBuffer, item);
                    this.AddOutgoingHistoryBufferEntry(this.OutputBuffer, item);
                }
            }
            this.Model.ZigBeeSource.Send(this.OutputBuffer, this.SelectedDestinationAddress);
            this.AddOutgoingHistoryBufferEntry(this.OutputBuffer, this.SelectedDestinationAddress);
            this.OutputBuffer = string.Empty;
        }
    }
}
