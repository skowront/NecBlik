using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NecBlik.Core.GUI.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DiagramItemMetadata
    {
        [JsonProperty]
        public Size Size { get; set; } = new Size() { Height = double.NaN, Width = double.NaN };
        [JsonProperty]
        public Point<double> Point { get; set; } = new Point<double>(0, 0);
    }
}
