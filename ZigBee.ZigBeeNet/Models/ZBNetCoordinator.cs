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
using ZigBee.ZigBeeNet.USB;

namespace ZigBee.ZigBeeNet.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ZBNetCoordinator: VirtualZigBeeCoordinator
    {
        private ZBNetConnectionData connectionData;

        private IZigBeePort zigbeePort;

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
                List<ZBNetSource> r = new List<ZBNetSource>();
                Thread thread = new Thread(async () => {
                    this.zigbeePort = new ZigBeeSerialPort(this.connectionData.port, this.connectionData.baud, FlowControl.FLOWCONTROL_OUT_NONE);
                    this.dongle = new ZigBeeDongleXBee(zigbeePort);
                    this.networkManager = new ZigBeeNetworkManager(dongle);
                    this.networkManager.Initialize();
                    ZigBeeStatus startupSucceded = networkManager.Startup(false);
                    if (startupSucceded == ZigBeeStatus.SUCCESS)
                    {
                        Log.Logger.Information("ZigBee console starting up ... [OK]");
                        
                        var task = Task.Run(() =>
                        {
                            foreach (var item in this.networkManager.Nodes)
                            {
                                ZBNetSource zb = new(item);
                                r.Add(zb);
                                foreach(var endpoint in item.GetEndpoints())
                                {
                                    ZBNetSource endpt = new(endpoint.Node) { Version = endpoint.DeviceVersion.ToString() };
                                    r.Add(endpt);
                                }
                            }
                        });
                        await task;
                        this.zigbeePort.Close();
                        return;
                    }
                    else
                    {
                        Log.Logger.Information("ZigBee console starting up ... [FAIL]");
                        this.zigbeePort.Close();
                        
                    }
                });
                thread.Start();
                thread.Join();

                return r;
            }
            catch (Exception ex)
            {
                var e = ex;
                this.zigbeePort.Close();
            }
            this.zigbeePort.Close();
            return null;
        }

        public override void Save(string folderPath)
        {
            File.WriteAllText(folderPath + "\\" + "Coordinator.json", JsonConvert.SerializeObject(this.connectionData, Formatting.Indented));
        }

        public override string GetAddress()
        {
            return this.dongle?.IeeeAddress.ToString();
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

            [JsonProperty]
            public int baud = 9600;
        }
    }
}
