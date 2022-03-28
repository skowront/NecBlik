﻿using Newtonsoft.Json;
using ZigBee.Common.WpfExtensions.Interfaces;
using ZigBee.Core.Interfaces;
using ZigBee.Core.Models;
using ZigBee.Digi.Factories;
using ZigBee.Virtual.Models;

namespace ZigBee.Digi.Models
{
    public class DigiZigBeeNetwork: VirtualZigBeeNetwork
    {
        public DigiZigBeeNetwork():base()
        {
            this.ZigBeeFactory = new DigiZigBeeFactory();
            this.internalNetworkType = this.ZigBeeFactory.GetVendorID();
        }

        public DigiZigBeeNetwork(ZigBeeCoordinator coordinator, IUpdatableResponseProvider<int, bool, string> updatableResponseProvider = null) : this()
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
