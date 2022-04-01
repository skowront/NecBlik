using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NecBlik.Core.GUI.ViewModels;
using NecBlik.Core.Models;
using NecBlik.Virtual.GUI.ViewModels;

namespace NecBlik.PyDigi.GUI.ViewModels
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
