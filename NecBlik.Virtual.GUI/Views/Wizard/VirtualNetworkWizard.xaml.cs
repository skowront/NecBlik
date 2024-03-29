﻿using System.Collections.Generic;
using System.Threading.Tasks;
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
    public partial class VirtualNetworkWizard: IResponseProvider<Task<VirtualNetworkViewModel>, object>
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
            var coordinator = new VirtualCoordinator(new VirtualDeviceFactory());
            network.DeviceCoordinatorSubtypeFactoryRule= new Core.Factories.FactoryRule() { Value = this.ViewModel.CoordinatorType, 
                CacheObjectId = coordinator.GetCacheId(), 
                Property = VirtualDeviceGuiFactory.DeviceViewModelRuledProperties.ViewModel };
            List<IDeviceSource> sources = new List<IDeviceSource>();
            for (int i = 0; i < this.ViewModel.VirtualDevices; i++)
            {
                var source = new VirtualDevice();
                sources.Add(source);
            }
            coordinator.SetDevices(sources);
            await network.SetCoordinator(coordinator);
            await network.SyncCoordinator();
            var factory = new VirtualDeviceGuiFactory();
            var vm = factory.NetworkViewModelBySubType(network,this.ViewModel.NetworkType);
            vm.SyncFromModel();
            return vm;
        }
    }
}
