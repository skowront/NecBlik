using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZigBee.Common.WpfExtensions.Base;
using ZigBee.Core.GUI.ViewModels;
using ZigBee.Core.Models;
using ZigBee.Virtual.GUI.Views;
using ZigBee.Virtual.Models;

namespace ZigBee.Virtual.GUI.ViewModels
{
    public class VirtualZigBeeNetworkViewModel : ZigBeeNetworkViewModel
    {
        public ObservableCollection<VirtualZigBeeViewModel> ZigBees { get; set; } = new ObservableCollection<VirtualZigBeeViewModel>();

        public VirtualZigBeeNetworkViewModel(ZigBeeNetwork network) : base(network)
        {
            this.EditResponseProvider = new GenericResponseProvider<string, ZigBeeNetworkViewModel>((q) => {
                Window window = new VirtualZigBeeNetworkWindow(this);
                window.Show();
                return null;
            });

            this.SyncFromModel();
        }

        public void SyncFromModel()
        {
            this.ZigBees.Clear();
           
            foreach (var device in this.model.ZigBeeSources)
            {
                var vm = new VirtualZigBeeViewModel(new ZigBeeModel(device));
                this.ZigBees.Add(vm);
            }
        }
    }
}
