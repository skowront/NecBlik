using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NecBlik.Core.GUI.ViewModels;
using NecBlik.Core.Models;
using NecBlik.Virtual.GUI.ViewModels;

namespace NecBlik.PyDigi.GUI.ViewModels
{
    public class PyDigiZigBeeViewModel: VirtualDeviceViewModel
    {
        public PyDigiZigBeeViewModel(DeviceModel model = null, NetworkViewModel networkModel = null) : base(model, networkModel)
        {
            this.Icon = FontAwesome.WPF.FontAwesomeIcon.Desktop;
        }

        public override string GetCacheId()
        {
            return this.Model.CacheId;
        }

        public override void OnDataRecieved(string data, string sourceAddress)
        {
            this.AddIncomingHistoryBufferEntry(data, sourceAddress);
        }

        public override void OnDataSent(string data, string sourceAddress)
        {
            //Maybe poll for sent items?
        }

        public override void Send()
        {
            this.AddHistoryBufferEntry(Strings.SR.CantSendFromRemoteDevices);
        }
    }
}
