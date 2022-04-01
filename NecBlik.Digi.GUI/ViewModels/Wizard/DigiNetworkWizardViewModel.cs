using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.IO.Ports;
using System.Windows;
using NecBlik.Common.WpfExtensions.Base;
using NecBlik.Common.WpfElements.PopupValuePickers;
using NecBlik.Common.WpfElements.PopupValuePickers.ResponseProviders;
using Microsoft.Win32;
using NecBlik.Core.Helpers;

namespace NecBlik.Digi.GUI.ViewModels.Wizard
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

        private string networkName = NecBlik.Digi.Resources.Resources.DefaultDigiNetworkName;

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
            var portInfoList = SerialPortHelper.GetSerialPorts();
            if (portInfoList.Count > 0)
                this.SerialPortName = portInfoList.Where((o) => o.description.Contains(NecBlik.Digi.Resources.Resources.AutoDetectionFilterUSBSerialPort)).FirstOrDefault().name;
            else
                this.SerialPortName = SerialPort.GetPortNames().FirstOrDefault("--");
            this.BuildCommands();
        }

        private void BuildCommands()
        {
            this.PickSerialPortCommand = new RelayCommand((o) =>
            {
                List<string> portNames = new();
                var portInfoList = SerialPortHelper.GetSerialPorts();
                Dictionary<string, string> dict = new Dictionary<string, string>();
                foreach (var item in portInfoList)
                {
                    var full = Strings.SR.GPName + ": " + item.name + "\n" + Strings.SR.GPPID + ": " + item.pid + "\n" 
                    + Strings.SR.GPVID +": " + item.vid + "\n" + Strings.SR.GPDescription + ": " + item.description;
                    dict[full] = item.name;
                    portNames.Add(full);
                }

                var rp = new ListInputValuePicker();
                var result = rp.ProvideResponse(new Tuple<string, IEnumerable<string>>(Strings.SR.GPSelectPort, portNames));
                if (result == string.Empty || result == null || portNames.Contains(result) == false)
                    return;
                this.SerialPortName = dict[result];
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
