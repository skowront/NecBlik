using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZigBee.Core.GUI.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ProjectGuiModel
    {
        [JsonProperty]
        public DiagramItemMetadata mapDiagramMetadata { get; set; } = new DiagramItemMetadata();

        public void Save(string projectFolder)
        {

        }

        public void Load(string projectFolder)
        {
            
        }
    }
}
