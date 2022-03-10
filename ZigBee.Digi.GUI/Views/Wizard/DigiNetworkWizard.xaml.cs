﻿using System;
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
using ZigBee.Digi.Factories;
using ZigBee.Digi.GUI.ViewModels;
using ZigBee.Digi.GUI.ViewModels.Wizard;
using ZigBee.Digi.Models;
using ZigBee.Virtual.GUI.ViewModels;
using ZigBee.Virtual.GUI.ViewModels.Wizard;

namespace ZigBee.Digi.GUI.Views.Wizard
{
    /// <summary>
    /// Interaction logic for VirtualNetworkWizard.xaml
    /// </summary>
    public partial class DigiNetworkWizard : Window, IAsyncResponseProvider<DigiZigBeeNetworkViewModel, object>
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
        
        public async Task<DigiZigBeeNetworkViewModel> ProvideResponseAsync(object context = null)
        {
            this.ShowDialog();
            if(this.ViewModel.Committed == false)
            {
                return null;
            }

            var coordinator = new DigiZigBeeUSBCoordinator(new DigiZigBeeFactory(),
                new DigiZigBeeUSBCoordinator.DigiUSBConnectionData() { baud = this.ViewModel.BaudRate, port = this.ViewModel.SerialPortName });
            var popup = new SimpleYesNoProgressBarPopup("Please wait...","",Popups.ZigBeeIcons.InfoIcon,null,null,0,0,0,false,false);
            var network = new DigiZigBeeNetwork(coordinator, new YesNoProgressBarPopupResponseProvider(popup));
            await network.Initialize(coordinator);
            var vm = new DigiZigBeeNetworkViewModel(network);
            return vm;
        }
    }
}