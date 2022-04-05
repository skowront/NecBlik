using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Python.Runtime;
using NecBlik.Common.WpfExtensions.Interfaces;
using NecBlik.Core.Interfaces;
using NecBlik.PyDigi.Factories;
using NecBlik.Virtual.Models;

namespace NecBlik.PyDigi.Models
{
    public class PyDigiZigBeeUSBCoordinator : VirtualCoordinator
    {
        private PyModule scope;

        private dynamic pyCoordinator;

        [JsonProperty]
        public PyDigiUSBConnectionData connectionData { get; set; }

        public PyDigiZigBeeUSBCoordinator(IDeviceFactory zigBeeFactory, PyDigiUSBConnectionData connectionData = null) : base(zigBeeFactory)
        {
            this.connectionData = connectionData ?? new PyDigiUSBConnectionData() { port = "COM4", baud = 9600 };
            this.deviceFactory = new PyDigiZigBeeFactory();
            this.internalType = this.deviceFactory.GetVendorID();
            this.connectionData = connectionData ?? new() { port = string.Empty, baud = 9600 };
            this.Name = Resources.Resources.PyDefaultDigiCoordinatorName;
            try
            {
                using (Py.GIL())
                {
                    this.scope = ZigBeePyEnv.NewInitializedScope();
                    this.scope.Exec(File.ReadAllText(Resources.Resources.PyDigiScriptsLocation + "/" + Resources.Resources.ScriptZigBeeCoordinator_py));
                    this.scope.Exec($"coordinator = Coordinator(\"{this.connectionData.port}\",\"{this.connectionData.baud}\");");
                    this.pyCoordinator = this.scope.Get<dynamic>("coordinator");
                    this.pyCoordinator.Open();
                    this.pyCoordinator.add_expl_data_received_callback(new Action<object,object>((self,args)=>
                    {
                        this.ZigBeeDataRecieved(self, args);
                    }));
                }
            }
            catch (Exception ex)
            {

            }
        }

        ~PyDigiZigBeeUSBCoordinator()
        {
            this.pyCoordinator.Close();
        }

        private void ZigBeeDataRecieved(object? sender, object arg)
        {
            using (Py.GIL())
            {
                var scope = ZigBeePyEnv.NewInitializedScope();
                scope.Set("data", (arg as dynamic).data);
                scope.Set("data_str", string.Empty);
                scope.Exec("data_str = bytes(data)");
                var dataString = scope.Get<string>("data_str");
                string senderAddress = (arg as dynamic).remote_node.get_64bit_addr().ToString();
                this.OnDataRecieved(dataString, senderAddress);
                scope.Dispose();
            }
        }

        public override async Task<IEnumerable<IDeviceSource>> GetDevices(IUpdatableResponseProvider<int, bool, string> progressResponseProvider = null)
        {
            using (Py.GIL())
            {
                pyCoordinator.Open();
                await this.Discover();
                dynamic devices = this.pyCoordinator.devices;
                List<IDeviceSource> sources = new();
                foreach(var item in devices)
                {
                    PyDigiZigBeeSource source = new PyDigiZigBeeSource(item);
                    sources.Add(source);
                }
                return sources;
            }
        }

        public override async Task Discover()
        {
            using (Py.GIL())
            {
                var t = Task.Run(() =>
                {
                    this.scope.Exec("coordinator.DiscoverDevices();");
                });
                await t;
            }
            return;
        }
        public override void Save(string folderPath)
        {
            File.WriteAllText(folderPath + "\\" + Resources.Resources.CoordinatorFile, JsonConvert.SerializeObject(this.connectionData, Formatting.Indented));
        }

        public override string GetVersion()
        {
            using (Py.GIL())
            {
                pyCoordinator.Open();
                dynamic version = this.pyCoordinator.xbee.get_firmware_version();
                return version.ToString();
            }
        }

        public override string GetAddress()
        {
            using (Py.GIL())
            {
                pyCoordinator.Open();
                dynamic version = this.pyCoordinator.xbee.get_firmware_version();
                return version.ToString();
            }
        }
        public override string GetCacheId()
        {
            return this.GetAddress();
        }

        public override string GetPanID()
        {
            using (Py.GIL())
            {
                pyCoordinator.Open();
                dynamic version = this.pyCoordinator.xbee.get_pan_id().hex();
                return version.ToString();
            }
        }

        public override void Close()
        {
            this.pyCoordinator.Close();
        }

        public override bool Open()
        {
            this.pyCoordinator.Open();
            return true;
        }

        public override void Dispose()
        {
            base.Dispose();
            this.Close();
        }

        [JsonObject(MemberSerialization.OptIn)]
        public class PyDigiUSBConnectionData
        {
            [JsonProperty]
            public int baud;

            [JsonProperty]
            public string port = string.Empty;
        }
    }
}
