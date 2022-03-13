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
using ZigBee.ZigBeeNet.Factories;
using ZigBee.ZigBeeNet.GUI.ViewModels;
using ZigBee.ZigBeeNet.GUI.ViewModels.Wizard;
using ZigBee.ZigBeeNet.Models;
using ZigBee.Virtual.GUI.ViewModels;
using ZigBee.Virtual.GUI.ViewModels.Wizard;

namespace ZigBee.ZigBeeNet.GUI.Views.Wizard
{
    /// <summary>
    /// Interaction logic for VirtualNetworkWizard.xaml
    /// </summary>
    public partial class ZBNetNetworkWizard : Window, IAsyncResponseProvider<ZBNetNetworkViewModel, object>
    {
        ZBNetNetworkWizardViewModel ViewModel { get; set; }

        public ZBNetNetworkWizard()
        {
            InitializeComponent();
        }

        public ZBNetNetworkWizard(ZBNetNetworkWizardViewModel networkWizardViewModel)
        {
            InitializeComponent();
            this.ViewModel = networkWizardViewModel;
            this.ViewModel.Window = this;
            this.DataContext = networkWizardViewModel;
        }
        
        public async Task<ZBNetNetworkViewModel> ProvideResponseAsync(object context = null)
        {
            this.ShowDialog();
            if(this.ViewModel.Committed == false)
            {
                return null;
            }

            var coordinator = new ZBNetCoordinator(new ZBNetFactory(),
                new ZBNetCoordinator.ZBNetConnectionData() { port = this.ViewModel.SerialPortName, baud = this.ViewModel.BaudRate });
            var popup = new SimpleYesNoProgressBarPopup("Please wait...","",Popups.ZigBeeIcons.InfoIcon,null,null,0,0,0,false,false);
            var network = new ZBNetNetwork(coordinator, new YesNoProgressBarPopupResponseProvider(popup));
            await network.Initialize(coordinator);
            var vm = new ZBNetNetworkViewModel(network);
            return vm;
        }
    }
}
