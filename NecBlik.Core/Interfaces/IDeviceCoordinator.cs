using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NecBlik.Common.WpfExtensions.Interfaces;
using NecBlik.Core.Enums;

namespace NecBlik.Core.Interfaces
{
    public interface IDeviceCoordinator: IDeviceSource
    {
        public Task<IEnumerable<IDeviceSource>> GetDevices(IUpdatableResponseProvider<int, bool, string> progressResponseProvider = null);

        public IEnumerable<Tuple<string, string>> GetConnections();

        public Task Discover();

        public Task<PingModel> Ping(long timeout = 0, string payload = "", string remoteAddress = "");

        public Task<PingModel> PingPacket(long timeout = 0, string payload = "", string remoteAddress = "");

        public Task<string> GetStatusOf(string remoteAddress);
    }
}
