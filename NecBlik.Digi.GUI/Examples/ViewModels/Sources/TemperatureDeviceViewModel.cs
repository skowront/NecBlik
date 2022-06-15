using NecBlik.Common.WpfExtensions.Base;
using NecBlik.Core.GUI.ViewModels;
using NecBlik.Core.Models;
using NecBlik.Digi.GUI.Examples.Views.Sources;
using NecBlik.Digi.GUI.ViewModels;
using NecBlik.Virtual.GUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace NecBlik.Digi.GUI.Examples.ViewModels.Sources
{
    public class TemperatureDeviceViewModel : DigiZigBeeViewModel
    {
        private double maxRecordedTemperature = 0;
        private double minRecordedTemperature = 0;

        private double? temperature = null;
        public double? Temperature
        {
            get
            {
                return temperature;
            }
            set
            {
                if(value>this.maxRecordedTemperature)
                {
                    this.maxRecordedTemperature = value ?? this.maxRecordedTemperature;
                }
                if(value<this.minRecordedTemperature)
                {
                    this.minRecordedTemperature = value ?? this.minRecordedTemperature;
                }
                this.temperature = value;
                this.TemperatureColor = TemperatureDeviceViewModel.TemperatureToColor(this.temperature ?? 0, this.minRecordedTemperature, this.maxRecordedTemperature);
                this.OnPropertyChanged();
            }
        }

        private Color temperatureColor = Colors.White;
        public Color TemperatureColor {
            get 
            { 
                return this.temperatureColor; 
            }
            set { 
                this.temperatureColor = value; this.OnPropertyChanged(); 
            }
        } 

        public TemperatureDeviceViewModel(DeviceModel model, NetworkViewModel networkModel) : base(model, networkModel)
        {
            this.EditCommand = new RelayCommand((o) =>
            {
                var window = new TemperatureDeviceWindow(this);
                window.Show();
            });
        }

        public override void OnRecievedDataSentFromSourceDevice(string data, string sourceAddress)
        {
            var splitData = data.Split(':');
            var parsed = 0.0d;
            if (splitData.Count() > 1)
                if(double.TryParse(splitData[1],out parsed))
                    this.Temperature = parsed;
            else if (double.TryParse(data, out parsed))
                    this.Temperature = parsed;
        }

        private static Color TemperatureToColor(double temperature, double min, double max)
        {
            var color = new Color();
            color.A = 255;
            if (temperature >= 0)
            {
                color.R = 255;
                color.G = (byte)(255-(temperature / max) * 255);
                color.B = (byte)(255-(temperature / max) * 255);
            }
            else
            {
                color.R = (byte)(255-(Math.Abs(temperature) / Math.Abs(min)) * 255);
                color.G = (byte)(255-(Math.Abs(temperature) / Math.Abs(min)) * 255);
                color.B = 255;
            }
            return color;
        }
    }
}
