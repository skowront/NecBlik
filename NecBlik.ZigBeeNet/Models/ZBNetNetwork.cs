using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NecBlik.Virtual.Models;
using NecBlik.Core.Models;
using NecBlik.ZigBeeNet.Factories;
using NecBlik.Common.WpfExtensions.Interfaces;
using NecBlik.Core.Interfaces;
using Newtonsoft.Json;

namespace NecBlik.ZigBeeNet.Models
{
    public class ZBNetNetwork:VirtualZigBeeNetwork
    {
        public ZBNetNetwork():base()
        {
            this.ZigBeeFactory = new ZBNetFactory();
            this.internalNetworkType = this.ZigBeeFactory.GetVendorID();
        }

        public ZBNetNetwork(ZigBeeCoordinator coordinator, IUpdatableResponseProvider<int, bool, string> updatableResponseProvider = null):this()
        {
            this.ProgressResponseProvider = updatableResponseProvider;
            this.ZigBeeFactory = new ZBNetFactory();
            this.internalNetworkType = this.ZigBeeFactory.GetVendorID();
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
                File.WriteAllText(dir + "\\" + "Network.json", JsonConvert.SerializeObject(this, Formatting.Indented));
            }
        }
    }
}
