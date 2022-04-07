using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XBeeLibrary.Core;
using NecBlik.Core.Models;
using NecBlik.Digi.Factories;
using NecBlik.Core.Enums;

namespace NecBlik.Digi.Models
{
    public class DigiZigBeeSource : Device
    {
        private RemoteXBeeDevice zigBeeDevice { get;set;}
        public DigiZigBeeSource(RemoteXBeeDevice zigBeeDevice)
        {
            this.zigBeeDevice = zigBeeDevice;   
            this.internalType = (new DigiZigBeeFactory()).GetVendorID();
        }

        public override string GetAddress()
        {
            return this.zigBeeDevice?.GetAddressString() ?? String.Empty;
        }

        public override string GetName()
        {
            return this.GetAddress();
        }

        public override string GetVersion()
        {
            return zigBeeDevice.GetLocalXBeeDevice().HardwareVersion.Description;
        }

        public override string GetCacheId()
        {
            return this.zigBeeDevice.GetAddressString();
        }

        public override void Send(string data, string address)
        {

        }

        public override void OnDataSent(string data, string sourceAddress)
        {
            //Maybe check for possible response?
        }
    }
}
