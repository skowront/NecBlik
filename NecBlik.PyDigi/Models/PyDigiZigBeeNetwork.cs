using Newtonsoft.Json;
using NecBlik.Common.WpfExtensions.Interfaces;
using NecBlik.Core.Interfaces;
using NecBlik.Core.Models;
using NecBlik.PyDigi.Factories;
using NecBlik.Virtual.Models;

namespace NecBlik.PyDigi.Models
{
    public class PyDigiZigBeeNetwork: VirtualNetwork
    {
        public PyDigiZigBeeNetwork() : base()
        {
            this.DeviceFactory = new PyDigiZigBeeFactory();
            this.internalNetworkType = this.DeviceFactory.GetVendorID();
        }

        public PyDigiZigBeeNetwork(Coordinator coordinator, IUpdatableResponseProvider<int, bool, string> updatableResponseProvider = null) : this()
        {
            this.ProgressResponseProvider = updatableResponseProvider;
        }

        public async Task Initialize(Coordinator coordinator)
        {
            await this.SetCoordinator(coordinator);
        }

        public override void AddSource(IDeviceSource source)
        {
            return;
        }

        public override void Save(string folderName)
        {
            var dir = folderName + "\\" + this.Guid + "." + this.internalNetworkType;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            if (Directory.Exists(dir))
            {
                this.Coordinator.Save(dir);
                File.WriteAllText(dir + "\\" + Resources.Resources.NetworkFile, JsonConvert.SerializeObject(this, Formatting.Indented));
            }
        }
    }
}
