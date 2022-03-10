using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Core.Interfaces;
using ZigBee.Core.Models;
using ZigBee.Virtual.Factories;
using ZigBee.Digi.Models;
using Newtonsoft.Json;
using ZigBee.Common.WpfElements;
using ZigBee.Common.WpfElements.ResponseProviders;

namespace ZigBee.Digi.Factories
{
    public class DigiZigBeeFactory:VirtualZigBeeFactory
    {
        public DigiZigBeeFactory()
        {
            this.internalFactoryType = "Digi";
        }

        public override IZigBeeSource BuildNewSource()
        {
            return base.BuildNewSource();
        }

        public override ZigBeeCoordinator BuildCoordinator()
        {
            return new DigiZigBeeUSBCoordinator(this);
        }

        public override ZigBeeNetwork BuildNetworkFromDirectory(string pathToDirectory)
        {
            if (pathToDirectory.Split('.').LastOrDefault() != this.GetVendorID())
            {
                return null;
            }

            var path = Path.GetDirectoryName(pathToDirectory);
            var fileName = Path.GetFileName(pathToDirectory);
            var connectionData = JsonConvert.DeserializeObject<DigiZigBeeUSBCoordinator.DigiUSBConnectionData>(File.ReadAllText(pathToDirectory + "\\Coordinator.json"));
            var network = JsonConvert.DeserializeObject<DigiZigBeeNetwork>(File.ReadAllText(pathToDirectory + "\\Network.json"));
            var popup = new SimpleYesNoProgressBarPopup("Please wait...", "", Popups.ZigBeeIcons.InfoIcon, null, null, 0, 0, 0, false, false);
            if (network == null)
            {
                return null;
            }
            network.ProgressResponseProvider = new YesNoProgressBarPopupResponseProvider(popup);
            network.SetCoordinator(new DigiZigBeeUSBCoordinator(this, connectionData));
            return network;
        }

        public override IZigBeeSource BuildSourceFromJsonFile(string pathToFile)
        {
            return null;
        }
    }
}
