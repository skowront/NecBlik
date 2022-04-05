using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NecBlik.Core.Models;
using NecBlik.PyDigi.Factories;

namespace NecBlik.PyDigi.Models
{
    public class PyDigiZigBeeSource: Device
    {

        private dynamic pyDevice = null;

        public PyDigiZigBeeSource(dynamic device)
        {
            this.pyDevice = device;
            this.internalType = (new PyDigiZigBeeFactory()).GetVendorID();
        }

        public override string GetAddress()
        {
            using (Python.Runtime.Py.GIL())
                return this.pyDevice.get_64bit_addr().ToString();
        }

        public override string GetName()
        {
            return this.GetAddress();
        }

        public override string GetVersion()
        {
            using (Python.Runtime.Py.GIL())
                return this.pyDevice.get_hardware_version().description;
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
