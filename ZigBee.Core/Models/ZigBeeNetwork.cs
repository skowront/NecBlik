using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Core.Factories;
using ZigBee.Core.Interfaces;

namespace ZigBee.Core.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ZigBeeNetwork:IVendable
    {
        [JsonProperty]
        public string Name { get; set; } = "Unnamed network";
        [JsonProperty]
        public Guid Guid { get; set; } = Guid.NewGuid();

        public IZigBeeCoordinator ZigBeeCoordinator { get; set; }

        public Collection<IZigBeeSource> ZigBeeSources { get; set; } = new Collection<IZigBeeSource>();

        public Collection<Tuple<string, string>> ZigBeeConnections { get; set; }

        public IZigBeeFactory ZigBeeFactory { get; set; } = new ZigBeeDefaultFactory();

        protected string internalNetworkType { get; set; } = string.Empty;

        public ZigBeeNetwork()
        {
            this.internalNetworkType = ZigBeeFactory.GetVendorID();
        }

        public void SetCoordinator(IZigBeeCoordinator zigBeeCoordinator)
        {
            this.ZigBeeCoordinator = zigBeeCoordinator;
        }

        public void AddSource(IZigBeeSource source)
        {
            this.ZigBeeSources.Add(source);
        }

        public void Save(string folderName)
        {
            var dir = folderName + "\\" + this.Guid + "." + this.internalNetworkType;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            if (Directory.Exists(dir))
            {
                this.ZigBeeCoordinator.Save(dir);
                File.WriteAllText(dir+"\\"+"Network.json",JsonConvert.SerializeObject(this,Formatting.Indented));
                var sourcesSubDir = dir + Resources.Resources.ZigBeeNetworkSourcesSubdirectory;
                if (!Directory.Exists(sourcesSubDir))
                {
                    Directory.CreateDirectory(sourcesSubDir);
                }
                if (Directory.Exists(sourcesSubDir))
                {
                    
                    foreach (var item in this.ZigBeeSources)
                    {
                        item.Save(sourcesSubDir);
                    }
                }
            }
        }

        public string GetVendorID()
        {
            return this.internalNetworkType;
        }
    }
}
