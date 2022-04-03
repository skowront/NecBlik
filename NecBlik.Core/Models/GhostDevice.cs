using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecBlik.Core.Models
{
    public class GhostDevice:Device
    {
        public string GhostCache { private get; set; }

        public override string GetCacheId()
        {
            return this.GhostCache;
        }

        public GhostDevice()
        {

        }

        public GhostDevice(string cache)
        {
            this.GhostCache = cache;
        }

    }
}
