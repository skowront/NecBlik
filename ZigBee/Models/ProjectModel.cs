using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ZigBee.Core.Models;

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
        [JsonProperty]
        public IEnumerable<DiagramZigBee> DiagramZigBees { get; set; } = new List<DiagramZigBee>();
        [JsonProperty]
        public string MapFile { get; set; }
        [JsonProperty]
        public DiagramItemMetadata DiagramMapMetadata { get; set; } = new DiagramItemMetadata();
    }
}
