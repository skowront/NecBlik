using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO.Ports;
using ZigBee.Common.WpfExtensions.Base;
using ZigBee.Common.WpfElements.PopupValuePickers;
using ZigBee.Common.WpfElements.PopupValuePickers.ResponseProviders;
using ZigBee.Virtual.GUI.Factories;

namespace ZigBee.Virtual.GUI.ViewModels.Wizard
{
    public class VirtualNetworkWizardViewModel:BaseViewModel
    {
        private Window window;

        public Window Window
        {
            get { return window; }
            set { window = value; }
        }
        public bool Committed { get; set; } = false;

        private string networkName = Resources.Resources.VirtualNetworkDefaultName;

        public string NetworkName
        {
            get { return networkName; }
            set { networkName = value; this.OnPropertyChanged(); }
        }

        private string networkType;
        public string NetworkType
        {
            get { return networkType; }
            set { networkType = value; this.OnPropertyChanged(); }
        }

        private string coordinatorType;
        public string CoordinatorType
        {
            get { return coordinatorType; }
            set { coordinatorType = value; this.OnPropertyChanged(); }
        }

        private int virtualZigBees;
        public int VirtualZigBees
        {
            get { return virtualZigBees; }
            set { virtualZigBees = value; this.OnPropertyChanged(); }
        }
        public RelayCommand PickVirtualZigBeesCommand { get; set; }
        public RelayCommand PickNetworkTypeCommand { get; set; }
        public RelayCommand PickCoordinatorTypeCommand { get; set; }
        public RelayCommand ConfirmCommand { get; set; }
        public RelayCommand AbortCommand { get; set; }

        public VirtualNetworkWizardViewModel(Window window=null)
        {
            this.window = window;
            this.BuildCommands();
        }

        private void BuildCommands()
        {
            this.PickVirtualZigBeesCommand = new RelayCommand((o) =>
            {
                var vp = new NumericResponseProvider<int>(new NumericValuePicker());
                this.VirtualZigBees = vp.ProvideResponse();
            });

            this.PickNetworkTypeCommand = new RelayCommand((o) =>
            {
                var rp = new ListInputValuePicker();
                var fac = new VirtualZigBeeGuiFactory();
                var availableTypes = fac.GetAvailableNetworkViewModels();
                this.NetworkType = rp.ProvideResponse(new Tuple<string, IEnumerable<string>>(string.Empty,availableTypes));
            });

            this.PickCoordinatorTypeCommand = new RelayCommand((o) =>
            {
                var rp = new ListInputValuePicker();
                var fac = new VirtualZigBeeGuiFactory();
                var availableTypes = fac.GetAvailableZigBeeViewModels();
                this.CoordinatorType = rp.ProvideResponse(new Tuple<string, IEnumerable<string>>(string.Empty, availableTypes));
            });

            this.ConfirmCommand = new RelayCommand((o) =>
            {
                this.Committed = true;
                this.window?.Close();
            });

            this.AbortCommand = new RelayCommand((o) =>
            {
                this.Committed = false;
                this.window?.Close();
            });
        }

    }
}
