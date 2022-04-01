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
using NecBlik.Common.WpfExtensions.Interfaces;
using NecBlik.Core.Interfaces;
using NecBlik.Virtual.Factories;
using NecBlik.Virtual.GUI.Factories;
using NecBlik.Virtual.GUI.ViewModels;
using NecBlik.Virtual.GUI.ViewModels.Wizard;
using NecBlik.Virtual.Models;

namespace NecBlik.Virtual.GUI.Views.Wizard
{
    /// <summary>
    /// Interaction logic for VirtualNetworkWizard.xaml
    /// </summary>
    public partial class VirtualNetworkWizard : Window, IResponseProvider<Task<VirtualNetworkViewModel>, object>
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
        
        public async Task<VirtualNetworkViewModel> ProvideResponse(object context = null)
        {
            this.ShowDialog();
            if(this.ViewModel.Committed==false)
            {
                return null;
            }
            var network = new VirtualNetwork();
            network.Name = this.ViewModel.NetworkName;
            network.DeviceCoordinatorSubtypeFactoryRule= new Core.Factories.FactoryRule() { Value = this.ViewModel.CoordinatorType };
            var coordinator = new VirtualCoordinator(new VirtualDeviceFactory());
            List<IDeviceSource> sources = new List<IDeviceSource>();
            for(int i = 0; i<this.ViewModel.VirtualDevices; i++)
            {
                var source = new VirtualDevice();
                sources.Add(source);
            }
            coordinator.SetDevices(sources);
            await network.SetCoordinator(coordinator);
            var factory = new VirtualDeviceGuiFactory();
            return factory.NetworkViewModelBySubType(network,this.ViewModel.NetworkType);
        }
    }
}
