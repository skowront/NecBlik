using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NecBlik.Common.WpfExtensions.Interfaces;
using NecBlik.Core.Interfaces;
using NecBlik.Core.Models;
using NecBlik.Core.Enums;

namespace NecBlik.Virtual.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class VirtualCoordinator : Coordinator
    {
        [JsonProperty]
        public string Address
        {
            get { return this.GetAddress(); }
            set { this.cachedAddress = value; }
        }

        private string cachedAddress = string.Empty;

        public override string GetAddress()
        {
            if (cachedAddress == string.Empty)
            {
                this.cachedAddress = VirtualNetwork.generateAddress64bit();
            }
            return this.cachedAddress;
        }

        private List<IDeviceSource> Sources = new List<IDeviceSource>();

        private List<Tuple<string, string>> connections = new List<Tuple<string, string>>();

        protected IDeviceFactory deviceFactory;

        public VirtualCoordinator(IDeviceFactory deviceFactory)
        {
            this.deviceFactory = deviceFactory;
            this.Name = Resources.Resources.DefaultVirtualDeviceCoordinatorName;
            this.PanId = Guid.NewGuid().ToString();
            this.internalType = deviceFactory?.GetVendorID() ?? Resources.Resources.VirtualFactoryId;
        }

        public VirtualCoordinator(IDeviceFactory deviceFactory, bool setupExampleDevices): this(deviceFactory)
        {
            if (setupExampleDevices)
                this.setupExampleDevices();
        }

        private void setupExampleDevices()
        {
            this.Sources = new List<IDeviceSource>();
            this.Sources.Add(this.deviceFactory.BuildNewSource());
            this.Sources.Add(this.deviceFactory.BuildNewSource());
            this.Sources.Add(this.deviceFactory.BuildNewSource());
            this.Sources.Add(this.deviceFactory.BuildNewSource());
            this.Sources.Add(this.deviceFactory.BuildNewSource());
            this.Sources.Add(this.deviceFactory.BuildNewSource());

            this.connections.Add(new Tuple<string, string>(this.Sources[0].GetAddress(),this.Sources[1].GetAddress()));
            this.connections.Add(new Tuple<string, string>(this.Sources[1].GetAddress(),this.Sources[2].GetAddress()));
            this.connections.Add(new Tuple<string, string>(this.Sources[1].GetAddress(),this.Sources[3].GetAddress()));
            this.connections.Add(new Tuple<string, string>(this.Sources[4].GetAddress(),this.Sources[5].GetAddress()));
        }

        public override async Task<IEnumerable<IDeviceSource>> GetDevices(IUpdatableResponseProvider<int, bool, string> progressResponseProvider = null)
        {
            return this.Sources;
        }

        public override void SetDevices(IEnumerable<IDeviceSource> sources)
        {
            this.Sources = new List<IDeviceSource>(sources);
        }

        public override IEnumerable<Tuple<string, string>> GetConnections()
        {
            return this.connections;
        }

        public override string GetCacheId()
        {
            return this.Address;
        }


        public override void Save(string folderPath)
        {
            var json = JsonConvert.SerializeObject(this, Formatting.Indented);
            var file = this.Guid + "." + this.internalType + ".json";
            if (File.Exists(folderPath + "\\" + file))
            {
                File.WriteAllText(folderPath + "\\" + file, json);
            }
            else
            {
                File.AppendAllText(folderPath + "\\" + file, json);
            }
        }

        public override async Task<string> GetStatusOf(string remoteAddress)
        {
            return NecBlik.Core.Resources.Statuses.Connected;
        }
    }
}
