using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecBlik.Core.Models
{
    public class GhostNetwork:Network
    {
        public GhostNetwork(Guid guid)
        {
            this.Guid = guid;
        }
    }
}
