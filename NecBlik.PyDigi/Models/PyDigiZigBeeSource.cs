using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NecBlik.Core.Models;

namespace NecBlik.PyDigi.Models
{
    public class PyDigiZigBeeSource: Device
    {

        private dynamic pyDevice = null;

        public PyDigiZigBeeSource(dynamic device)
        {
            this.pyDevice = device; 
        }

        public override string GetAddress()
        {
            return this.pyDevice.get_64bit_addr().ToString();
        }

        public override string GetName()
        {
            return this.GetAddress();
        }

        public override string GetVersion()
        {
            return this.pyDevice.get_firmware_version();
        }

        public override string GetCacheId()
        {
            return this.GetAddress();
        }

        public override void Send(string data, string address)
        {
            
        }

        public override void OnDataSent(string data, string sourceAddress)
        {
            
        }
    }
}
