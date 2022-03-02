﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ZigBee.Core.GUI.Models
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