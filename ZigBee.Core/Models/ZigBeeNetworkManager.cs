using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Core.Interfaces;

namespace ZigBee.Core.Models
{
    public class ZigBeeNetworkManager
    {
        public string PANID { get; set; }

        public IZigBeeCoordinator ZigBeeCoordinator { get; set; }

        public Collection<IZigBeeSource> ZigBeeSources { get; set; }

        public Collection<Tuple<string,string>> ZigBeeConnections { get; set; }

        public IZigBeeFactory ZigBeeFactory { get; set; }
    }
}
