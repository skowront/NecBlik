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
    public class ZigBeeNetwork
    {
        [JsonProperty]
        public string Name { get; set; } = "Unnamed network";
        [JsonProperty]
        public Guid Guid { get; set; } = Guid.NewGuid();

        public ZigBeeCoordinator ZigBeeCoordinator { get; set; }

        public Collection<IZigBeeSource> ZigBeeSources { get; set; }

        public Collection<Tuple<string, string>> ZigBeeConnections { get; set; }

        public IZigBeeFactory ZigBeeFactory { get; set; } = new ZigBeeFactory();

        protected string internalNetworkType { get; set; } = string.Empty;

        public ZigBeeNetwork()
        {
            this.internalNetworkType = ZigBeeFactory.GetInternalFactoryType();
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
    }
}
