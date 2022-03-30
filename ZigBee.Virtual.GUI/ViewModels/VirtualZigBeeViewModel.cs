using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZigBee.Common.WpfExtensions.Base;
using ZigBee.Core.GUI;
using ZigBee.Core.GUI.ViewModels;
using ZigBee.Core.Interfaces;
using ZigBee.Core.Models;
using ZigBee.Virtual.GUI.Views;

namespace ZigBee.Virtual.GUI.ViewModels
{
    public class VirtualZigBeeViewModel : ZigBeeViewModel
    {
        public VirtualZigBeeViewModel(ZigBeeModel model = null, ZigBeeNetworkViewModel networkModel = null) : base(model, networkModel)
        {

        }

        protected override void BuildCommands()
        {
            base.BuildCommands();
            this.EditCommand = new RelayCommand((o) =>
            {
                Window window = new VirtualZigBeeWindow(this);
                window.Show();
            });
        }

        public override void OnDataRecieved(string data)
        {
            base.OnDataRecieved(data);
            this.IOHistoryBuffer += Strings.SR.GPRecieved + ":" + data + "\n";
        }
    }
}
