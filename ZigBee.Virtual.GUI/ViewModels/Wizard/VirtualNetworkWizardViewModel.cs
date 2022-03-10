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

        private string networkName = "vNetwork";

        public string NetworkName
        {
            get { return networkName; }
            set { networkName = value; this.OnPropertyChanged(); }
        }

        private int virtualZigBees;
        public int VirtualZigBees
        {
            get { return virtualZigBees; }
            set { virtualZigBees = value; this.OnPropertyChanged(); }
        }
        public RelayCommand PickVirtualZigBeesCommand { get; set; }
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
