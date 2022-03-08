using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Core.GUI;

namespace ZigBee.Core.GUI.Interfaces
{
    public interface IZigBeePresenter
    {
        public ZigBeeViewModel GetZigBeeViewModel();
    }
}
