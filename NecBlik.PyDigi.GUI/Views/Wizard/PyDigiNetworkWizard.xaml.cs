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
using NecBlik.Common.WpfElements;
using NecBlik.Common.WpfElements.ResponseProviders;
using NecBlik.Common.WpfExtensions.Interfaces;
using NecBlik.PyDigi.Factories;
using NecBlik.PyDigi.GUI.Factories;
using NecBlik.PyDigi.GUI.ViewModels;
using NecBlik.PyDigi.GUI.ViewModels.Wizard;
using NecBlik.PyDigi.Models;
using NecBlik.Virtual.GUI.Factories;
using NecBlik.Virtual.GUI.ViewModels;
using NecBlik.Virtual.GUI.ViewModels.Wizard;

namespace NecBlik.PyDigi.GUI.Views.Wizard
{
    /// <summary>
    /// Interaction logic for VirtualNetworkWizard.xaml
    /// </summary>
    public partial class PyDigiNetworkWizard : IAsyncResponseProvider<VirtualNetworkViewModel, object>
    {
        PyDigiNetworkWizardViewModel ViewModel { get; set; }

        public PyDigiNetworkWizard()
        {
            InitializeComponent();
        }

        public PyDigiNetworkWizard(PyDigiNetworkWizardViewModel digiNetworkWizardViewModel)
        {
            InitializeComponent();
            this.ViewModel = digiNetworkWizardViewModel;
            this.ViewModel.Window = this;
            this.DataContext = digiNetworkWizardViewModel;
        }
        
        public async Task<VirtualNetworkViewModel> ProvideResponseAsync(object context = null)
        {
            this.ShowDialog();

            if (this.ViewModel.Committed == false)
            {
                return null;
            }

            var coordinator = new PyDigiZigBeeUSBCoordinator(new PyDigiZigBeeFactory(),
                new PyDigiZigBeeUSBCoordinator.PyDigiUSBConnectionData() { baud = this.ViewModel.BaudRate, port = this.ViewModel.SerialPortName });
            var popup = new SimpleYesNoProgressBarPopup(Strings.SR.GPPleaseWait+ "...","",Popups.Icons.InfoIcon,null,null,0,0,0,false,false);
            var network = new PyDigiZigBeeNetwork(coordinator, new YesNoProgressBarPopupResponseProvider(popup));
            network.DeviceCoordinatorSubtypeFactoryRule = new Core.Factories.FactoryRule()
            {
                Value = this.ViewModel.CoordinatorType,
                CacheObjectId = coordinator.GetCacheId(),
                Property = VirtualDeviceGuiFactory.DeviceViewModelRuledProperties.ViewModel
            };
            await network.Initialize(coordinator);
            var factory = new PyDigiZigBeeGuiFactory();
            var vm = factory.NetworkViewModelBySubType(network, this.ViewModel.NetworkType);
            return vm;
        }
    }
}
