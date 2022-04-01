using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecBlik.Core.GUI.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ProjectGuiModel
    {
        [JsonProperty]
        public DiagramItemMetadata mapDiagramMetadata { get; set; } = new DiagramItemMetadata();

        [JsonProperty]
        public Collection<DiagramZigBee> mapItemsMetadata { get; set; } = new Collection<DiagramZigBee>();

        public void Save(string projectFolder)
        {

        }

        public void Load(string projectFolder)
        {
            
        }
    }
}
