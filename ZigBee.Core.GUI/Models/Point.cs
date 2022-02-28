using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZigBee.Core.GUI.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Point<T>
    {
        [JsonProperty]
        public T X { get; set; }
        [JsonProperty]
        public T Y { get; set; }

        public Point(T x, T y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
