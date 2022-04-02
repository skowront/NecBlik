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
using NecBlik.Core.GUI;
using NecBlik.Core.GUI.Interfaces;

namespace NecBlik.Core.GUI.Views.Controls
{
    /// <summary>
    /// Interaction logic for ZigBeeControl.xaml
    /// </summary>
    public partial class DeviceControl : UserControl, IDevicePresenter
    {
        public DeviceControl()
        {
            InitializeComponent();
        }

        public DeviceControl(DeviceViewModel deviceViewModel)
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
