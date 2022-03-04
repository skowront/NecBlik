using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ZigBee.Core.Interfaces;
using ZigBee.Core.Models;
using ZigBee.Virtual.Factories;

namespace ZigBee.Virtual.Models
{
    public class VirtualZigBeeNetwork : ZigBeeNetwork
    {
        public VirtualZigBeeNetwork() : base()
        {
            this.ZigBeeFactory = new VirtualZigBeeFactory();
            this.internalNetworkType = this.ZigBeeFactory.GetVendorID();
        }

        public VirtualZigBeeNetwork(bool setupExampleNetwork) : this()
        {
            if (setupExampleNetwork)
                this.SetupExampleNetwork();
        }

        public void SetupExampleNetwork()
        {
            this.ZigBeeFactory = new VirtualZigBeeFactory();
            this.SetCoordinator(new VirtualZigBeeCoordinator(this.ZigBeeFactory, true));
            this.ZigBeeSources = new Collection<IZigBeeSource>(this.ZigBeeCoordinator.GetDevices().ToList());
            this.ZigBeeConnections = new Collection<Tuple<string, string>>(this.ZigBeeCoordinator.GetConnections().ToList());
        }

        public override void AddSource(IZigBeeSource source)
        {
            if (this.HasCoordinator)
            {
                var devs = new List<IZigBeeSource>(this.ZigBeeCoordinator.GetDevices());
                devs.Add(source);
                this.ZigBeeCoordinator.SetDevices(devs);
                this.ZigBeeSources = new Collection<IZigBeeSource>(this.ZigBeeCoordinator.GetDevices().ToList());
            }
            else
            {
                this.ZigBeeSources.Add(source);
            }
        }
    }
}
