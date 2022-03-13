using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Core.Interfaces;
using ZigBee.Virtual.Models;
using ZigBee.Core.Models;
using ZigBee.ZigBeeNet.Factories;
using ZigBeeNet.Hardware.Digi.XBee;
using ZigBeeNet;

namespace ZigBee.ZigBeeNet.Models
{
    public class ZBNetSource : ZigBeeSource
    {
        ZigBeeNode zigBeeNode;

        public string Version { get; set; }

        public ZBNetSource(ZigBeeNode zigBeeNode)
        {
            this.zigBeeNode = zigBeeNode;
            this.internalType = (new ZBNetFactory()).GetVendorID();
        }

        public override string GetAddress()
        {
            return zigBeeNode?.IeeeAddress.ToString();
        }


        public override string GetName()
        {
            return this.GetAddress();
        }

        public override string GetVersion()
        {
            return this.Version;
        }

        public override string GetCacheId()
        {
            return this.GetAddress();
        }
    }
}
