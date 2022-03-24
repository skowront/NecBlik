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

namespace ZigBee.Digi.GUI.ViewModels.Wizard
{
    public class DigiNetworkWizardViewModel:BaseViewModel
    {
        private Window window;

        public Window Window
        {
            get { return window; }
            set { window = value; }
        }

        public bool Committed { get; set; } = false;

        private string networkName = ZigBee.Digi.Resources.Resources.DefaultDigiNetworkName;

        public string NetworkName
        {
            get { return networkName; }
            set { networkName = value; this.OnPropertyChanged(); }
        }

        private string serialPortName = SerialPort.GetPortNames().FirstOrDefault("--");
        public string SerialPortName
        {
            get { return serialPortName; }  
            set { serialPortName = value; this.OnPropertyChanged(); }
        }

        private int baudRate = 9600;
        public int BaudRate
        {
            get { return baudRate; }
            set { baudRate = value; this.OnPropertyChanged(); }
        }

        public RelayCommand PickSerialPortCommand { get; set; }
        public RelayCommand PickBaudRateCommand { get; set; }
        public RelayCommand ConfirmCommand { get; set; }
        public RelayCommand AbortCommand { get; set; }
        public DigiNetworkWizardViewModel()
        {
            this.BuildCommands();
        }

        private void BuildCommands()
        {
            this.PickSerialPortCommand = new RelayCommand((o) =>
            {
                string[] ports = SerialPort.GetPortNames();
                var rp = new ListInputValuePicker();
                var result = rp.ProvideResponse(new Tuple<string,IEnumerable<string>>("Select port",ports));
                if (result == string.Empty || result == null || ports.Contains(result) == false)
                    return;
                this.SerialPortName = result;
            });

            this.PickBaudRateCommand = new RelayCommand((o) =>
            {
                var vp = new NumericResponseProvider<int>(new NumericValuePicker());
                this.BaudRate = vp.ProvideResponse();
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
