using NecBlik.Core.GUI.ViewModels;
using NecBlik.Core.Models;
using NecBlik.Digi.GUI.Examples.ViewModels.Sources;
using NecBlik.Digi.GUI.ViewModels;
using NecBlik.Virtual.GUI.ViewModels;
using System.Linq;

namespace NecBlik.Digi.GUI.Examples.ViewModels.Coordinators
{
    public class TemperatureCoordinatorViewModel: DigiZigBeeCoordinatorViewModel
    {
        public TemperatureCoordinatorViewModel(DeviceModel model, NetworkViewModel networkModel) : base(model, networkModel)
        {

        }

        public override void NotifySubscriber(RecievedData updateInformation)
        {
            base.NotifySubscriber(updateInformation);

            var sourceVms = this.Network.GetDeviceViewModels().Where((d) => { return d.Address == updateInformation.SourceAddress; });
            if(sourceVms!=null)
            {
                if(sourceVms.Any())
                {
                    var sourceVm = sourceVms.First();
                    sourceVm?.OnRecievedDataSentFromSourceDevice(updateInformation.Data,updateInformation.SourceAddress);
                }
            }
        }
    }
}
