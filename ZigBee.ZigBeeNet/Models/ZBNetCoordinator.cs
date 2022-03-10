using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Core.Interfaces;
using ZigBee.Virtual.Models;
using Newtonsoft.Json;
using ZigBeeNet;
using ZigBee.Common.WpfExtensions.Interfaces;
using ZigBeeNet.Tranport.SerialPort;
using ZigBeeNet.Transport;
using Serilog;
using ZigBeeNet.Hardware.Digi.XBee;

namespace ZigBee.ZigBeeNet.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ZBNetCoordinator: VirtualZigBeeCoordinator
    {
        private ZigBeeNode zigBeeNode;

        private ZBNetConnectionData connectionData;

        private ZigBeeSerialPort zigbeePort;

        private IZigBeeTransportTransmit dongle;

        ZigBeeNetworkManager networkManager;

        public ZBNetCoordinator(IZigBeeFactory zigBeeFactory, ZBNetConnectionData connectionData) : base(zigBeeFactory)
        {
            this.connectionData = connectionData;
            this.zigBeeFactory = zigBeeFactory;
            this.internalType = this.zigBeeFactory.GetVendorID();
            this.Name = "ZigBeeNet Coordinator";
        }

        public override async Task<IEnumerable<IZigBeeSource>> GetDevices(IUpdatableResponseProvider<int, bool, string> progressResponseProvider = null)
        {
            try
            {
                this.zigbeePort = new ZigBeeSerialPort("COM7");
                this.dongle = new ZigBeeDongleXBee(zigbeePort);
                this.networkManager = new ZigBeeNetworkManager(dongle);
                this.networkManager.Initialize();
                ZigBeeStatus startupSucceded = networkManager.Startup(false);
                if (startupSucceded == ZigBeeStatus.SUCCESS)
                {
                    Log.Logger.Information("ZigBee console starting up ... [OK]");
                    List<ZBNetSource> r = new List<ZBNetSource>();
                    foreach(var item in this.networkManager.Nodes)
                    {
                        ZBNetSource zb = new(item);
                        r.Add(zb);
                    }
                    return r;
                }
                else
                {
                    Log.Logger.Information("ZigBee console starting up ... [FAIL]");
                    return null;
                }

            }
            catch (Exception ex)
            {
                var e = ex;
            }

            return null;
        }

        public override void Save(string folderPath)
        {
            File.WriteAllText(folderPath + "\\" + "Coordinator.json", JsonConvert.SerializeObject(this.connectionData, Formatting.Indented));
        }

        public override string GetAddress()
        {
            return this.zigBeeNode?.IeeeAddress.ToString();
        }

        public override string GetVersion()
        {
            return "-??-";
        }

        public override string GetCacheId()
        {
            return "Coordinator"+this.GetAddress();
        }

        [JsonObject(MemberSerialization.OptIn)]
        public class ZBNetConnectionData
        {
            [JsonProperty]
            public string port = string.Empty;
        }
    }
}
