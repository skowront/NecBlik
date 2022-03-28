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
using ZigBee.Common.WpfElements;
using ZigBee.Common.WpfElements.ResponseProviders;
using ZigBee.Common.WpfExtensions.Interfaces;
using ZigBee.PyDigi.Factories;
using ZigBee.PyDigi.GUI.ViewModels;
using ZigBee.PyDigi.GUI.ViewModels.Wizard;
using ZigBee.PyDigi.Models;
using ZigBee.Virtual.GUI.ViewModels;
using ZigBee.Virtual.GUI.ViewModels.Wizard;

namespace ZigBee.PyDigi.GUI.Views.Wizard
{
    /// <summary>
    /// Interaction logic for VirtualNetworkWizard.xaml
    /// </summary>
    public partial class PyDigiNetworkWizard : Window, IAsyncResponseProvider<PyDigiZigBeeNetworkViewModel, object>
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
        
        public async Task<PyDigiZigBeeNetworkViewModel> ProvideResponseAsync(object context = null)
        {
            this.ShowDialog();
            if(this.ViewModel.Committed == false)
            {
                return null;
            }

            var coordinator = new PyDigiZigBeeUSBCoordinator(new PyDigiZigBeeFactory(),
                new PyDigiZigBeeUSBCoordinator.PyDigiUSBConnectionData() { baud = this.ViewModel.BaudRate, port = this.ViewModel.SerialPortName });
            var popup = new SimpleYesNoProgressBarPopup(Strings.SR.GPPleaseWait+ "...","",Popups.ZigBeeIcons.InfoIcon,null,null,0,0,0,false,false);
            var network = new PyDigiZigBeeNetwork(coordinator, new YesNoProgressBarPopupResponseProvider(popup));
            await network.Initialize(coordinator);
            var vm = new PyDigiZigBeeNetworkViewModel(network);
            return vm;
        }
    }
}
