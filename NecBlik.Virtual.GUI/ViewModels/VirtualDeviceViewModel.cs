using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NecBlik.Common.WpfExtensions.Base;
using NecBlik.Core.GUI;
using NecBlik.Core.GUI.ViewModels;
using NecBlik.Core.Interfaces;
using NecBlik.Core.Models;
using NecBlik.Virtual.GUI.Views;

namespace NecBlik.Virtual.GUI.ViewModels
{
    public class VirtualDeviceViewModel : DeviceViewModel
    {
        public VirtualDeviceViewModel(DeviceModel model, NetworkViewModel networkModel) : base(model, networkModel)
        {

        }

        protected override void BuildCommands()
        {
            base.BuildCommands();
            this.EditCommand = new RelayCommand((o) =>
            {
                Window window = new VirtualDeviceWindow(this);
                window.Show();
            });
        }

        public override void OnDataRecieved(string data, string sourceAddress)
        {
            base.OnDataRecieved(data,sourceAddress);
        }
    }
}
