using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NecBlik.Core.Interfaces;
using NecBlik.Virtual.Models;
using NecBlik.Core.Models;
using NecBlik.ZigBeeNet.Factories;
using ZigBeeNet.Hardware.Digi.XBee;
using ZigBeeNet;

namespace NecBlik.ZigBeeNet.Models
{
    public class ZBNetSource : Device
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

        public override void Send(string data, string address)
        {
            base.Send(data, address);
        }
    }
}
