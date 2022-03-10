using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Common.WpfExtensions.Interfaces;

namespace ZigBee.Core.Interfaces
{
    public interface IZigBeeCoordinator: IZigBeeSource
    {
        public Task<IEnumerable<IZigBeeSource>> GetDevices(IUpdatableResponseProvider<int, bool, string> progressResponseProvider = null);

        public IEnumerable<Tuple<string, string>> GetConnections();
    }
}
