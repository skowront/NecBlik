using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZigBee.Core.Interfaces
{
    public interface IZigBeeCoordinator: IZigBeeSource
    {
        public IEnumerable<IZigBeeSource> GetDevices();

        public IEnumerable<Tuple<string, string>> GetConnections();
    }
}
