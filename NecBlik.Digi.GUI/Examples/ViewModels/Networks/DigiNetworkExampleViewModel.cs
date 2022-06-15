using NecBlik.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecBlik.Digi.GUI.Examples.ViewModels.Networks
{
    public class DigiNetworkExampleViewModel : Digi.GUI.ViewModels.DigiZigBeeNetworkViewModel
    {
        public DigiNetworkExampleViewModel(Network network) : base(network)
        {
            this.EditCommand = new Common.WpfExtensions.Base.RelayCommand((o) =>
            {
                var window = new Digi.GUI.Views.DigiNetworkWindow(this);
                window.Show();
            });
        }
    }
}
