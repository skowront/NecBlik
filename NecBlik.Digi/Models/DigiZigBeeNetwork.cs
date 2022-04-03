using Newtonsoft.Json;
using NecBlik.Common.WpfExtensions.Interfaces;
using NecBlik.Core.Interfaces;
using NecBlik.Core.Models;
using NecBlik.Digi.Factories;
using NecBlik.Virtual.Models;

namespace NecBlik.Digi.Models
{
    public class DigiZigBeeNetwork: VirtualNetwork
    {
        public DigiZigBeeNetwork():base()
        {
            this.DeviceFactory = new DigiZigBeeFactory();
            this.internalNetworkType = this.DeviceFactory.GetVendorID();
        }

        public DigiZigBeeNetwork(Coordinator coordinator, IUpdatableResponseProvider<int, bool, string> updatableResponseProvider = null) : this()
        {
            this.ProgressResponseProvider = updatableResponseProvider;
        }

        public async Task Initialize(Coordinator coordinator)
        {
            await this.SetCoordinator(coordinator);
        }

        public override void AddSource(IDeviceSource source)
        {
            this.DeviceSources.Add(source);
            return;
        }

        public override void Save(string folderName)
        {
            base.Save(folderName);
            //var dir = folderName + "\\" + this.Guid + "." + this.internalNetworkType;
            //if (!Directory.Exists(dir))
            //{
            //    Directory.CreateDirectory(dir);
            //}
            //if (Directory.Exists(dir))
            //{
            //    base.Save(folderName);

            //    this.Coordinator.Save(dir);
            //    File.WriteAllText(dir + "\\" + Resources.Resources.NetworkFile, JsonConvert.SerializeObject(this, Formatting.Indented));
            //}
        }
    }
}
