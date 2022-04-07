using NecBlik.Core.GUI;
using NecBlik.Core.GUI.Interfaces;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NecBlik.Digi.GUI.Examples.Views.Controls
{
    /// <summary>
    /// Interaction logic for TemperatureControl.xaml
    /// </summary>
    public partial class TemperatureControl : UserControl, IDeviceControl
    {
        public TemperatureControl()
        {
            InitializeComponent();
        }

        public TemperatureControl(DeviceViewModel deviceViewModel)
        {
            InitializeComponent();
            this.DataContext = deviceViewModel;
        }

        public DeviceViewModel GetDeviceViewModel()
        {
            return this.DataContext as DeviceViewModel;
        }
    }
}
