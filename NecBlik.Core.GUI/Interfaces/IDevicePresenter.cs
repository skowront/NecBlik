using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NecBlik.Core.GUI;

namespace NecBlik.Core.GUI.Interfaces
{
    public interface IDevicePresenter
    {
        public DeviceViewModel GetDeviceViewModel();
    }
}
