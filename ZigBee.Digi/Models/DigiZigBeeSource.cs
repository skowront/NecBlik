using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XBeeLibrary.Core;
using ZigBee.Core.Models;
using ZigBee.Digi.Factories;


namespace ZigBee.Digi.Models
{
    public class DigiZigBeeSource : ZigBeeSource
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
    }
}
