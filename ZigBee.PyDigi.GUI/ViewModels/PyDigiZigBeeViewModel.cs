using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Core.Models;
using ZigBee.Virtual.GUI.ViewModels;

namespace ZigBee.PyDigi.GUI.ViewModels
{
    public class PyDigiZigBeeViewModel: VirtualZigBeeViewModel
    {
        public PyDigiZigBeeViewModel(ZigBeeModel model = null) : base(model)
        {
            this.Model = model;
        }

        public override string GetCacheId()
        {
            return this.Model.CacheId;
        }
    }
}
