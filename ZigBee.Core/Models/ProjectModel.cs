using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ZigBee.Core.Models;

namespace ZigBee.Core.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ProjectModel
    {
        [JsonProperty]
        public string ProjectName { get; set; } = Resources.Resources.ProjectModelPojectNameDefault;
        [JsonProperty]
        public ZigBeeModel ZigBeeTemplate { get; set; } = new ZigBeeModel();
        [JsonProperty]
        public string MapFile { get; set; }

        public List<ZigBeeNetwork> ZigBeeNetworks { get; set; } = new List<ZigBeeNetwork>();

        public void Save(string projectFolder)
        {
            var dir = projectFolder + Resources.Resources.ZigBeeNetworksDirectory;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            if (Directory.Exists(dir))
            {
                foreach (var ZigBeeNetwork in this.ZigBeeNetworks)
                {
                    ZigBeeNetwork.Save(dir);
                }
            }
        }
    }
}
