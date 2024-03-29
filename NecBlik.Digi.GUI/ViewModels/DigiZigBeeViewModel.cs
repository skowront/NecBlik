﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NecBlik.Core.GUI.ViewModels;
using NecBlik.Core.Interfaces;
using NecBlik.Core.Models;
using NecBlik.Virtual.GUI.ViewModels;

namespace NecBlik.Digi.GUI.ViewModels
{
    public class DigiZigBeeViewModel : VirtualDeviceViewModel
    {
        public DigiZigBeeViewModel(DeviceModel model = null, NetworkViewModel networkModel = null) : base(model, networkModel)
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
            if (this.Network?.Model?.Coordinator != null)
            {
                this.AddOutgoingHistoryBufferEntry(this.OutputBuffer, this.Address);
                this.Network?.Model?.Coordinator?.Send(this.OutputBuffer, this.Address);
            }
            else
                this.AddHistoryBufferEntry(Strings.SR.CantSendFromRemoteDevices);
        }
    }
}
