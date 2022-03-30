using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Core.Models;

namespace ZigBee.PyDigi.Models
{
    public class PyDigiZigBeeSource: ZigBeeSource
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
    }
}
