using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NecBlik.Common.WpfElements.PopupValuePickers;
using NecBlik.Common.WpfElements.PopupValuePickers.ResponseProviders;
using NecBlik.Common.WpfExtensions.Base;
using NecBlik.Core.GUI.ViewModels;
using NecBlik.Core.Helpers;
using NecBlik.Core.Models;
using NecBlik.Digi.Models;

namespace NecBlik.Digi.GUI.ViewModels
{
    public class DigiZigBeeCoordinatorViewModel:DigiZigBeeViewModel
    {
        public int BaudRate
        {
            get
            {
                if(this.Model.DeviceSource is DigiZigBeeUSBCoordinator)
                {
                    var c = this.Model.DeviceSource as DigiZigBeeUSBCoordinator;
                    return c.connectionData.baud;
                }
                return 0;
            }
            set
            {
                if (this.Model.DeviceSource is DigiZigBeeUSBCoordinator)
                {
                    var c = this.Model.DeviceSource as DigiZigBeeUSBCoordinator;
                    c.connectionData.baud = value;
                    c.SetNewConnectionData(c.connectionData);
                }
                this.OnPropertyChanged();
            }
        }

        public string SerialPort
        {
            get
            {
                if (this.Model.DeviceSource is DigiZigBeeUSBCoordinator)
                {
                    var c = this.Model.DeviceSource as DigiZigBeeUSBCoordinator;
                    return c.connectionData.port;
                }
                return string.Empty;
            }
            set
            {
                if (this.Model.DeviceSource is DigiZigBeeUSBCoordinator)
                {
                    var c = this.Model.DeviceSource as DigiZigBeeUSBCoordinator;
                    c.connectionData.port = value;
                    c.SetNewConnectionData(c.connectionData);
                }
                this.OnPropertyChanged();
            }
        }

        public RelayCommand PickSerialPortCommand { get; set; }
        public RelayCommand PickBaudRateCommand { get; set; }

        public DigiZigBeeCoordinatorViewModel(DeviceModel model = null, NetworkViewModel networkModel = null) : base(model, networkModel)
        {
            this.PickSerialPortCommand = new RelayCommand((o) =>
            {
                List<string> portNames = new();
                var portInfoList = SerialPortHelper.GetSerialPorts();
                Dictionary<string, string> dict = new Dictionary<string, string>();
                foreach (var item in portInfoList)
                {
                    var full = Strings.SR.GPName + ": " + item.name + "\n" + Strings.SR.GPPID + ": " + item.pid + "\n"
                    + Strings.SR.GPVID + ": " + item.vid + "\n" + Strings.SR.GPDescription + ": " + item.description;
                    dict[full] = item.name;
                    portNames.Add(full);
                }

                var rp = new ListInputValuePicker();
                var result = rp.ProvideResponse(new Tuple<string, IEnumerable<string>>(Strings.SR.GPSelectPort, portNames));
                if (result == string.Empty || result == null || portNames.Contains(result) == false)
                    return;
                this.SerialPort = dict[result];
            });

            this.PickBaudRateCommand = new RelayCommand((o) =>
            {
                var vp = new NumericResponseProvider<int>(new NumericValuePicker());
                this.BaudRate = vp.ProvideResponse();
            });
        }

        public override void Send()
        {
            if (this.SelectedDestinationAddress == this.Address)
            {
                this.AddOutgoingHistoryBufferEntry(this.OutputBuffer, this.Address);
                this.OnDataRecieved(this.OutputBuffer, this.Address);
                return;
            }
            if (this.SelectedDestinationAddress == Core.GUI.Strings.SR.Broadcast)
            {
                var aaddr = this.GetAvailableDestinationAdresses();
                foreach (var item in aaddr)
                {
                    this.Model.DeviceSource.Send(this.OutputBuffer, item);
                    this.AddOutgoingHistoryBufferEntry(this.OutputBuffer, item);
                }
                var sources = this.Network?.GetDeviceViewModels();
                if(sources!=null)
                {
                    foreach(var source in sources)
                    {
                        source.OnDataSent(this.OutputBuffer,this.Address);
                    }
                }
                return;
            }
            this.Model.DeviceSource.Send(this.OutputBuffer, this.SelectedDestinationAddress);
            var vm = this.Network?.GetDeviceViewModels().Where((x) => { return x.Address == this.SelectedDestinationAddress; }).FirstOrDefault();
            this.AddOutgoingHistoryBufferEntry(this.OutputBuffer, this.SelectedDestinationAddress);
            if (vm != null)
                vm.OnDataSent(this.OutputBuffer, this.Address);
            else
                this.AddOutgoingHistoryBufferEntry(NecBlik.Core.GUI.Strings.SR.GPDeviceUnavailable,this.SelectedDestinationAddress);
            this.OutputBuffer = string.Empty;
        }

        public string SendAtCommand(DigiATCommandViewModel atCommand)
        {
            if(this.Model.DeviceSource is DigiZigBeeUSBCoordinator)
            {
                return (this.Model.DeviceSource as DigiZigBeeUSBCoordinator)?.SendATCommandPacket(atCommand.Address,atCommand.Command,atCommand.Parameter) ?? string.Empty;
            }
            return string.Empty;
        }


        public override void OnDataSent(string data, string sourceAddress)
        {
            this.OnDataRecieved(data,sourceAddress);
        }
    }
}
