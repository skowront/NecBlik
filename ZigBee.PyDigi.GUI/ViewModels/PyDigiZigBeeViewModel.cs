using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Core.GUI.ViewModels;
using ZigBee.Core.Models;
using ZigBee.Virtual.GUI.ViewModels;

namespace ZigBee.PyDigi.GUI.ViewModels
{
    public class PyDigiZigBeeViewModel: VirtualZigBeeViewModel
    {
        public PyDigiZigBeeViewModel(ZigBeeModel model = null, ZigBeeNetworkViewModel networkModel = null) : base(model, networkModel)
        {
            this.Model = model;
        }

        public override string GetCacheId()
        {
            return this.Model.CacheId;
        }
    }
}
