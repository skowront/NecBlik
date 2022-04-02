using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NecBlik.Core.GUI.ViewModels;
using NecBlik.Core.Models;
using NecBlik.Virtual.GUI.ViewModels;

namespace NecBlik.ZigBeeNet.GUI.ViewModels
{
    public class ZBNetSourceViewModel: VirtualDeviceViewModel
    {
        public ZBNetSourceViewModel(DeviceModel model = null,NetworkViewModel networkModel = null) : base(model,networkModel)
        {
        }

        public override string GetCacheId()
        {
            return this.Model.CacheId;
        }
    }
}
