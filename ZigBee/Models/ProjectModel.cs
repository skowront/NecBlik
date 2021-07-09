using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ZigBee.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ProjectModel
    {
        [JsonProperty]
        public string ProjectName { get; set; } = Resources.Resources.ProjectModelPojectNameDefault;
        [JsonProperty]
        public List<ZigBeeModel> AvailableZigBees { get; set; } = new List<ZigBeeModel>();
        [JsonProperty]
        public ZigBeeModel ZigBeeTemplate { get; set; } = new ZigBeeModel();
    }
}
