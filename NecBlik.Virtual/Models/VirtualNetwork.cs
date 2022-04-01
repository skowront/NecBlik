using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NecBlik.Core.Interfaces;
using NecBlik.Core.Models;
using NecBlik.Virtual.Factories;

namespace NecBlik.Virtual.Models
{
    public class VirtualNetwork : Network
    {
        public VirtualNetwork() : base()
        {
            this.DeviceFactory = new VirtualDeviceFactory();
            this.internalNetworkType = this.DeviceFactory.GetVendorID();
        }

        public VirtualNetwork(bool setupExampleNetwork) : this()
        {
            if (setupExampleNetwork)
                this.SetupExampleNetwork();
        }

        public async void SetupExampleNetwork()
        {
            this.DeviceFactory = new VirtualDeviceFactory();
            await this.SetCoordinator(new VirtualCoordinator(this.DeviceFactory, true));
            this.DeviceSources = new Collection<IDeviceSource>((await this.Coordinator.GetDevices()).ToList());
            this.Connections = new Collection<Tuple<string, string>>(this.Coordinator.GetConnections().ToList());
        }

        public override async void AddSource(IDeviceSource source)
        {
            if (this.HasCoordinator)
            {
                var devs = new List<IDeviceSource>(await this.Coordinator.GetDevices());
                devs.Add(source);
                this.Coordinator.SetDevices(devs);
                this.DeviceSources = new Collection<IDeviceSource>((await this.Coordinator.GetDevices()).ToList());
            }
            else
            {
                this.DeviceSources.Add(source);
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
