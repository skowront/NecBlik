using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XBeeLibrary.Core;
using XBeeLibrary.Core.Connection;
using ZigBee.Virtual.Models;
using ZigBee.Digi.Factories;
using ZigBee.Core.Interfaces;
using ZigBee.Digi.USB;
using Newtonsoft.Json;

namespace ZigBee.Digi.Models
{

    [JsonObject(MemberSerialization.OptIn)]
    public class DigiZigBeeUSBCoordinator: VirtualZigBeeCoordinator
    {
        private ZigBeeDevice zigBee;

        [JsonProperty]
        public DigiUSBConnectionData connectionData { get; set; }

        public DigiZigBeeUSBCoordinator(IZigBeeFactory zigBeeFactory, DigiUSBConnectionData connectionData = null) : base(zigBeeFactory)
        {
            this.zigBeeFactory = new DigiZigBeeFactory();
            this.internalType = this.zigBeeFactory.GetVendorID();
            this.connectionData = connectionData ?? new() { port = string.Empty, baud = 9600 };
            this.Name = "Digi Coordinator";
        }

        public override IEnumerable<IZigBeeSource> GetDevices()
        {
            if(this.connectionData.port==string.Empty || this.connectionData.port == null)
            {
                return null;
            }
            this.zigBee = new ZigBeeDevice(new WinSerialPort(connectionData.port, connectionData.baud));
            this.zigBee.Open();
            var network = this.zigBee.GetNetwork();
            var options = new HashSet<XBeeLibrary.Core.Models.DiscoveryOptions>();
            options.Add(XBeeLibrary.Core.Models.DiscoveryOptions.DISCOVER_MYSELF);
            options.Add(XBeeLibrary.Core.Models.DiscoveryOptions.APPEND_DD);
            network.SetDiscoveryOptions(options);
            network.SetDiscoveryTimeout(25000L);
            network.DiscoveryFinished += Network_DiscoveryFinished;
            network.DeviceDiscovered += Network_DeviceDiscovered;
            network.StartNodeDiscoveryProcess();
            while (network.IsDiscoveryRunning)
            {
                Thread.Sleep(500);
            }
            var nodes = network.GetDevices();
            this.zigBee.Close();
            List<IZigBeeSource> list = new();
            foreach (var node in nodes)
            {
                var ZigBeeSource = new DigiZigBeeSource(node);
                list.Add(ZigBeeSource);
            }
            return list;
        }

        private void Network_DeviceDiscovered(object sender, XBeeLibrary.Core.Events.DeviceDiscoveredEventArgs e)
        {
        }

        private void Network_DiscoveryFinished(object sender, XBeeLibrary.Core.Events.DiscoveryFinishedEventArgs e)
        {
        }

        public override void Save(string folderPath)
        {
            File.WriteAllText(folderPath + "\\" + "Coordinator.json", JsonConvert.SerializeObject(this.connectionData, Formatting.Indented));
        }

        public override string GetVersion()
        {
            return this.zigBee.HardwareVersion?.Description;
        }

        [JsonObject(MemberSerialization.OptIn)]
        public class DigiUSBConnectionData
        {
            [JsonProperty]
            public int baud;

            [JsonProperty]
            public string port = string.Empty;
        }
    }
}
