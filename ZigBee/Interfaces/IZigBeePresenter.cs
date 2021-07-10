using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.ViewModels;

namespace ZigBee.Interfaces
{
    interface IZigBeePresenter
    {
        public ZigBeeViewModel GetZigBeeViewModel();
    }
}
