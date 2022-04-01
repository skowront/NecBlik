using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NecBlik.Core.Interfaces;
using NecBlik.Core.Models;
using NecBlik.Virtual.Factories;

namespace NecBlik.Virtual.Models
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

        public async void SetupExampleNetwork()
        {
            this.ZigBeeFactory = new VirtualZigBeeFactory();
            await this.SetCoordinator(new VirtualZigBeeCoordinator(this.ZigBeeFactory, true));
            this.ZigBeeSources = new Collection<IZigBeeSource>((await this.ZigBeeCoordinator.GetDevices()).ToList());
            this.ZigBeeConnections = new Collection<Tuple<string, string>>(this.ZigBeeCoordinator.GetConnections().ToList());
        }

        public override async void AddSource(IZigBeeSource source)
        {
            if (this.HasCoordinator)
            {
                var devs = new List<IZigBeeSource>(await this.ZigBeeCoordinator.GetDevices());
                devs.Add(source);
                this.ZigBeeCoordinator.SetDevices(devs);
                this.ZigBeeSources = new Collection<IZigBeeSource>((await this.ZigBeeCoordinator.GetDevices()).ToList());
            }
            else
            {
                this.ZigBeeSources.Add(source);
            }
        }

        public static string generateAddress64bit()
        {
            Random random = new Random();
            const string chars = "0123456789ABCDEF";
            var value = new string(Enumerable.Repeat(chars, 16)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            while (value == "0000000000000000" || value == "000000000000FFFF" || TakenAddresses.Contains(value))
            {
                value = new string(Enumerable.Repeat(chars, 16)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            }
            return value;
        }

        public static Collection<string> TakenAddresses = new Collection<string>();
    }
}
