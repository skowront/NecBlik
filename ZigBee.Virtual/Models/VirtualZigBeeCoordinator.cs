using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Common.WpfExtensions.Interfaces;
using ZigBee.Core.Interfaces;
using ZigBee.Core.Models;

namespace ZigBee.Virtual.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class VirtualZigBeeCoordinator:ZigBeeCoordinator
    {        
        private List<IZigBeeSource> zigBeeSources = new List<IZigBeeSource>();

        private List<Tuple<string, string>> connections = new List<Tuple<string, string>>();

        protected IZigBeeFactory zigBeeFactory;

        public VirtualZigBeeCoordinator(IZigBeeFactory zigBeeFactory)
        {
            this.zigBeeFactory = zigBeeFactory;
            this.Name = Resources.Resources.DefaultVirtualZigBeeCoordinatorName;
            this.internalType = zigBeeFactory?.GetVendorID() ?? Resources.Resources.VirtualFactoryId;
        }

        public VirtualZigBeeCoordinator(IZigBeeFactory zigBeeFactory, bool setupExampleZigBees): this(zigBeeFactory)
        {
            if (setupExampleZigBees)
                this.setupExampleZigBees();
        }

        private void setupExampleZigBees()
        {
            this.zigBeeSources = new List<IZigBeeSource>();
            this.zigBeeSources.Add(this.zigBeeFactory.BuildNewSource());
            this.zigBeeSources.Add(this.zigBeeFactory.BuildNewSource());
            this.zigBeeSources.Add(this.zigBeeFactory.BuildNewSource());
            this.zigBeeSources.Add(this.zigBeeFactory.BuildNewSource());
            this.zigBeeSources.Add(this.zigBeeFactory.BuildNewSource());
            this.zigBeeSources.Add(this.zigBeeFactory.BuildNewSource());

            this.connections.Add(new Tuple<string, string>(this.zigBeeSources[0].GetAddress(),this.zigBeeSources[1].GetAddress()));
            this.connections.Add(new Tuple<string, string>(this.zigBeeSources[1].GetAddress(),this.zigBeeSources[2].GetAddress()));
            this.connections.Add(new Tuple<string, string>(this.zigBeeSources[1].GetAddress(),this.zigBeeSources[3].GetAddress()));
            this.connections.Add(new Tuple<string, string>(this.zigBeeSources[4].GetAddress(),this.zigBeeSources[5].GetAddress()));
        }

        public override async Task<IEnumerable<IZigBeeSource>> GetDevices(IUpdatableResponseProvider<int, bool, string> progressResponseProvider = null)
        {
            return this.zigBeeSources;
        }

        public override void SetDevices(IEnumerable<IZigBeeSource> sources)
        {
            this.zigBeeSources = new List<IZigBeeSource>(sources);
        }

        public override IEnumerable<Tuple<string, string>> GetConnections()
        {
            return this.connections;
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
    }
}
