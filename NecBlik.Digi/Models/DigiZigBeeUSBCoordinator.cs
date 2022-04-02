using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XBeeLibrary.Core;
using XBeeLibrary.Core.Connection;
using NecBlik.Virtual.Models;
using NecBlik.Digi.Factories;
using NecBlik.Core.Interfaces;
using NecBlik.Digi.USB;
using Newtonsoft.Json;
using NecBlik.Common.WpfExtensions.Interfaces;

namespace NecBlik.Digi.Models
{

    [JsonObject(MemberSerialization.OptIn)]
    public class DigiZigBeeUSBCoordinator : VirtualCoordinator
    {
        private ZigBeeDevice zigBee;
        private XBeeNetwork xBeeNetwork;

        [JsonProperty]
        public DigiUSBConnectionData connectionData { get; set; }

        IUpdatableResponseProvider<int, bool, string> progressResponseProvider = null;

        private const int sleepTime = 500;

        private const long timeout = 25000L;

        private const int maxProgress = (int)timeout / sleepTime;

        private bool discoveryFinished = false;

        public DigiZigBeeUSBCoordinator(IDeviceFactory zigBeeFactory, DigiUSBConnectionData connectionData = null) : base(zigBeeFactory)
        {
            this.deviceFactory = new DigiZigBeeFactory();
            this.internalType = this.deviceFactory.GetVendorID();
            this.connectionData = connectionData ?? new() { port = string.Empty, baud = 9600 };
            this.Name = Resources.Resources.DefaultDigiCoordinatorName;
            this.zigBee = new ZigBeeDevice(new WinSerialPort(connectionData.port, connectionData.baud));
            this.Open();
            this.zigBee.DataReceived += ZigBeeDataReceived;
        }

        ~DigiZigBeeUSBCoordinator()
        {
            this.zigBee?.Close();
        }

        private void ZigBeeDataReceived(object? sender, XBeeLibrary.Core.Events.DataReceivedEventArgs e)
        {
            this.OnDataRecieved(e.DataReceived.DataString, e.DataReceived.Device.GetAddressString());
        }

        public override async Task<IEnumerable<IDeviceSource>> GetDevices(IUpdatableResponseProvider<int, bool, string> progressResponseProvider = null)
        {
            if(!this.Open())
                return new List<IDeviceSource>();
            this.progressResponseProvider = progressResponseProvider;
            if (this.connectionData.port == string.Empty || this.connectionData.port == null)
            {
                return null;
            }

            await this.Discover();
            
            var nodes = this.xBeeNetwork.GetDevices();
            List<IDeviceSource> list = new();
            foreach (var node in nodes)
            {
                var ZigBeeSource = new DigiZigBeeSource(node);
                list.Add(ZigBeeSource);
            }
            return list;
        }        

        public async override Task Discover()
        {
            progressResponseProvider?.Init(0, DigiZigBeeUSBCoordinator.maxProgress, 0);
            var progress = 0;
            this.xBeeNetwork = this.zigBee.GetNetwork();
            var options = new HashSet<XBeeLibrary.Core.Models.DiscoveryOptions>();
            options.Add(XBeeLibrary.Core.Models.DiscoveryOptions.APPEND_DD);
            this.xBeeNetwork.SetDiscoveryOptions(options);
            this.xBeeNetwork.SetDiscoveryTimeout(25000L);
            this.xBeeNetwork.DiscoveryFinished += Network_DiscoveryFinished;
            this.xBeeNetwork.DeviceDiscovered += Network_DeviceDiscovered;
            this.xBeeNetwork.StartNodeDiscoveryProcess();
            var task = Task.Run(() =>
            {
                while (this.xBeeNetwork.IsDiscoveryRunning && this.discoveryFinished == false)
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
            this.Open();
            var r = this.zigBee.XBee64BitAddr.ToString();
            return r;
        }

        public override string GetPanID()
        {
            this.Open();
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
                    this.Open();
                    this.zigBee.SendData(remote.First(), bytes);
                }
            }
        }

        public override bool Open()
        {
            try
            {
                if (!this.zigBee.IsOpen)
                    this.zigBee.Open();
            }
            catch (Exception ex)
            {
                return false;
            }
            if (this.zigBee.IsOpen)
                return true;
            return false;   
        }

        public override void Close()
        {
            if(this.zigBee.IsOpen)
                this.zigBee.Close();
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
