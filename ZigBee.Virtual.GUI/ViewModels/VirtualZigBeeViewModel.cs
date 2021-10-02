using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Core.GUI;
using ZigBee.Core.Interfaces;
using ZigBee.Core.Models;

namespace ZigBee.Virtual.GUI.ViewModels
{
    public class VirtualZigBeeViewModel : ZigBeeViewModel
    {
        public VirtualZigBeeViewModel(ZigBeeModel model = null) : base(model)
        {
            this.Model = model;
        }
    }
}
