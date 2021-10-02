using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Core.Interfaces;

namespace ZigBee.Core.Models
{
    public abstract class ZigBeeCoordinator: IZigBeeCoordinator
    {

        public ZigBeeCoordinator()
        {

        }

        public virtual string GetAddress()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<Tuple<string, string>> GetConnections()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IZigBeeSource> GetDevices()
        {
            throw new NotImplementedException();
        }

        public virtual string GetPanID()
        {
            throw new NotImplementedException();
        }
    }
}
