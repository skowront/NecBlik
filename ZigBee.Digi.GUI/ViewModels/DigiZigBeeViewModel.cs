using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Core.GUI.ViewModels;
using ZigBee.Core.Interfaces;
using ZigBee.Core.Models;
using ZigBee.Virtual.GUI.ViewModels;

namespace ZigBee.Digi.GUI.ViewModels
{
    public class DigiZigBeeViewModel : VirtualZigBeeViewModel
    {
        public DigiZigBeeViewModel(ZigBeeModel model = null, ZigBeeNetworkViewModel networkModel = null) : base(model, networkModel)
        {

        }

        public override string GetCacheId()
        {
            return this.Model.CacheId;
        }

        public override void OnDataRecieved(string data, string sourceAddress)
        {
            this.AddIncomingHistoryBufferEntry(data, sourceAddress);
        }

       
        public override void Send()
        {
            if(this.SelectedDestinationAddress == this.Address)
            {
                this.OnDataRecieved(this.OutputBuffer, this.Address);
            }
            if(this.SelectedDestinationAddress == Core.GUI.Strings.SR.Broadcast)
            {
                var aaddr = this.GetAvailableDestinationAdresses();
                foreach (var item in aaddr)
                {
                    this.Model.ZigBeeSource.Send(this.OutputBuffer, item);
                    this.AddOutgoingHistoryBufferEntry(this.OutputBuffer, item);
                }
            }
            this.Model.ZigBeeSource.Send(this.OutputBuffer, this.SelectedDestinationAddress);
            this.AddOutgoingHistoryBufferEntry(this.OutputBuffer,this.SelectedDestinationAddress);
            this.OutputBuffer = string.Empty;
        }
    }
}
