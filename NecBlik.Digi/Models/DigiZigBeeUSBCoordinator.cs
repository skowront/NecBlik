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
using NecBlik.Core.Enums;
using XBeeLibrary.Core.Models;
using XBeeLibrary.Core.Packet;
using XBeeLibrary.Core.Packet.Common;
using NecBlik.Digi.Packets;

namespace NecBlik.Digi.Models
{

    [JsonObject(MemberSerialization.OptIn)]
    public class DigiZigBeeUSBCoordinator : VirtualCoordinator
    {
        private ZigBeeDevice zigBee;
        private XBeeNetwork xBeeNetwork;
        private bool disposed = false;  

        [JsonProperty]
        public DigiUSBConnectionData connectionData { get; set; }

        IUpdatableResponseProvider<int, bool, string> progressResponseProvider = null;

        public PacketLogger PacketLogger;

        private const int sleepTime = 500;

        private const long timeout = 25000L;

        private const int maxProgress = (int)timeout / sleepTime;

        private bool discoveryFinished = false;

        private WinSerialPort winSerialPort = null;

        protected byte frameId = 1;

        protected void IncrementFrameId()
        {
            this.frameId = this.frameId++;
            if(this.frameId == 0)
            {
                this.frameId = 1;
            }
        }

        public DigiZigBeeUSBCoordinator(IDeviceFactory zigBeeFactory, DigiUSBConnectionData connectionData = null) : base(zigBeeFactory)
        {
            this.deviceFactory = new DigiZigBeeFactory();
            this.internalType = this.deviceFactory.GetVendorID();
            this.connectionData = connectionData ?? new() { port = string.Empty, baud = 9600 };
            this.Name = Resources.Resources.DefaultDigiCoordinatorName;
            this.winSerialPort = new WinSerialPort(connectionData.port, connectionData.baud);
            this.zigBee = new ZigBeeDevice(winSerialPort);
            this.Open();
            this.zigBee.DataReceived += ZigBeeDataReceived;
            this.zigBee.PacketReceived += ZigBee_PacketReceived;
        }

        public void SetNewConnectionData(DigiUSBConnectionData connectionData)
        {
            this.connectionData = connectionData ?? new() { port = string.Empty, baud = 9600 };
            this.zigBee.Close();
            this.Close();
            this.winSerialPort = new WinSerialPort(connectionData.port, connectionData.baud);
            this.zigBee = new ZigBeeDevice(this.winSerialPort);
            this.Open();
            this.zigBee.DataReceived += ZigBeeDataReceived;
            this.zigBee.PacketReceived += ZigBee_PacketReceived;
        }

        public void ResetCoordinatorSoftware()
        {
            this.Close();
            this.Close();
            this.winSerialPort = new WinSerialPort(this.connectionData.port, this.connectionData.baud);
            this.zigBee = new ZigBeeDevice(this.winSerialPort);
            this.Open();
            this.zigBee.DataReceived += ZigBeeDataReceived;
            this.zigBee.PacketReceived += ZigBee_PacketReceived;
        }

        private void ZigBee_PacketReceived(object? sender, XBeeLibrary.Core.Events.PacketReceivedEventArgs e)
        {
            
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
            if (!this.Open())
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
            try
            {
                if (!this.Open())
                    return;
                progressResponseProvider?.Init(0, DigiZigBeeUSBCoordinator.maxProgress, 0);
                var progress = 0;
                this.xBeeNetwork = this.zigBee.GetNetwork();
                var options = new HashSet<XBeeLibrary.Core.Models.DiscoveryOptions>();
                options.Add(XBeeLibrary.Core.Models.DiscoveryOptions.APPEND_DD);
                this.xBeeNetwork.SetDiscoveryOptions(options);
                //this.xBeeNetwork.SetDiscoveryTimeout(25000L);
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
            catch (Exception ex)
            {
                try
                {
                    this.Close();
                    this.Open();
                    
                }
                catch(Exception ex2)
                {

                }
            }
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
            return this.zigBee?.GetAddressString();
        }

        public override void Send(string data, string address)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(data + "\0");
            if (address == string.Empty)
            {
                this.zigBee.SendBroadcastData(bytes);
            }
            else
            {
                try
                {
                    var remote = this.zigBee.GetNetwork().GetDevices().Where((dev) => { return dev.GetAddressString() == address; });
                    if (remote.Count() >= 1)
                    {
                        this.Open();
                        this.zigBee.SendData(remote.First(), bytes);
                        this.PacketLogger?.AddEntry(DateTime.Now, data, "SEND_DATA", nameof(this.zigBee.SendData));
                    }
                }
                catch(Exception ex)
                {

                }
            }
        }

        public XBeePacket SendATCommandPacket(string address, string command, string parameter)
        {
            try
            {
                
                XBeePacket resp;
                if (address == this.Address)
                {
                    ATCommandPacket aTCommandPacket = new(this.frameId, command, parameter);
                    this.IncrementFrameId();
                    resp = this.zigBee.SendPacket(aTCommandPacket);
                }
                else
                {
                    var device = this.zigBee.GetNetwork().GetDevices().Where((x) => { return x.GetAddressString() == address; }).FirstOrDefault();
                    if (device == null)
                        return null;
                    RemoteATCommandPacket aTCommandPacket = new(this.frameId, device.XBee64BitAddr, device.XBee16BitAddr, 2, command, parameter);
                    this.IncrementFrameId();
                    resp = this.zigBee.SendPacket(aTCommandPacket);
                }
                return resp;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public override bool Open()
        {
            if (this.disposed == true)
                return false;
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
            if (this.zigBee.IsOpen)
                this.zigBee.Close();
            this.zigBee.Close();
        }

        public override void Dispose()
        {
            this.disposed = true;
            this.Close();
            this.winSerialPort.Dispose();
        }

        public override void OnDataSent(string data, string sourceAddress)
        {
            if (sourceAddress == this.Address)
            {
                this.OnDataRecieved(data, sourceAddress);
            }
        }

        public override async Task<PingModel> Ping(long timeout = 0, string payload = "", string remoteAddress = "")
        {
            var result = new PingModel();
            try
            {
                if (remoteAddress == string.Empty)
                {
                    return new PingModel(0, PingModel.PingResult.Ok, payload);
                }
                XBeeMessage msg = null;
                var sendingTime = DateTime.Now;
                this.Send("Echo" + payload, remoteAddress);
                msg = this.zigBee.ReadData((int)timeout);
                if (msg == null)
                {
                    result.Result = PingModel.PingResult.NotOk;
                    result.ResponseTime = (DateTime.Now - sendingTime).TotalMilliseconds;
                    return result;
                }
                else
                {
                    result.Result = PingModel.PingResult.Ok;
                    result.ResponseTime = (DateTime.Now - sendingTime).TotalMilliseconds;
                    result.Payload = msg.DataString;
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Result = PingModel.PingResult.NotOk;
                result.ResponseTime = double.PositiveInfinity;
                result.Message = ex.Message;
                return result;
            }
            return result;
        }

        public override async Task<PingModel> PingPacket(long timeout = 0, string payload = "", string remoteAddress = "", bool awaitConfirmation = true)
        {
            //Considering andrew-rapp library and the fact that we expetct a \0 at the end of the message.
            const int maxPayload = 239;
            var result = new PingModel();
            try
            {
                if (remoteAddress == string.Empty)
                {
                    return new PingModel(0, PingModel.PingResult.Ok, payload);
                }
                //var packet = new TxRequest64Bit(remoteAddress,this.zigBee.OperatingMode);
                if(payload.Length<1 || payload == String.Empty)
                {
                    payload = "\0";
                }
                if(payload[payload.Length-1]!='\0')
                {
                    payload.Remove(payload.Length - 1);
                    payload += '\0';
                }
                var rawPayload = Encoding.ASCII.GetBytes(payload);
                TransmitPacket packet = awaitConfirmation ? new TransmitPacket(this.frameId, new XBee64BitAddress(remoteAddress), new XBee16BitAddress("FFFE"), 0, 0, rawPayload) :
                    new TransmitPacket(0, new XBee64BitAddress(remoteAddress), new XBee16BitAddress("FFFE"), 0, 01, rawPayload);
                var api2 = packet.GenerateByteArrayEscaped();
                var br = ByteArrayToString(api2);
                this.IncrementFrameId();
                var sendingTime = DateTime.Now;
                XBeePacket rpacket = null;
                if (!awaitConfirmation)
                {
                    this.PacketLogger?.AddEntry(DateTime.Now, payload, "NO_ACK", nameof(TransmitPacket));
                    this.zigBee.SendPacketAsync(packet);
                    return new PingModel() { ResponseTime = double.NaN };
                }
                this.PacketLogger?.AddEntry(DateTime.Now, payload, "ACK", nameof(TransmitPacket));
                rpacket = this.zigBee.SendPacket(packet);
                if (rpacket == null)
                {
                    result.Result = PingModel.PingResult.NotOk;
                    result.ResponseTime = (DateTime.Now - sendingTime).TotalMilliseconds;
                    return result;
                }
                else
                {
                    result.Result = PingModel.PingResult.Ok;
                    result.ResponseTime = (DateTime.Now - sendingTime).TotalMilliseconds;
                    //result.Message = rpacket.TransmitStatus.ToString();
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Result = PingModel.PingResult.Timeout;
                result.ResponseTime = double.PositiveInfinity;
                result.Message = ex.Message;
                return result;
            }
            return result;
        }

        public override async Task<string> GetStatusOf(string remoteAddress)
        {
            if (remoteAddress == this.Address)
            {
                return this.Open() ?  NecBlik.Core.Resources.Statuses.Connected:
                     NecBlik.Core.Resources.Statuses.Disconnected;
            }
            else
            {
                var res = await this.Ping(10000, string.Empty, remoteAddress);
                if(res.Result == PingModel.PingResult.Ok)
                {
                    return NecBlik.Core.Resources.Statuses.Connected;
                }
                else
                {
                    return NecBlik.Core.Resources.Statuses.Disconnected;
                }
            }
        }

        public override async Task<(double localStrength, double remoteStrength)> GetSignalStrength(string remoteAddress)
        {
            var r = this.SendATCommandPacket(remoteAddress, "DB", string.Empty);
            var l = this.SendATCommandPacket(this.Address, "DB", string.Empty);
            double local = 0.0f;
            double remote = 0.0f;
            if (r != null)
                if(r is RemoteATCommandResponsePacket)
                    remote = (r as RemoteATCommandResponsePacket).CommandValue[0]*(-1);
            if (l != null)
                if (l is ATCommandResponsePacket)
                    local = (l as ATCommandResponsePacket).CommandValue[0] * (-1);
            return (local,remote);
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
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
