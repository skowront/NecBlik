using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ZigBee.Common.WpfExtensions.Interfaces;
using ZigBee.Core.Interfaces;
using ZigBee.Virtual.Factories;
using ZigBee.Virtual.GUI.Factories;
using ZigBee.Virtual.GUI.ViewModels;
using ZigBee.Virtual.GUI.ViewModels.Wizard;
using ZigBee.Virtual.Models;

namespace ZigBee.Virtual.GUI.Views.Wizard
{
    /// <summary>
    /// Interaction logic for VirtualNetworkWizard.xaml
    /// </summary>
    public partial class VirtualNetworkWizard : Window, IResponseProvider<Task<VirtualZigBeeNetworkViewModel>, object>
    {
        VirtualNetworkWizardViewModel ViewModel { get; set; }

        public VirtualNetworkWizard()
        {
            InitializeComponent();
        }

        public VirtualNetworkWizard(VirtualNetworkWizardViewModel virtualNetworkWizardViewModel)
        {
            InitializeComponent();
            virtualNetworkWizardViewModel.Window = this;
            this.ViewModel = virtualNetworkWizardViewModel;
            this.DataContext = virtualNetworkWizardViewModel;
        }
        
        public async Task<VirtualZigBeeNetworkViewModel> ProvideResponse(object context = null)
        {
            this.ShowDialog();
            if(this.ViewModel.Committed==false)
            {
                return null;
            }
            var network = new VirtualZigBeeNetwork();
            network.Name = this.ViewModel.NetworkName;
            network.ZigBeeCoordinatorSubtypeFactoryRule= new Core.Factories.FactoryRule() { Value = this.ViewModel.CoordinatorType };
            var coordinator = new VirtualZigBeeCoordinator(new VirtualZigBeeFactory());
            List<IZigBeeSource> zigBeeSources = new List<IZigBeeSource>();
            for(int i = 0; i<this.ViewModel.VirtualZigBees; i++)
            {
                var source = new VirtualZigBeeSource();
                zigBeeSources.Add(source);
            }
            coordinator.SetDevices(zigBeeSources);
            await network.SetCoordinator(coordinator);
            var factory = new VirtualZigBeeGuiFactory();
            return factory.NetworkViewModelBySubType(network,this.ViewModel.NetworkType);
        }
    }
}
