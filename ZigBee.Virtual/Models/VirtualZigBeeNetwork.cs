using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Core.Interfaces;
using ZigBee.Core.Models;
using ZigBee.Virtual.Factories;

namespace ZigBee.Virtual.Models
{
    public class VirtualZigBeeNetwork:ZigBeeNetwork
    {
        public VirtualZigBeeNetwork()
        {
            this.ZigBeeFactory = new VirtualZigBeeFactory();
            this.internalNetworkType = this.ZigBeeFactory.GetVendorID();
        }

        public VirtualZigBeeNetwork(bool setupExampleNetwork) : this()
        {
            if(setupExampleNetwork)
                this.SetupExampleNetwork();
        }

        public void SetupExampleNetwork()
        {
            this.ZigBeeFactory = new VirtualZigBeeFactory();
            this.ZigBeeCoordinator = new VirtualZigBeeCoordinator(this.ZigBeeFactory,true);
            this.ZigBeeSources = new Collection<IZigBeeSource>(this.ZigBeeCoordinator.GetDevices().ToList());
            this.ZigBeeConnections = new Collection<Tuple<string, string>>(this.ZigBeeCoordinator.GetConnections().ToList());
        }
    }
}
