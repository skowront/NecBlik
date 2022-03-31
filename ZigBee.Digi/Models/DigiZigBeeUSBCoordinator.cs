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
using ZigBee.Common.WpfExtensions.Interfaces;

namespace ZigBee.Digi.Models
{

    [JsonObject(MemberSerialization.OptIn)]
    public class DigiZigBeeUSBCoordinator : VirtualZigBeeCoordinator
    {
        private ZigBeeDevice zigBee;

        [JsonProperty]
        public DigiUSBConnectionData connectionData { get; set; }

        IUpdatableResponseProvider<int, bool, string> progressResponseProvider = null;

        private const int sleepTime = 500;

        private const long timeout = 25000L;

        private const int maxProgress = (int)timeout / sleepTime;

        private bool discoveryFinished = false;

        public DigiZigBeeUSBCoordinator(IZigBeeFactory zigBeeFactory, DigiUSBConnectionData connectionData = null) : base(zigBeeFactory)
        {
            this.zigBeeFactory = new DigiZigBeeFactory();
            this.internalType = this.zigBeeFactory.GetVendorID();
            this.connectionData = connectionData ?? new() { port = string.Empty, baud = 9600 };
            this.Name = Resources.Resources.DefaultDigiCoordinatorName;
            this.zigBee = new ZigBeeDevice(new WinSerialPort(connectionData.port, connectionData.baud));
            if (!this.zigBee.IsOpen)
                this.zigBee.Open();
            this.zigBee.DataReceived += ZigBeeDataReceived;
        }

        private void ZigBeeDataReceived(object? sender, XBeeLibrary.Core.Events.DataReceivedEventArgs e)
        {
            this.OnDataRecieved(e.DataReceived.DataString, e.DataReceived.Device.GetAddressString());
        }

        public override async Task<IEnumerable<IZigBeeSource>> GetDevices(IUpdatableResponseProvider<int, bool, string> progressResponseProvider = null)
        {
            this.progressResponseProvider = progressResponseProvider;
            if (this.connectionData.port == string.Empty || this.connectionData.port == null)
            {
                return null;
            }
            progressResponseProvider?.Init(0, DigiZigBeeUSBCoordinator.maxProgress, 0);
            var progress = 0;
            var network = this.zigBee.GetNetwork();
            var options = new HashSet<XBeeLibrary.Core.Models.DiscoveryOptions>();
            options.Add(XBeeLibrary.Core.Models.DiscoveryOptions.APPEND_DD);
            network.SetDiscoveryOptions(options);
            network.SetDiscoveryTimeout(25000L);
            network.DiscoveryFinished += Network_DiscoveryFinished;
            network.DeviceDiscovered += Network_DeviceDiscovered;
            network.StartNodeDiscoveryProcess();
            var task = Task.Run(() =>
            {
                while (network.IsDiscoveryRunning && this.discoveryFinished == false)
                {
                    Thread.Sleep(sleepTime);
                    progress++;
                    progressResponseProvider?.Update(progress);
                }
                this.discoveryFinished = false;
            });
            await task;
            this.progressResponseProvider?.Update(DigiZigBeeUSBCoordinator.maxProgress);
            progressResponseProvider?.SealUpdates();
            var nodes = network.GetDevices();
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
            this.discoveryFinished = true;
        }

        public override void Save(string folderPath)
        {
            File.WriteAllText(folderPath + "\\" + Resources.Resources.CoordinatorFile, JsonConvert.SerializeObject(this.connectionData, Formatting.Indented));
        }

        public override string GetVersion()
        {
            return this.zigBee.HardwareVersion?.Description;
        }

        public override string GetAddress()
        {
            if (!this.zigBee.IsOpen)
                this.zigBee.Open();
            var r = this.zigBee.XBee64BitAddr.ToString();
            return r;
        }

        public override string GetPanID()
        {
            if (!this.zigBee.IsOpen)
                this.zigBee.Open();
            var r = this.zigBee.GetPANID();
            return Convert.ToHexString(r);
        }

        public override string GetCacheId()
        {
            return Resources.Resources.CoordinatorCachePrefix + this.zigBee?.GetAddressString();
        }

        public override void Send(string data, string address)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(data+"\0");
            if (address == string.Empty)
            {
                this.zigBee.SendBroadcastData(bytes);
            }
            else
            {
                var remote = this.zigBee.GetNetwork().GetDevices().Where((dev) => { return dev.GetAddressString() == address; });
                if (remote.Count() >= 1)
                {
                    if (!this.zigBee.IsOpen)
                        this.zigBee.Open();
                    this.zigBee.SendData(remote.First(), bytes);
                }
            }
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
