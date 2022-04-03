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
using NecBlik.Digi.Factories;
using NecBlik.Digi.GUI.Factories;
using NecBlik.Digi.GUI.ViewModels;
using NecBlik.Digi.GUI.ViewModels.Wizard;
using NecBlik.Digi.Models;
using NecBlik.Virtual.GUI.Factories;
using NecBlik.Virtual.GUI.ViewModels;
using NecBlik.Virtual.GUI.ViewModels.Wizard;

namespace NecBlik.Digi.GUI.Views.Wizard
{
    /// <summary>
    /// Interaction logic for VirtualNetworkWizard.xaml
    /// </summary>
    public partial class DigiNetworkWizard: IAsyncResponseProvider<VirtualNetworkViewModel, object>
    {
        DigiNetworkWizardViewModel ViewModel { get; set; }

        public DigiNetworkWizard()
        {
            InitializeComponent();
        }

        public DigiNetworkWizard(DigiNetworkWizardViewModel digiNetworkWizardViewModel)
        {
            InitializeComponent();
            this.ViewModel = digiNetworkWizardViewModel;
            this.ViewModel.Window = this;
            this.DataContext = digiNetworkWizardViewModel;
        }
        
        public async Task<VirtualNetworkViewModel> ProvideResponseAsync(object context = null)
        {
            this.ShowDialog();
            if(this.ViewModel.Committed == false)
            {
                return null;
            }

            var coordinator = new DigiZigBeeUSBCoordinator(new DigiZigBeeFactory(),
                new DigiZigBeeUSBCoordinator.DigiUSBConnectionData() { baud = this.ViewModel.BaudRate, port = this.ViewModel.SerialPortName });
            var popup = new SimpleYesNoProgressBarPopup(Strings.SR.GPPleaseWait+ "...","",Popups.Icons.InfoIcon,null,null,0,0,0,false,false);
            var network = new DigiZigBeeNetwork(coordinator, new YesNoProgressBarPopupResponseProvider(popup));
            network.DeviceCoordinatorSubtypeFactoryRule = new Core.Factories.FactoryRule() { Value = this.ViewModel.CoordinatorType, 
                CacheObjectId = coordinator.GetCacheId(),
                Property = VirtualDeviceGuiFactory.DeviceViewModelRuledProperties.ViewModel };
            await network.Initialize(coordinator);
            var factory = new DigiZigBeeGuiFactory();
            var vm = factory.NetworkViewModelBySubType(network, this.ViewModel.NetworkType);
            return vm;
        }
    }
}
