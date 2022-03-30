using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Core.GUI.ViewModels;
using ZigBee.Core.Models;
using ZigBee.Virtual.GUI.ViewModels;

namespace ZigBee.Digi.GUI.ViewModels
{
    public class DigiZigBeeViewModel:VirtualZigBeeViewModel
    {
        public DigiZigBeeViewModel(ZigBeeModel model = null, ZigBeeNetworkViewModel networkModel =null) : base(model,networkModel)
        {
        }

        public override string GetCacheId()
        {
            return this.Model.CacheId;
        }
    }
}
