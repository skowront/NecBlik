using Newtonsoft.Json;
using NecBlik.Common.WpfExtensions.Interfaces;
using NecBlik.Core.Interfaces;
using NecBlik.Core.Models;
using NecBlik.PyDigi.Factories;
using NecBlik.Virtual.Models;

namespace NecBlik.PyDigi.Models
{
    public class PyDigiZigBeeNetwork: VirtualZigBeeNetwork
    {
        public PyDigiZigBeeNetwork() : base()
        {
            this.ZigBeeFactory = new PyDigiZigBeeFactory();
            this.internalNetworkType = this.ZigBeeFactory.GetVendorID();
        }

        public PyDigiZigBeeNetwork(ZigBeeCoordinator coordinator, IUpdatableResponseProvider<int, bool, string> updatableResponseProvider = null) : this()
        {
            this.ProgressResponseProvider = updatableResponseProvider;
        }

        public async Task Initialize(ZigBeeCoordinator coordinator)
        {
            await this.SetCoordinator(coordinator);
        }

        public override void AddSource(IZigBeeSource source)
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
                this.ZigBeeCoordinator.Save(dir);
                File.WriteAllText(dir + "\\" + Resources.Resources.NetworkFile, JsonConvert.SerializeObject(this, Formatting.Indented));
            }
        }
    }
}
