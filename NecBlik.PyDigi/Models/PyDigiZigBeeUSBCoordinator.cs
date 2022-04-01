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
    public class PyDigiZigBeeUSBCoordinator : VirtualZigBeeCoordinator
    {
        private PyModule scope;

        private dynamic pyCoordinator;

        [JsonProperty]
        public PyDigiUSBConnectionData connectionData { get; set; }

        public PyDigiZigBeeUSBCoordinator(IZigBeeFactory zigBeeFactory, PyDigiUSBConnectionData connectionData = null) : base(zigBeeFactory)
        {
            this.connectionData = connectionData ?? new PyDigiUSBConnectionData() { port = "COM4", baud = 9600 };
            this.zigBeeFactory = new PyDigiZigBeeFactory();
            this.internalType = this.zigBeeFactory.GetVendorID();
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
                }
            }
            catch (Exception ex)
            {

            }
        }

        public override async Task<IEnumerable<IZigBeeSource>> GetDevices(IUpdatableResponseProvider<int, bool, string> progressResponseProvider = null)
        {
            using (Py.GIL())
            {
                this.scope.Exec("coordinator.DiscoverDevices();");
                pyCoordinator.Open();
                dynamic devices = this.pyCoordinator.devices;
                pyCoordinator.Close();
                List<IZigBeeSource> sources = new();
                foreach(var item in devices)
                {
                    PyDigiZigBeeSource source = new PyDigiZigBeeSource(item);
                    sources.Add(source);
                }
                return sources;
            }
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
                pyCoordinator.Close();
                return version.ToString();
            }
        }

        public override string GetAddress()
        {
            using (Py.GIL())
            {
                pyCoordinator.Open();
                dynamic version = this.pyCoordinator.xbee.get_firmware_version();
                pyCoordinator.Close();
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
                pyCoordinator.Close();
                return version.ToString();
            }
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
