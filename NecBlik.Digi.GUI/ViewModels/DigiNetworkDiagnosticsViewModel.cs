using NecBlik.Common.WpfExtensions.Base;
using NecBlik.Core.Interfaces;
using NecBlik.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NecBlik.Digi.GUI.ViewModels
{
    public class DigiNetworkDiagnosticsViewModel: BaseViewModel, IDisposable
    {
        public List<string> AvailableDevices
        {
            get { return new List<string>(this.NetworkViewModel.Devices.Select((x) => { return x.Address; })); }
        }

        public DigiZigBeeNetworkViewModel NetworkViewModel
        {
            get;set;
        }

        public ThroughputViewModel ThroughputVM
        {
            get; set;
        }

        public RangeTestViewModel RangeTestVM
        {
            get; set;
        }

        public DigiNetworkDiagnosticsViewModel(DigiZigBeeNetworkViewModel networkViewModel)
        {
            this.NetworkViewModel = networkViewModel;
            this.ThroughputVM = new ThroughputViewModel(networkViewModel);
            this.RangeTestVM = new RangeTestViewModel(networkViewModel);
        }

        ~DigiNetworkDiagnosticsViewModel()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            this.ThroughputVM.Dispose();
            this.RangeTestVM.Dispose();
        }
    }
}
