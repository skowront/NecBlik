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
    public class VirtualZigBeeViewModel : ZigBeeViewModel
    {
        public VirtualZigBeeViewModel(ZigBeeModel model, ZigBeeNetworkViewModel networkModel) : base(model, networkModel)
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

        public override void OnDataRecieved(string data, string sourceAddress)
        {
            base.OnDataRecieved(data,sourceAddress);
        }
    }
}
