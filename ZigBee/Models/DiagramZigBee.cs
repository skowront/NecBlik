using System;
using System.Windows;
using Newtonsoft.Json;

namespace ZigBee.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DiagramZigBee
    {
        [JsonProperty]
        public Size Size { get; set; } = new Size();
        [JsonProperty]
        public Point<double> Point { get; set; } = new Point<double>(0, 0);
        [JsonProperty]
        public Guid ZigBeeGuid { get; set; } = new Guid();
    }
}
